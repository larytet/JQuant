
using System;
using System.Text;
using System.Threading;
using System.ComponentModel;
using JQuant;

#if USEFMRSIM
using TaskBarLibSim;
#else
using TaskBarLib;
#endif

namespace FMRShell
{
	//here we take care of the orders processing by FMR's TaksBar
	//refer also to the order flow charts and tables https://www.assembla.com/spaces/jquant/documents

	/// <summary>
	/// List of the events possible in the orders FSM. There are two different sources of events:
	///  - Events we get from the broker and the stock exchange - like fills or approvals
	///  - Events that we get from the Algo - directions to place / cancel / update / cleanup
	///  
	/// * The Algo usually passes a reference to RezefOrder object (FSM's internal object) in a form of cookie, 
	///   so no need to lookup for the correct object. In case of a new order directive from the Algo, no RezefOrder
	///   instance exists yet, so Algo passes LMTOrderParameters object which contains all the information needed to 
	///   create such an instance.
	/// 
	/// * The exchange events are obtained either from calling TaskBar API's methods or via polling the remote 
	///   server (then they come in a form of TaskBar struct). In order to relate event from poll to the right 
	///   RezefOrder instance, a lookup in the hash table is performed, matching the new data from the poll and
	///   the instance by unique id - AsmachtaFMR.
	/// </summary>
	public enum OrderFSMEvent
	{
		[Description("New order initialized and sent to the remote server for trading")]  //algo event
		Init,
		[Description("Internal fatal error on broker's server")]
		FatalBroker,
		[Description("Successfully passed FMR (broker) checks")]    //only for new orders. means received AsmachtaFMR
		ApproveBroker,
		[Description("Fatal error on TASE")]
		FatalTASE,
		[Description("New Order approved by TASE for trading")]
		ApproveTrading,
		[Description("Sent cancellation request of an existing to the remote server")]    //algo event
		Cancel,
		[Description("Sent update request of an existing order to the remote server")]    //algo event
		Update,
		[Description("Update failed at TASE server")]  // may indicate a fill
		UpdateTimeOut,
		[Description("Cancel failed at TASE server")]  // may indicate a fill
		CancelTimeOut,
		[Description("Cancelation of existing order approved by TASE")]
		ApproveCancel,
		[Description("Order partially executed")]
		PartialFill,
		[Description("Order fully executed")]
		CompleteFill,
		[Description("Update/cancel timeout timer expired")]
		TimerUpdateCancel,
		[Description("The algo/operator asked for cleanup")]	//algo event - directions to the exit
		Clean,
		[Description("Forced reset by the operator")]	//algo or manual intervention - directions to the exit
		ForcedClean,
		[Description("Watchdog timer expired")]
		Watchdog,
	}

	/// <summary>
	/// states of the orders FSM
	/// </summary>
	public enum OrderFSMState
	{
		[Description("No pending orders in the FSM, not an order state")]   //this is the entry/exit point FSM
		IDLE,
		[Description("Order initialized with all the data and submitted to the remote server, waiting for response")]
		SENT,
		[Description("Waiting for approval by TASE")]       //the approval can be for trading (either brand new order or updated one) or cancel
		WaitingTASE,
		[Description("Approved and placed for trading, waiting fill")] //partial fill won't change this state
		WaitingFill,
		[Description("Submitted for cancellation, not approved yet")]   //at this stage we can get full or partial fill, TASE or internal error
		CANCELING,
		[Description("Submitted update for the order, not approved yet")]   //at this stage we can get full or partial fill, TASE or internal error
		UPDATING,
		[Description("The order was fully executed")]
		EXECUTED,
		[Description("The order was canceled")]
		CANCELED,
		[Description("Unsolvable within FSM, operator interfernce required")]
		FATAL,
	}

	/// <summary>
	/// this type is used for passing the events and event data to the Orders FSM mailbox
	/// </summary>
	public class OrderFSMMessage
	{
		public OrderFSMMessage(OrderFSMEvent msgEvent, object msgData)
		{
			this.msgEvent = msgEvent;
			this.msgData = msgData;
		}
		/// <summary>
		/// i am going to switch by event first
		/// depending on the event, data can contain different things
		/// </summary>
		public OrderFSMEvent msgEvent;

		/// <summary>
		/// Actual type of the data depends on the message event and its source 
		/// </summary>
		public object msgData;
	}

	public class OrderEventNotifierMessage
	{
		public OrderEventNotifierMessage(OrderProcessorEvent _event)
		{
			this.orderEvent = _event;
			this.data = null;
		}

		public OrderEventNotifierMessage(OrderProcessorEvent _event, object _data)
		{
			this.orderEvent = _event;
			this.data = _data;
		}

		public OrderProcessorEvent orderEvent;
		public object data;
	}

	/// <summary>
	/// implements the common functionality for all FMR's orders
	/// this class is not to be used directly, but to be inherited by 
	/// either maof or rezef order types. 
	/// </summary>
	public abstract class FMROrder : LimitOrderBase
	{
		protected FMROrder(LMTOrderParameters orderParams, Connection connection)
			: base(orderParams)
		{
			//initialize FMR-specific data to be defualt values
			//if order processing logic will require, either FSM 
			//or TaskBar will set them to something else
			this.VBMsg = default(string);
			this.ErrNO = default(int);
			this.ErrorType = default(OrdersErrorTypes);
			this.OrderID = 0;   //make sure that the broker's tests don't fail - start from the begining
			this.AuthUser = default(string);
			this.AuthPassword = default(string);
			this.ReEnteredValue = default(string);

			//initialize the state
			this.OrderState = OrderFSMState.IDLE;
		}

		/// <summary>
		/// FSM state 
		/// </summary>
		public OrderFSMState OrderState
		{
			get;
			set;
		}

		//Special variables required by FMR to treat internal errors:

		/// <summary>
		/// message recieved in case of internal error, initially is
		/// an empty string, 'out' parameter required by SensOrderRZ
		/// </summary>
		public string VBMsg;

		/// <summary>
		/// internal error code. error codes are grouped into ErrorType, 
		/// each of which is processed in similar manner
		/// </summary>
		public int ErrNO;

		/// <summary>
		/// taskbar internal error type: fatal/confirmation/alert/re-enter/'password required'
		/// </summary>
		public OrdersErrorTypes ErrorType;

		/// <summary>
		/// internal order id assigned by the TaskBar for any order in process of internal FMR tests, 
		/// needed only for internal errors corrections processing, denotes the stage at which internal checks faied, 
		/// so FMR server is able to start its checks from the point of failure, in case we correct and re-submit the order,
		/// and skip what was successfully checked previously. 
		/// Important - be sure to reset OrderId to zero once the order passes all the checks - value other than zero can cause 
		/// problems if we will try to update or cancel this order in the future.
		/// </summary>
		public int OrderID;

		/// <summary>
		/// this parameter is username required by the TaskBarLib.UserClass.SendOrderRZ method (and other *sendorder* methods) 
		/// in case additional authorization is needed ('PasswordReq' internal error)
		/// here we use just an empty string to put it as a parameterr required by the sendorder; 
		/// if needed, user and pass are retrieved directly from the Connection instance (to which the FSM has a reference)
		/// </summary>
		public string AuthUser
		{
			get;
			protected set;
		}

		/// <summary>
		/// this parameter is username required by the TaskBarLib.UserClass.SendOrderRZ method (and other *sendorder* methods) 
		/// in case additional authorization is needed ('PasswordReq' internal error)
		/// here we use just an empty string to put it as a parameterr required by the sendorder; 
		/// if needed, user and pass are retrieved directly from the Connection instance (to which the FSM has a reference)
		/// </summary>
		public string AuthPassword
		{
			get;
			protected set;
		}

		/// <summary>
		/// it's a placeholder for data used for resubmitting orders in case 
		/// of FMR internal error which requires correction of some parameter.
		/// in practice may contain only the following data:
		///  - corrected price or quantity (in case of ReEnter error)
		///  - "YES" or "NO" strings (note caps), in case of Confirmation error 
		/// </summary>
		public string ReEnteredValue
		{
			get;
			set;
		}

		/// <value>
		/// Our internal unique order Id we use for analyzing logs data. Ids start from zero.
		/// </value>
		public long Id;

		/// <summary>
		/// keeps the last event, for logging purposes
		/// </summary>
		public OrderFSMEvent lastEvent
		{
			get;
			set;
		}
	}//class FMROrder

}//namespace FMRShell
