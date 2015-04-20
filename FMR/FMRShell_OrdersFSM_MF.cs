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
	/// <summary>
	/// The FSM for Maof trading orders
	/// </summary>
	public class MaofOrderFSM : MailboxThread<OrderFSMMessage>, IDisposable
	{

		/// <summary>
		/// Translates the TaskBarLib.MOFINQType (structure returned when polling the server for orders events)
		/// into delimited string. This one I need for logging purposes.
		/// </summary>
		public class MOFINQTypeToString : StructToString<MOFINQType>
		{
			public MOFINQTypeToString(string delimiter)
				: base(delimiter)
			{
			}
		}

		/// <summary>
		/// Represents a single trading directive in the Maof options. 
		/// Essentially it's a data container, containing all the order-related data.
		/// REferences to the objetcs of this type are kept inside a hash table in the RezefOrderFSM,
		/// as long as the order is active (not executed, canceled or encountered a fatal error)
		/// RezefOrderFSM takes care of the porcessing, logging and reporting.
		/// Note all members are public since I want the FSM be able to operate them.
		/// </summary>
		public class MaofOrder : FMROrder, ILoggerData
		{
			/// <summary>
			/// constructor 
			/// </summary>
			/// <param name="security">a <see cref="JQuant.Security"/> object</param>
			/// <param name="trType">a <see cref="JQuant TransactionType"/> -either buy or sell</param>
			/// <param name="quantity">a <see cref="System.Int32"/>, how many units of security to trade</param>
			/// <param name="LMTPrice">a <see cref="System.Double"/>, limit price</param>
			/// <param name="connection">a <see cref="FMRShell.Connection"/>, connection to the current TaskBar instance</param>
			public MaofOrder(LMTOrderParameters orderParams, Connection connection)
				: base(orderParams, connection)
			{
				//initialize TaskBar maof order placeholder instance ... 
				mf_order = new MaofOrderType();
				//... and fill its parameters  with data.
				mf_order.Option = orderParams.Security.IdNum.ToString();
				if (orderParams.TransactionType == TransactionType.BUY)
					mf_order.ammount = orderParams.Quantity.ToString();
				else if (orderParams.TransactionType == TransactionType.SELL)
					mf_order.ammount = (-orderParams.Quantity).ToString();
				mf_order.price = orderParams.Price.ToString();
				mf_order.Account = connection.Parameters.Account;
				mf_order.Branch = connection.Parameters.Branch;
				mf_order.Sug_Pkuda = "LMT";//this is a common case, we will change this if "IOC" or "FOK" will be in use
				mf_order.operation = "N";

				mf_order.Asmachta="";
				mf_order.AsmachtaFmr="";
				mf_order.OrderID=0;
				mf_order.Pass=0;

				// generate internal order ID
				Id = JQuant.UniqueId.Get();

				//set order state to IDLE. this is the only place outside the FSM where state is manipulated
				//because I need entry point to the FSM
				this.OrderState = OrderFSMState.IDLE;
			}

			public static string LogLegend()
			{
				StringBuilder sb = new StringBuilder(250);
				sb.Append("Timestamp,");
				sb.Append("Ticks,");
				sb.Append("IdNum,");
				sb.Append("TransactionType,");
				sb.Append("Quantity,");
				sb.Append("Price,");
				sb.Append("Id,");
				sb.Append("AsmachtaFMR,");
				sb.Append("AsmachtaRezef,");
				sb.Append("OrderState,");
				sb.Append("lastEvent,");
				sb.Append("ErrNO,");
				sb.Append("ErrorType,");
				sb.Append("OrderID,");
				sb.Append("ReEnteredValue,");
				sb.Append("VBMsg,");

				MOFINQTypeToString rs = new MOFINQTypeToString(",");
				sb.Append(rs.Legend);

				return sb.ToString();
			}

			public string ToLogString()
			{
				StringBuilder sb = new StringBuilder(250);

				sb.Append(JQuant.DateTimePrecise.Now.ToString("hh:mm:ss.fff"));     //timestamp
				sb.Append(",");
				sb.Append(DateTime.UtcNow.Ticks);                                   //ticks
				sb.Append(",");
				sb.Append(this.Security.IdNum);                                     //what security
				sb.Append(",");
				sb.Append(this.TransactionType);                                    //buy or sell
				sb.Append(",");
				sb.Append(this.Quantity);                                           //quantity
				sb.Append(",");
				sb.Append(this.Price);                                              //LMT price
				sb.Append(",");
				sb.Append(this.Id);                                                 //different ids
				sb.Append(",");
				sb.Append(this.AsmachtaFMR);
				sb.Append(",");
				sb.Append(this.AsmachtaRezef);
				sb.Append(",");
				sb.Append(this.OrderState);                                         //FSM state
				sb.Append(",");
				sb.Append(this.lastEvent);                                          //last FSM event
				sb.Append(",");
				sb.Append(this.ErrNO);                                              //internal FMR data
				sb.Append(",");
				sb.Append(this.ErrorType);
				sb.Append(",");
				sb.Append(this.OrderID);
				sb.Append(",");
				sb.Append(this.ReEnteredValue);
				sb.Append(",");
				sb.Append(this.VBMsg);

				if (!lastPoll.Equals(default(MOFINQType)))
				{
					MOFINQTypeToString _pollString = new MOFINQTypeToString(",");
					_pollString.Init(this.lastPoll);

					sb.Append(",");
					sb.Append(_pollString.Values);
				}
				return sb.ToString();
			}

			/// <summary>
			/// TaskBar struct representing Maof order, used for sending the trading directive to the API.
			/// </summary>
			public MaofOrderType mf_order;
			
			//various order reference IDs

			/// <summary>
			/// TASE reference number, this one may change over the course of trading
			/// indeed, every TASE event causes a change in its value. We keep it for logging purposes
			/// and also for update/cancel purposes, as the API requires the most recent value of it.
			/// AsmachtaFMR is the unique order id recognized by the API. Until we obtain it, use Id instead
			/// </summary>
			public int AsmachtaRezef
			{
				get;
				set;
			}

			/// <summary>
			/// At what price order get FILL
			/// </summary>
			public double PriceFill { get; set; }

			/// <summary>
			/// Size of the position
			/// </summary>
			public int QuantityFill { get; set; }


			//the following are fields, not properties, since they are used as 'out' params by the API

			/// <summary>
			/// Exchange member's internal ref. no., will serve us as our primary order id.
			/// Serves us as the key in the hash table of active orders.
			/// Until valid value is set to it, we use Id instead. After it's obtained, we add a 
			/// reference to the MaofOrder to the hash table, because TASE events can arrive from the polls.
			/// </summary>
			public int AsmachtaFMR;

			/// <summary>
			/// a delegate used by the FSM to inform other objects (e.g. Algo) about its events
			/// </summary>
			public OrderEventNotifier eventNotifier;

			/// <summary>
			/// flag for orders sent from automatic machines - always set it = 1
			/// </summary>
			public int SPCOrder;

			/// <summary>
			/// keeps the reference to an object recognized by orders FSM client - usually the Algo
			/// </summary>
			public object eventNotifierCookie;

						/// <summary>
			/// keeps the result of the last poll of the order stream
			/// can be used for logging or for checking the poll data as needed
			/// </summary>
			public MOFINQType lastPoll
			{
				get;
				set;
			}
		}//class MaofOrder

		#region outside API;
		//here comes all the functionality used by clients outside the FSM - usually the Algo

		/// <summary>
		/// Outside client (the Algo) will call this method to tell the FSM 
		/// to create a brand new order and submit it for trading to the exchange.
		/// </summary>
		/// <param name="p">a <see cref="JQuant.LMTOrderParameters"/></param>
		/// <param name="orderId">a reference to the new order</param>
		/// <returns>a <see cref="System.Boolean"/> telling the user whether the 
		/// creating new order directive was successfully passed to the 
		/// orders FSM</returns>
		public bool Create(LMTOrderParameters lmtParams, OrderEventNotifier eventNotifier, object cookie, out object orderId)
		{
			//initialize a new order instance
			MaofOrder order = new MaofOrder(lmtParams, this.connection);
			order.eventNotifier = eventNotifier;
			order.eventNotifierCookie = cookie;

			//create a mesage, send it to the FSM
			OrderFSMMessage msg = new OrderFSMMessage(OrderFSMEvent.Init, order);
			bool result = this.Send(msg);
			Console.WriteLine("New order created in state " + order.OrderState + " event is: Init");

			//set the return parameters - this one is to be kept as a cookie within Algo
			orderId = order;

			return result;
		}
		
		public double GetOrderPrice(object orderId)
		{
			MaofOrder order = (MaofOrder)orderId;
			return order.Price;
		}

		public int GetOrderSize (object orderId)
		{
			MaofOrder order = (MaofOrder)orderId;
			return order.Quantity;
		}


		public double GetOrderPriceFill (object orderId)
		{
			MaofOrder order = (MaofOrder)orderId;
			return order.PriceFill;
		}

		public TransactionType GetTransactionType (object orderId)
		{
			MaofOrder order = (MaofOrder)orderId;
			return order.TransactionType;
		}


		/// <summary>
		/// User will call this method to cancel a pending order
		/// </summary>
		/// <param name="o">a reference to the order the user wants to cancel</param>
		/// <returns>a <see cref="System.Boolean"/> 
		/// telling the user whether the cancel directive was successfully 
		/// passed to the orders FSM</returns>
		public bool Cancel(ref object orderId)
		{
			//create a message and send it to the FSM
			OrderFSMMessage msg = new OrderFSMMessage(OrderFSMEvent.Cancel, orderId);
			bool result = this.Send(msg);
			return result;
		}

		//note that there are 3 different versions of Update 
		//- for updating the quantity, the price or both

		/// <summary>
		/// User will call this method to update pending order's
		/// quantity
		/// </summary>
		/// <param name="quantity">a <see cref="System.Int32"/></param>
		/// <param name="o">a reference to the order the user wants to update</param>
		/// <returns>telling the user whether the cancel directive was successfully 
		/// passed to the orders FSM</returns>
		public bool Update(int quantity, ref object orderId)
		{
			//update order's parameters
			MaofOrder order = (MaofOrder)orderId;
			order.Quantity = quantity;
			order.mf_order.ammount = quantity.ToString();
			orderId = order;

			//create a message and send it to the FSM
			OrderFSMMessage msg = new OrderFSMMessage(OrderFSMEvent.Update, orderId);
			bool result = this.Send(msg);
			return result;
		}

		public bool Update(double price, ref object orderId)
		{
			//update order's parameters
			MaofOrder order = (MaofOrder)orderId;
			order.Price = price;
			order.mf_order.price = price.ToString();
			orderId = order;

			//create a message and send it to the FSM
			OrderFSMMessage msg = new OrderFSMMessage(OrderFSMEvent.Update, orderId);
			bool result = this.Send(msg);
			return result;
		}

		public bool Update(int quantity, double price, ref object orderId)
		{
			//update order's parameters
			MaofOrder order = (MaofOrder)orderId;
			order.Quantity = quantity;
			order.Price = price;
			order.mf_order.ammount = quantity.ToString();
			order.mf_order.price = price.ToString();
			orderId = order;

			//create a message and send it to the FSM
			OrderFSMMessage msg = new OrderFSMMessage(OrderFSMEvent.Update, orderId);
			bool result = this.Send(msg);
			return result;
		}

		/// <summary>
		/// User directs the FSM to cleanup inactive orders
		/// (fatal / execued / canceled). 
		/// </summary>
		/// <param name="orderId">reference to the order i want to remove from the FSM</param>
		/// <returns></returns>
		public bool Remove(ref object orderId)
		{
			// create message and send it to the FSM
			// FSM will take care of the rest
			OrderFSMMessage msg = new OrderFSMMessage(OrderFSMEvent.Clean, orderId);
			bool result = this.Send(msg);
			return result;
		}

		/// <summary>
		/// Forced cleanup the orders FSM - in case manual intervention 
		/// is necessary.
		/// </summary>
		/// <param name="orderId"></param>
		/// <returns></returns>
		public bool ForcedRemove(ref object orderId)
		{
			OrderFSMMessage msg = new OrderFSMMessage(OrderFSMEvent.ForcedClean, orderId);
			bool result = this.Send(msg);
			return result;
		}

		#endregion;


		/// <summary>
		///This is a flag indicating whether trading session is opened / closed
		///FMR's API allows multiple instances (or maybe references ?) of the TaskBar trading session
		///by calling multiple times to TaskBarLib.UserClass.StartRezefSession
		///trading session will not stop until StopRezefSession is called the same number of times
		///this flag assures that we have only one such instance
		/// </summary>
		public bool isTradingSessionActive
		{
			get;
			protected set;
		}

		/// <summary>
		/// This is a flag indicating whether orders data stream is opened / closed
		/// this one is similar to isTradingSessionActive and assures that we have at most one 
		/// such instance at any time
		/// </summary>
		public bool isOrderStreamActive
		{
			get;
			protected set;
		}

		/// <summary>
		/// starts TaskBar trading session. In general, it's possible to send orders
		/// without strating one, but response times are huge, because TaskBar opens 
		/// temporary session each time an order is submitted.
		/// </summary>
		public void StartTrading()
		{
			//if no trading session is started, attempt to start one now
			if (!this.isTradingSessionActive)
			{
				int rc = this.connection.userClass.StartRezefSession(
					this.connection.GetSessionId(),
					this.connection.Parameters.Account,
					this.connection.Parameters.Branch
					);

				// check the return code, report to the operator
				// TODO: i need a cleanup here, console notifications are for debugging purposes only
				if (rc == 0) Console.WriteLine("MF trading session successflly started, rc = " + rc);
				else Console.WriteLine("MF trading session failed to start, rc = " + rc);
			}
		}

		/// <summary>
		/// stops trading session
		/// </summary>
		public void StopTrading()
		{
			if (this.isTradingSessionActive)
			{
				int rc = this.connection.userClass.StopRezefSession(
					this.connection.GetSessionId(),
					this.connection.Parameters.Account,
					this.connection.Parameters.Branch
					);

				// check the return code, report to the operator
				// TODO: i need a cleanup here, console notifications are for debugging purposes only
				if (rc == 0) Console.WriteLine("MF trading session successflly stopped, rc = " + rc.ToString());
				else Console.WriteLine("MF trading session failed to stop, rc = " + rc.ToString());
			}
		}


		/// <summary>
		/// start TaskBar orders data stream, without one I'm unable to poll the server for orders events
		/// </summary>
		public void StartOrderDataStream()
		{
			if (!this.isOrderStreamActive)
			{
				int rc = this.connection.userClass.OrdersStreamStart(
					this.connection.GetSessionId(),
					this.connection.Parameters.Account,
					this.connection.Parameters.Branch,
					TradeType.MF //hardcoded MF, since in MaofOrdersFSM I don't expect any other stream, but MF
					);

				// check the return code, report to the operator
				// TODO: i need a cleanup here, console notifications are for debugging purposes only
				if (rc == 0) Console.WriteLine("MF order stream successflly started, rc = " + rc);
				else Console.WriteLine("MF order stream failed to start, rc = " + rc);
			}
		}

		/// <summary>
		/// stops TaskBar orders data stream, call it at the end of the trading session
		/// </summary>
		public void StopOrderDataStream()
		{
			if (this.isOrderStreamActive)
			{
				int rc = this.connection.userClass.OrdersStreamStop(
					this.connection.GetSessionId(),
					this.connection.Parameters.Account,
					this.connection.Parameters.Branch,
					TradeType.MF    //hardcoded MF, since in MaofOrdersFSM I don't expect any other stream, but MF
					);

				// check the return code, report to the operator
				// TODO: i need a cleanup here, console notifications are for debugging purposes only
				if (rc == 0) Console.WriteLine("MF trading session successflly stopped, rc = " + rc.ToString());
				else Console.WriteLine("MF trading session failed to stop, rc = " + rc.ToString());
			}
		}

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="connection">a <see cref="FMRShell.Connection"/> object. 
		/// Contains all the API's needed to operate orders through FMR</param>
		public MaofOrderFSM(Connection connection)
			: base("MFOrdersFSM", 100)
		{
			//name the log file
			string filename = Resources.CreateLogFileName("LogMaofOrderFSM_", LogType.CSV);
			// create logger
			fileLoggerMaof = new FileLogger<MaofOrder>("MFOrderFSM_Logger", filename, true, FMRShell.MaofOrderFSM.MaofOrder.LogLegend());
			//and start it
			fileLoggerMaof.Start();

			//reference to an instance of FMRShell.Connection
			this.connection = connection;

			// create timer task 
			timerTask = new TimerTask("MaofOrder");

			// create two types of timers - 100ms timer and 300ms timer
			timers_fast = new TimerList("100ms", 100, 1, this.TimerExpiredHandler_1, timerTask);    // i need only one fast timer 

			//Each pending order can get a timeout during its processing, the timers here will cause the FSM to wait for possible updates
			//(events) and attempt to continue the processing from the point we stopped if no updates were received upon timer's expiration
			timers_TimeOut = new TimerList("300ms", 300, 100, this.TimerUpdateCancelHandler, timerTask); // max number of coexsiting positions is 100

			// one-second periodic timer. i start the timer in the constructor. when event arrives I print out that the event arrived and 
			// restart the timer. In the future I can add entry to the log. This can be useful to ensure that FSM was responsive to the 
			// external events
			timers_Watchdog = new TimerList("1s", 1000, 1, this.TimerUpdateWatchdog, timerTask); // i need only one watchdog timer
			// i start the timer right here and now
			timers_Watchdog.Start();

			// start the timer task, no timers yet
			timerTask.Start();

			// Up to 100 pending jobs and 20 threads to serve the incoming job requests
			// this is overhead of course - real TaskBar allows only two parallell threads to send orders
			// so if we have more than two orders to send in any given moment, they will wait in line
			// no matter how many threads we have in our app - TaskBar will be blocked until 
			// either one of the two returns (I need to verify it vs. helpdesk). 
			// The other blocking operation is polling, but it is done one at a time.
			threadPool = new JQuant.ThreadPool("RezefOrderFSM", 20, 100, ThreadPriority.Normal);

			//initialize the hash table where broker-approved orders are stored
			OrdersHash = new System.Collections.Hashtable(20); //initial capacity of 20

			// Now I need to initialize two TaskBar streams in order to be able to trade and track my trades
			// they both will be active as long as OrderFSM instance exists.

			// this one is orders stream, which I need to start in order to be able to call 
			// userClass.GetOrdersRZ() method
			this.OrdersLastTime = "00000000"; //initialize time of last TaskBar orders update - set it to zero
			//to get all the updates at the first call, then its value will be set automatically via PollOrderStream
			//and only subsequent updates will be retrieved from the server. Note that for each order only the 
			//latest available update is returned by GetOrdersRZ. To get complete history, call GetOrdersRezef instead

			//now start the stream:
			this.StartOrderDataStream();

			// the second one is TaskBar trading session stream - this one i need in order to be able 
			// to submit trading directions to the remote server, that is, call to UserClass.SendOrderRZ() method
			// note that calling SendOrderRZ is still possible without active trading session in place, in this case
			// a temporary session will be opened, but response times will be a disater - the thread will freeze for ~5 secs
			this.StartTrading();

			bool timerStarted = timers_fast.Start(true);
			if (timerStarted) Console.WriteLine("Timer1 started");
			else Console.WriteLine("Failed to start Timer1");

			// i start order stream polling thread right away
			StartPollOrderStream();
		}

		/// <summary>
		/// Call this method to release associated resources 
		/// </summary>
		public override void Dispose()
		{
			//timers
			timers_fast.Dispose();
			timers_TimeOut.Dispose();
			timerTask.Stop();
			timerTask.Dispose();
			threadPool.Dispose();

			//stop and dispose the logger - make sure that the data is written out 
			fileLoggerMaof.Stop();
			fileLoggerMaof.Dispose();

			//stop trading session
			this.StopTrading();

			//stop orders data stream
			this.StopOrderDataStream();

			//eventually stop and dispose MailboxThread:
			this.Stop();
			base.Dispose();
		}

		/// <summary>
		/// this is Job delegate to poll order stream 
		/// </summary>
		private void PollOrderStream(ref object argument)
		{
			//keeps all the records (if any) retrieved during this call
			System.Array vecRecords = null;
			bool doPoll;
			int rc = 0;

			lock (OrdersHash)
			{
				doPoll = (OrdersHash.Count != 0);
			}

			// send request to the server
			if (doPoll)
			{
				rc = this.connection.userClass.GetOrdersRZ(
					this.connection.GetSessionId(),
					out vecRecords,
					this.connection.Parameters.Account,
					this.connection.Parameters.Branch,
					ref this.OrdersLastTime
				 );
			}

			// process the response
			// if there are any changes, for every one of them send a message to the FSM.
			// rc>=0 denotes the number of new records arrived, rc<0 means polling failure
			if (rc > 0)
			{
				OrderFSMEvent e;
				MOFINQType data;

				//for each new record, check what's the event
				for (int i = 0; i < rc; i++)
				{
					data = (MOFINQType)vecRecords.GetValue(i);
					switch (data.STS)
					{
						case ("1"):     //"נקלט ע.ת." - new order approved by FMR, but not by TASE
						case ("2"):     //"עדכון ע.ת." - update approved by FMR, but not by TASE
						case ("3"):     //"בטול ע.ת." - cancel approved by FMR, but not by TASE
							e = OrderFSMEvent.ApproveBroker;
							break;
						case ("4"):     //"נקלט" - approved TASE
							e = OrderFSMEvent.ApproveTrading;
							break;
						case ("5"):     //"בצוע חלקי" - partial fill
							e = OrderFSMEvent.PartialFill;
							break;
						case ("6"):     //"בצוע מלא" - complete fill
							e = OrderFSMEvent.CompleteFill;
							break;
						case ("7"):     //"בוטל" - cancel
							e = OrderFSMEvent.ApproveCancel;
							break;
						case ("B"):     //"לא עודכן" - didn't update
							e = OrderFSMEvent.UpdateTimeOut;
							break;
						case ("C"):     //"לא בוטל" - didn't cancel
							e = OrderFSMEvent.CancelTimeOut;
							break;
						case ("8"): //= "מושעה" (suspended?)    //any value higher than 7 denotes a problem with the order
						case ("A"): //= "שגוי"  ("error")       //any error at TASE stage means fatal, will require at most another attempt
						case ("R"): //= "שוגר פתח"              //R/S/T are uncommon values (not sure they can appear at all)
						case ("S"): //= "שוגר עדכון"            //in these cases the only thing I can do is just log
						case ("T"): //= "שוגר בטל"              //maybe try to re-submit the order
							e = OrderFSMEvent.FatalTASE;
							break;
						default:            //in general I can't get here - but need to test it empirically
							e = OrderFSMEvent.FatalTASE;
							Console.WriteLine("Unknown STS value =" + data.STS + " while parsing order stream poll.");
							break;
					}

					//send message to the FSM
					OrderFSMMessage msg = new OrderFSMMessage(e, vecRecords.GetValue(i));
					this.Send(msg);
				}
			}

			else if (rc < 0)
			{
				Console.WriteLine("OrderStream polling failed with rc = " + rc);
			}

			// i want to avoid tight loops even if previous calls return immediately.
			// i put a short sleep here
			Thread.Sleep(50);

			// restart the job again - poll all the time
			StartPollOrderStream();
		}

		private void StartPollOrderStream()
		{
			bool threadStarted;
			// only one order status polling thread. i do not need JobDone and I do not need any cookes
			threadStarted = threadPool.PlaceJob(PollOrderStream);
			if (!threadStarted)
			{
				Console.WriteLine("Failed to start Order status polling thread");
			}
		}

		/// <summary>
		/// Put here non blocking operations only. For example send message 
		/// </summary>
		private void TimerExpiredHandler_1(ITimer timer)
		{
		}

		/// <summary>
		/// Send watchdog event to the FSM
		/// FSM will print time stamp and restart the timer
		/// </summary>
		private void TimerUpdateWatchdog(ITimer timer)
		{
			OrderFSMMessage msg = new OrderFSMMessage(OrderFSMEvent.Watchdog, null);
			//this.Send(msg);
		}

		/// <summary>
		/// TimeOut Timer Handler. In case the timer didn't stop prior to it's expiration, 
		/// this method is going to perform a certain task - like retry Cancel or Update
		/// </summary>
		private void TimerUpdateCancelHandler(ITimer timer)
		{
			//retrieve the reference to the MaofOrder object that caused the timer to start
			MaofOrder rezefOrder = (MaofOrder)timer.ApplicationHookGet();
			OrderFSMMessage msg;

			msg = new OrderFSMMessage(OrderFSMEvent.TimerUpdateCancel, rezefOrder);
			this.Send(msg);
		}

		/// <summary>
		/// Call Dsipose() to get rid of the object 
		/// </summary>
		~MaofOrderFSM()
		{
		}

		/// <summary>
		/// check for possible errors at broker server level,
		/// take an action where possible. note that there can be a 
		/// lot of ping-pong between me and the server 
		/// (errors are produced once a time, requiring roundtrip for each)
		/// if action wasn't taken within certain time period (~20-30secs)
		/// this order will be wiped out from the server and no possible to send it again
		/// TODO: for some of the errors we will want to inform the human operator through the CLI
		/// </summary>
		/// <param name="rezefOrder"></param>
		/// <param name="rc"></param>
		/// <returns></returns>
		protected bool HandleFMRInternalError(MaofOrder maofOrder, out int rc)
		{
			rc = 0;
			OrderFSMMessage msg;

			// TODO: I may want to add here an indication by the Algo in case the order is no longer relevant
			// if received in the middle of this loop, it just will get out without attempting further pocessing

			while (maofOrder.ErrNO != 0)
			{
				//I want to log any round through the loop in order to trace the process:
				fileLoggerMaof.AddEntry(maofOrder);

				switch (maofOrder.ErrorType)
				{
					case OrdersErrorTypes.NoError:
						//I'm not sure I can get here at all, as ErrNO=0 by default
						//i keep this one just in case - TaskBar is a strange animal ;)

						//no need to take an action - everything is allright
						//set ErrNo = 0 to get out from the loop
						maofOrder.ErrNO = 0;
						break;
					case OrdersErrorTypes.Alert:
						//optionally display the alert on the screen:
						Console.WriteLine("Alert interanl error");
						//set ErrNo = 0 to get out from the loop
						maofOrder.ErrNO = 0;
						break;
					case OrdersErrorTypes.Confirmation:
						//user confirmation is needed - call SendOrderRZ again
						Console.WriteLine("Confirmation interanl error" + maofOrder.VBMsg);
						Console.Write("Continue? [y/n]: ");
						string operatorResponse = Console.ReadLine();
						//possibly wait for operator / algo response. Set rezefOrder.ReEnteredValue to either 'YES' or 'NO',
						//depending on the opeartor's / algo's response
						//or I can just set here hardcoded "YES" / "NO"

						//parse user's input
						switch (operatorResponse.ToLower())
						{
							case "y":
								maofOrder.ReEnteredValue = "YES";
								rc = this.connection.userClass.SendMaofOrder(
									connection.GetSessionId(),
									ref maofOrder.mf_order,
									ref maofOrder.VBMsg,
									ref maofOrder.ErrNO,
									out maofOrder.ErrorType,
									ref maofOrder.OrderID,
									maofOrder.AuthUser,
									maofOrder.AuthPassword,
									maofOrder.ReEnteredValue,
									maofOrder.SPCOrder
									);
								break;
							default:
								maofOrder.ReEnteredValue = "NO";
								Console.WriteLine("Order aborted");
								//get out without attempting to re-send the order
								maofOrder.ErrNO = 0;
								rc = -1; //indicates unsuccesful completion

								// the situation is the same as with fatal
								//just give up this order, either algo or the human operator will decide what to do next
								msg = new OrderFSMMessage(OrderFSMEvent.FatalBroker, maofOrder);
								this.Send(msg);
								break;
						}
						break;
					case OrdersErrorTypes.ReEnter:
						Console.WriteLine("ReEnter interanl error:" + maofOrder.VBMsg + "Please enter the corrected value or 'ABORT': ");
						string newData = Console.ReadLine();    //of course it will be initialized by either algo or the human operator
						//depending on the situation, the user / algo will decide whether continue or not
						if (newData.ToLower() == "abort")
						{
							//just get out, send a message to the FSM
							maofOrder.ErrNO = 0;
							rc = -1;
							msg = new OrderFSMMessage(OrderFSMEvent.FatalBroker, maofOrder);
							this.Send(msg);
						}

						else
						{
							maofOrder.ReEnteredValue = newData;

							rc = this.connection.userClass.SendMaofOrder(
								connection.GetSessionId(),
								ref maofOrder.mf_order,
								ref maofOrder.VBMsg,
								ref maofOrder.ErrNO,
								out maofOrder.ErrorType,
								ref maofOrder.OrderID,
								maofOrder.AuthUser,
								maofOrder.AuthPassword,
								maofOrder.ReEnteredValue,
								maofOrder.SPCOrder
								);
						}
						break;
					case OrdersErrorTypes.PasswordReq:
						Console.WriteLine("PasswordReq interanl error:" + maofOrder.VBMsg);
						//TODO: depending on the situation, the user/algo will be given 
						//an opportunity to decide whether continue or not, at the moment it's hardcoded continue:
						rc = this.connection.userClass.SendMaofOrder(
							connection.GetSessionId(),
							ref maofOrder.mf_order,
							ref maofOrder.VBMsg,
							ref maofOrder.ErrNO,
							out maofOrder.ErrorType,
							ref maofOrder.OrderID,
							connection.Parameters.userName,     //rezefOrder.AuthUser is an empty string,""
							connection.Parameters.userPassword, //rezefOrder.AuthPassword is an empty string
							maofOrder.ReEnteredValue,
							maofOrder.SPCOrder
							);
						break;
					case OrdersErrorTypes.Fatal:
						Console.WriteLine("Fatal interanl error:" + maofOrder.VBMsg);

						//set ErrNo = 0 to trigger exit from the loop
						maofOrder.ErrNO = 0;
						rc = -1;

						// send message to the FSM and let the FSM handle the rest
						msg = new OrderFSMMessage(OrderFSMEvent.FatalBroker, maofOrder);
						this.Send(msg);
						break;
					default:
						System.Console.WriteLine("Unhandled internal error " + maofOrder.ErrorType);
						//leave the loop
						maofOrder.ErrNO = 0;
						rc = -1;
						break;
				}
			}//while

			//set OrderID=0, other value will cause an error when calling SendOrderRZ again
			maofOrder.OrderID = 0;
			return (rc == 0);
		}


		/// <summary>
		/// This method is called from a job thread. 
		/// Any order operation will use it - send a brand new order / update / cancel.
		/// Update and Cancel require changing the order's internal parameters, so we need 
		/// here two additional methods - CancelOrder and UpdateOrder.
		/// Sending new order takes an initialized order produced by Create, so no additional
		/// operations are needed in that case.
		/// </summary>
		protected void SendOrder(ref object o)
		{
			MaofOrder order = (MaofOrder)o;
			OrderFSMMessage msg;

			int rc = this.connection.userClass.SendMaofOrder(
				connection.GetSessionId(),
				ref order.mf_order,
				ref order.VBMsg,
				ref order.ErrNO,
				out order.ErrorType,
				ref order.OrderID,
				order.AuthUser,
				order.AuthPassword,
				order.ReEnteredValue,
				order.SPCOrder
				);

			if (rc != 0)    //most probably an internal error, just warn the operator
			{
				Console.WriteLine("SendOrderRZ returned with error: " + rc);
			}

			//check for internal errors and correct if possible or report fatal
			bool noErr = HandleFMRInternalError(order, out rc);

			//in case of success - continue tracking the order
			if (rc == 0 && noErr)
			{
				// send message to the FSM and let the FSM handle the rest
				msg = new OrderFSMMessage(OrderFSMEvent.ApproveBroker, order);
			}
			//otherwise report of fatal error
			else
			{
				msg = new OrderFSMMessage(OrderFSMEvent.FatalBroker, order);
			}
			//send the message anyway
			this.Send(msg);
		}

		/// <summary>
		/// All order processing is done through SendOrder method
		/// which is called from a job thread. Here we do only parameters 
		/// initialization, which is non blocking, and then pass the 
		/// processing to the job thread.
		/// </summary>
		protected void CancelOrder(MaofOrder order)
		{
			//initialize internal FMR order - set Cancel parameter
			order.mf_order.operation = "D";	//TODO - check whether it's "D" or "C"

			// schedule the job for execution in a job thread, which will send request to the FMR
			threadPool.PlaceJob(SendOrder, null, order);
		}

		protected void UpdateOrder(MaofOrder order)
		{
			//initialize internal FMR order - set correct Update parameter
			order.mf_order.operation = "U";

			// schedule the job for execution in a job thread, which will send request to the FMR
			threadPool.PlaceJob(SendOrder, null, order);
		}

		/// <summary>
		/// Handle all incoming events here 
		/// </summary>
		/// <param name="message">
		/// A <see cref="OrderFSMMessage"/>
		/// </param>
		protected override void HandleMessage(OrderFSMMessage message)
		{
			switch (message.msgEvent)
			{
				case OrderFSMEvent.Init:
					HandleMessage_Init(message);
					Console.WriteLine("Init event handled by HandleMessage");
					break;
				case OrderFSMEvent.FatalBroker:
					HandleMessage_FatalBroker(message);
					Console.WriteLine("FatalBroker event handled by HandleMessage");
					break;
				case OrderFSMEvent.ApproveBroker:
					HandleMessage_ApproveBroker(message);
					Console.WriteLine("ApproveBroker event handled by HandleMessage");
					break;
				case OrderFSMEvent.FatalTASE:
					HandleMessage_FatalTASE(message);
					Console.WriteLine("FatalTASE event handled by HandleMessage");
					break;
				case OrderFSMEvent.ApproveTrading:
					HandleMessage_ApproveTrading(message);
					Console.WriteLine("ApproveTrading event handled by HandleMessage");
					break;
				case OrderFSMEvent.Update:
					HandleMessage_Update(message);
					Console.WriteLine("Update event handled by HandleMessage");
					break;
				case OrderFSMEvent.Cancel:
					HandleMessage_Cancel(message);
					Console.WriteLine("Cancel event handled by HandleMessage");
					break;
				case OrderFSMEvent.UpdateTimeOut:
					HandleMessage_UpdateTimeOut(message);
					Console.WriteLine("UpdateTimeOut event handled by HandleMessage");
					break;
				case OrderFSMEvent.CancelTimeOut:
					HandleMessage_CancelTimeOut(message);
					Console.WriteLine("CancelTimeOut event handled by HandleMessage");
					break;
				case OrderFSMEvent.ApproveCancel:
					HandleMessage_ApproveCancel(message);
					Console.WriteLine("ApproveCancel event handled by HandleMessage");
					break;
				case OrderFSMEvent.PartialFill:
					HandleMessage_PartialFill(message);
					Console.WriteLine("PartialFill event handled by HandleMessage");
					break;
				case OrderFSMEvent.CompleteFill:
					HandleMessage_CompleteFill(message);
					Console.WriteLine("CompleteFill event handled by HandleMessage");
					break;
				case OrderFSMEvent.TimerUpdateCancel:
					HandleMessage_TimerUpdateCancel(message);
					Console.WriteLine("TimerUpdateCancel event handled by HandleMessage");
					break;
				case OrderFSMEvent.Clean:
					HandleMessage_Clean(message);
					break;
				case OrderFSMEvent.ForcedClean:
					HandleMessage_ForcedClean(message);
					Console.WriteLine("ForcedClean event handled by HandleMessage");
					break;
				case OrderFSMEvent.Watchdog:
					Console.WriteLine(JQuant.DateTimePrecise.UtcNow.ToString() + ": watchdog");
					timers_Watchdog.Start();
					break;
				default:
					System.Console.WriteLine("Unhandled orders FSM event " + message.msgEvent);
					break;
			}
		}

		#region Messages handlers;

		//there are two types of events, each one passes different type of data in the message:
		//algo or internal FSM events pass a reference to the MaofOrder instance - in this case I just make a cast.
		//events from FMR's order stream come in form of TaskBarLib.MOFINQType struct. 
		//To get the reference to the right instance of MaofOrder I need to do two things:
		// - retrieve the data from the message (cast), parse the AsmachtaFMR (unique id)
		// - using AsmachtaFMR as key, get the reference to the relevant order from the hashtable

		protected void HandleMessage_Init(OrderFSMMessage message)
		{
			// i expect in the message data a new object of type MaofOrder, created by Create()
			MaofOrder order = (MaofOrder)message.msgData;
			order.lastEvent = OrderFSMEvent.Init;
			//log
			fileLoggerMaof.AddEntry(order);

			switch (order.OrderState)
			{
				case OrderFSMState.IDLE:
					// submit the new order for trading - schedule the job for execution in a job thread
					threadPool.PlaceJob(SendOrder, null, order);

					//the system knows now that the order is processed somewhere
					order.OrderState = OrderFSMState.SENT;

					//and notify the algo about this
					OrderEventNotifierMessage orderMsg = new OrderEventNotifierMessage(OrderProcessorEvent.OrderSent);
					order.eventNotifier(ref order.eventNotifierCookie, orderMsg);
					break;

				default: // print error - init comes only in idle state
					HandleMessage_Default(message, order.OrderState);
					break;
			}
		}

		private void HandleMessage_FatalBroker(OrderFSMMessage message)
		{
			MaofOrder order = (MaofOrder)message.msgData;
			order.lastEvent = OrderFSMEvent.FatalBroker;
			//log
			fileLoggerMaof.AddEntry(order);

			switch (order.OrderState)
			{
				case OrderFSMState.SENT:
				case OrderFSMState.CANCELING:
				case OrderFSMState.UPDATING:
					order.OrderState = OrderFSMState.FATAL;
					//notify the algo
					OrderEventNotifierMessage OrderMsg = new OrderEventNotifierMessage(OrderProcessorEvent.Error);
					order.eventNotifier(ref order.eventNotifierCookie, OrderMsg);
					break;
				default:
					HandleMessage_Default(message, order.OrderState);
					break;
			}
		}

		private void HandleMessage_ApproveBroker(OrderFSMMessage message)
		{
			MaofOrder order = null;

			//check the data type delivered by message. based on it, obtain 
			//the correct order reference. ApproveBroker event can be triggered
			//either internally (call to SendOrderRZ returned a valid 
			//AsmachtaFMR with no errors. Or it can be polling order stream.
			//depending on the origin of the event (internal or order stream poll)
			//data of different type can arrive in the message - it's a reference to an 
			//order in the former case and MOFINQType in the latter

			if (message.msgData.GetType() == typeof(MOFINQType))
			{
				MOFINQType record = (MOFINQType)message.msgData;
				CheckHashKey(record, ref order);
				order.AsmachtaRezef = JQuant.Convert.StrToInt(record.RZF_ORD_PIC);
			}
			else
				order = (MaofOrder)message.msgData;

			if (order != null)
			{
				order.lastEvent = OrderFSMEvent.ApproveBroker;

				fileLoggerMaof.AddEntry(order);

				switch (order.OrderState)
				{
					case OrderFSMState.SENT:
						//add the approved order to the hash table:
						lock (OrdersHash)
						{
							OrdersHash.Add(order.AsmachtaFMR, order);
						}
						order.OrderState = OrderFSMState.WaitingTASE;
						break;
					case OrderFSMState.WaitingTASE:
					case OrderFSMState.CANCELING:
					case OrderFSMState.UPDATING:
						break;
					default:
						HandleMessage_Default(message, order.OrderState);
						break;
				}
			}
		}

		/// <summary>
		/// handles message from the order stream, need to find the right order in the hash table
		/// </summary>
		/// <param name="message"></param>
		private void HandleMessage_FatalTASE(OrderFSMMessage message)
		{
			//check that this order exists at all in our system
			//retrieve the data from the message 
			//get the reference to the relevant order from the hashtable
			//I expect MOFINQType as this event was triggered by order stream poll
			MaofOrder order = null;
			MOFINQType record = (MOFINQType)message.msgData;
			if (CheckHashKey(record, ref order))
			{
				//update order data
				order.lastEvent = OrderFSMEvent.FatalTASE;
				order.lastPoll = record;
				order.AsmachtaRezef = JQuant.Convert.StrToInt(record.RZF_ORD_PIC);

				//write log entry
				fileLoggerMaof.AddEntry(order);

				switch (order.OrderState)
				{
					case OrderFSMState.WaitingTASE:
						order.OrderState = OrderFSMState.FATAL;
						//notify the algo - the algo will take care of cleanup - typically it will call Remove
						OrderEventNotifierMessage OrderMsg = new OrderEventNotifierMessage(OrderProcessorEvent.Error);
						order.eventNotifier(ref order.eventNotifierCookie, OrderMsg);
						//log
						fileLoggerMaof.AddEntry(order);
						break;
					case OrderFSMState.CANCELING:
						//probably a fill - let's retry cancel if not. I don't change the state, though
						OrderFSMMessage cancelTimeOutMsg = new OrderFSMMessage(OrderFSMEvent.CancelTimeOut, order);
						this.Send(cancelTimeOutMsg);
						break;
					case OrderFSMState.UPDATING:
						OrderFSMMessage updateTimeOutMsg = new OrderFSMMessage(OrderFSMEvent.UpdateTimeOut, order);
						this.Send(updateTimeOutMsg);
						break;
					default:
						HandleMessage_Default(message, order.OrderState);
						break;
				}
			}

		}

		private void HandleMessage_ApproveTrading(OrderFSMMessage message)
		{
			MaofOrder order = null;
			MOFINQType record = (MOFINQType)message.msgData;
			if (CheckHashKey(record, ref order))
			{

				order.lastEvent = OrderFSMEvent.ApproveTrading;
				order.lastPoll = record;
				order.AsmachtaRezef = JQuant.Convert.StrToInt(record.RZF_ORD_PIC);
				fileLoggerMaof.AddEntry(order);

				OrderEventNotifierMessage OrderMsg;

				switch (order.OrderState)
				{
					//TODO: here I want validate the data received via polling vs. internal
					case OrderFSMState.WaitingTASE:
						order.OrderState = OrderFSMState.WaitingFill;
						OrderMsg = new OrderEventNotifierMessage(OrderProcessorEvent.TradingApproved);
						order.eventNotifier(ref order.eventNotifierCookie, OrderMsg);
						break;
					case OrderFSMState.UPDATING:
						order.OrderState = OrderFSMState.WaitingFill;
						//inform the algo
						OrderMsg = new OrderEventNotifierMessage(OrderProcessorEvent.Updated);
						order.eventNotifier(ref order.eventNotifierCookie, OrderMsg);
						break;
					default:
						HandleMessage_Default(message, order.OrderState);
						break;
				}
			}
		}

		//TODO - algo will count how many recurring updates/cancels it received in the same state
		//and at some point it will ask the operator for manual intervention, causing 'ForcedClean'

		private void HandleMessage_Update(OrderFSMMessage message)
		{
			MaofOrder order = (MaofOrder)message.msgData;
			order.lastEvent = OrderFSMEvent.Update;

			fileLoggerMaof.AddEntry(order);

			OrderEventNotifierMessage OrderMsg;

			switch (order.OrderState)
			{
				case OrderFSMState.WaitingFill:
					//send Update directive to the FMR server
					order.OrderState = OrderFSMState.UPDATING;
					this.UpdateOrder(order);
					OrderMsg = new OrderEventNotifierMessage(OrderProcessorEvent.UpdateSent);
					order.eventNotifier(ref order.eventNotifierCookie, OrderMsg);
					break;
				case OrderFSMState.UPDATING:
					//update in updating state - a second try after time out
					this.UpdateOrder(order);
					OrderMsg = new OrderEventNotifierMessage(OrderProcessorEvent.UpdateSent);
					order.eventNotifier(ref order.eventNotifierCookie, OrderMsg);
					break;
				default:
					HandleMessage_Default(message, order.OrderState);
					break;
			}
		}

		private void HandleMessage_Cancel(OrderFSMMessage message)
		{
			MaofOrder order = (MaofOrder)message.msgData;
			order.lastEvent = OrderFSMEvent.Cancel;

			fileLoggerMaof.AddEntry(order);

			OrderEventNotifierMessage OrderMsg;

			switch (order.OrderState)
			{
				case OrderFSMState.WaitingFill:
					order.OrderState = OrderFSMState.CANCELING;
					CancelOrder(order);
					OrderMsg = new OrderEventNotifierMessage(OrderProcessorEvent.CancelSent);
					order.eventNotifier(ref order.eventNotifierCookie, OrderMsg);
					break;
				case OrderFSMState.CANCELING:
					// Cancel in canceling state - a second try after timeout
					CancelOrder(order);
					OrderMsg = new OrderEventNotifierMessage(OrderProcessorEvent.CancelSent);
					order.eventNotifier(ref order.eventNotifierCookie, OrderMsg);
					break;
				default:
					HandleMessage_Default(message, order.OrderState);
					break;
			}
		}

		private void HandleMessage_UpdateTimeOut(OrderFSMMessage message)
		{
			MaofOrder order = null;
			MOFINQType record = (MOFINQType)message.msgData;
			if (CheckHashKey(record, ref order))
			{

				order.lastEvent = OrderFSMEvent.UpdateTimeOut;
				order.lastPoll = record;
				order.AsmachtaRezef = JQuant.Convert.StrToInt(record.RZF_ORD_PIC);
				fileLoggerMaof.AddEntry(order);

				switch (order.OrderState)
				{
					case OrderFSMState.UPDATING:
						//start the timer, pass order's reference to its ITimer member
						ITimer timer;
						long timerId;
						timers_TimeOut.Start(out timer, out timerId, order, false);
						//the timer will return when expired via TimerExpiredHandler
						break;
					default:
						HandleMessage_Default(message, order.OrderState);
						break;
				}
			}
		}

		private void HandleMessage_CancelTimeOut(OrderFSMMessage message)
		{
			MaofOrder order = null;
			MOFINQType record = (MOFINQType)message.msgData;
			if (CheckHashKey(record, ref order))
			{

				order.lastEvent = OrderFSMEvent.CancelTimeOut;
				order.lastPoll = record;
				order.AsmachtaRezef = JQuant.Convert.StrToInt(record.RZF_ORD_PIC);
				fileLoggerMaof.AddEntry(order);

				switch (order.OrderState)
				{
					case OrderFSMState.CANCELING:
						//start the timer, pass order's reference to its Itemer member
						ITimer timer;
						long timerId;
						timers_TimeOut.Start(out timer, out timerId, order, false);
						break;
					default:
						HandleMessage_Default(message, order.OrderState);
						break;
				}
			}
		}

		private void HandleMessage_ApproveCancel(OrderFSMMessage message)
		{
			MaofOrder order = null;
			MOFINQType record = (MOFINQType)message.msgData;
			if (CheckHashKey(record, ref order))
			{
				order.lastEvent = OrderFSMEvent.ApproveCancel;
				order.lastPoll = record;
				order.AsmachtaRezef = JQuant.Convert.StrToInt(record.RZF_ORD_PIC);
				fileLoggerMaof.AddEntry(order);

				switch (order.OrderState)
				{
					case OrderFSMState.CANCELING:
						order.OrderState = OrderFSMState.CANCELED;
						//inform the algo, which will take care of cleanup, calling Remove
						OrderEventNotifierMessage OrderMsg = new OrderEventNotifierMessage(OrderProcessorEvent.Canceled);
						order.eventNotifier(ref order.eventNotifierCookie, OrderMsg);
						break;
					default:
						HandleMessage_Default(message, order.OrderState);
						break;
				}
			}
		}

		private void HandleMessage_PartialFill(OrderFSMMessage message)
		{
			MaofOrder order = null;
			MOFINQType record = (MOFINQType)message.msgData;
			if (CheckHashKey(record, ref order))
			{
				order.lastEvent = OrderFSMEvent.PartialFill;
				order.lastPoll = record;
				order.AsmachtaRezef = JQuant.Convert.StrToInt(record.RZF_ORD_PIC);
				fileLoggerMaof.AddEntry(order);

				switch (order.OrderState)
				{
					case OrderFSMState.WaitingFill:
					case OrderFSMState.UPDATING:
					case OrderFSMState.CANCELING:
						int FilledQuantity = JQuant.Convert.StrToInt(record.DIL_NV_PIC);
						//partial fill: subtract the last transaction quantity from original order's size
						order.Quantity = order.Quantity - FilledQuantity;
						OrderEventNotifierMessage OrderMsg = new OrderEventNotifierMessage(OrderProcessorEvent.PartialFill, FilledQuantity);
						order.eventNotifier(ref order.eventNotifierCookie, OrderMsg);
						break;
					default:
						HandleMessage_Default(message, order.OrderState);
						break;
				}
			}
		}

		private void HandleMessage_CompleteFill(OrderFSMMessage message)
		{
			MaofOrder order = null;
			MOFINQType record = (MOFINQType)message.msgData;
			if (CheckHashKey(record, ref order))
			{
				order.lastEvent = OrderFSMEvent.CompleteFill;
				order.lastPoll = record;
				order.AsmachtaRezef = JQuant.Convert.StrToInt(record.RZF_ORD_PIC);
				order.PriceFill = JQuant.Convert.StrToDouble(record.DIL_PRC_PIC);
				order.QuantityFill = JQuant.Convert.StrToInt(record.DIL_NV_PIC);
				fileLoggerMaof.AddEntry(order);

				switch (order.OrderState)
				{
					case OrderFSMState.WaitingFill:
					case OrderFSMState.UPDATING:
					case OrderFSMState.CANCELING:
						order.OrderState = OrderFSMState.EXECUTED;
						OrderEventNotifierMessage OrderMsg = new OrderEventNotifierMessage(OrderProcessorEvent.CompleteFill);
						order.eventNotifier(ref order.eventNotifierCookie, OrderMsg);
						break;
					default:
						HandleMessage_Default(message, order.OrderState);
						break;
				}
			}
		}

		protected void HandleMessage_TimerUpdateCancel(OrderFSMMessage message)
		{
			// i expect in the message data object of type MaofOrder
			MaofOrder order = (MaofOrder)message.msgData;
			order.lastEvent = OrderFSMEvent.TimerUpdateCancel;

			fileLoggerMaof.AddEntry(order);

			OrderEventNotifierMessage OrderMsg;
			switch (order.OrderState)
			{
				case OrderFSMState.UPDATING:
					// timeout in the updating state - inform the algo and it will decide
					// either to wait for fill or try to update again (call Update)
					OrderMsg = new OrderEventNotifierMessage(OrderProcessorEvent.UpdateSent);
					order.eventNotifier(ref order.eventNotifierCookie, OrderMsg);
					break;

				case OrderFSMState.CANCELING:
					// timeout in the canceling state - inform the algo and it will decide
					// either to wait for fill or try to cancel again (call Cancel)
					OrderMsg = new OrderEventNotifierMessage(OrderProcessorEvent.CancelSent);
					order.eventNotifier(ref order.eventNotifierCookie, OrderMsg);
					break;

				case OrderFSMState.FATAL:
					//inform the algo and it will take care of cleanup (call Remove)
					OrderMsg = new OrderEventNotifierMessage(OrderProcessorEvent.Error);
					order.eventNotifier(ref order.eventNotifierCookie, OrderMsg);
					break;
				default: // print error - init comes only in idle state
					HandleMessage_Default(message, order.OrderState);
					break;
			}
		}

		// Cleanup. An exit point from the FSM.
		private void HandleMessage_Clean(OrderFSMMessage message)
		{
			MaofOrder order = (MaofOrder)message.msgData;
			order.lastEvent = OrderFSMEvent.Clean;
			fileLoggerMaof.AddEntry(order);	//log

			OrderEventNotifierMessage OrderMsg;

			switch (order.OrderState)
			{
				case OrderFSMState.SENT:
					//this happens if there is internal error on broker's server
					//i.e., internal fatal or operator decided not to continue when
					//asked for confirmation or re-enter value.
					//there is no AsmachtaFMR yet at this stage
					order.OrderState = OrderFSMState.IDLE;
					//inform the algo:
					OrderMsg = new OrderEventNotifierMessage(OrderProcessorEvent.Cleanup);
					order.eventNotifier(ref order.eventNotifierCookie, OrderMsg);
					order = null;   //clean
					break;
				case OrderFSMState.EXECUTED:
				case OrderFSMState.CANCELED:
				case OrderFSMState.FATAL:
					order.OrderState = OrderFSMState.IDLE;
					lock (OrdersHash)
					{
						OrdersHash.Remove(order.AsmachtaFMR);
					}
					OrderMsg = new OrderEventNotifierMessage(OrderProcessorEvent.Cleanup);
					order.eventNotifier(ref order.eventNotifierCookie, OrderMsg);
					order = null;
					break;
				default:
					HandleMessage_Default(message, order.OrderState);
					break;
			}
		}

		// Forced cleanup. Probably manually by the operator. 
		// An exit point from the FSM.
		private void HandleMessage_ForcedClean(OrderFSMMessage message)
		{
			MaofOrder order = (MaofOrder)message.msgData;
			order.lastEvent = OrderFSMEvent.ForcedClean;

			fileLoggerMaof.AddEntry(order);

			//no switch by states - forced works in any state
			order.OrderState = OrderFSMState.IDLE;

			//now check where is the order, and clean, if necessary
			if (OrdersHash.Contains(order.AsmachtaFMR))
			{
				lock (OrdersHash)
				{
					OrdersHash.Remove(order.AsmachtaFMR);
				}
			}
			fileLoggerMaof.AddEntry(order);

			order = null;
		}

		protected void HandleMessage_Default(OrderFSMMessage message, OrderFSMState state)
		{
			System.Console.WriteLine("Unhandeled event " + message.msgEvent + " in state " + state);
		}

		protected bool CheckHashKey(MOFINQType data, ref MaofOrder order)
		{
			int key = JQuant.Convert.StrToInt(data.SEQ_PIC); //TODO - make sure it's the right asmachta

			if (OrdersHash.ContainsKey(key))
			{
				//get the reference to the relevant order from the hashtable
				order = (MaofOrder)this.OrdersHash[key];

				return true;

			}
			else
			{
				//log here the MOFINQType struct for the non-existant order
				//create a dummy order object - contains only the poll data to be logged
				LMTOrderParameters parms = new LMTOrderParameters();
				parms.Security = new Stock();
				parms.Security.IdNum = 0;
				parms.Price = 0;
				parms.Quantity = 0;
				parms.TransactionType = TransactionType.BUY;
				order = new MaofOrder(parms, this.connection);
				//parse the message from the orders stream:
				order.lastPoll = data;
				fileLoggerMaof.AddEntry(order);
				//report to the operator
				System.Console.WriteLine("Security with id=" + data.BNO_PIC +
					", asmachtaFMR=  " + data.SEQ_PIC +
					" was reported by the orders stream, but not found in hash table of active orders");
				return false;
			}
		}

		#endregion;

		// I need a reference to an object containing active UserClass, as 
		// TaskBar.UserClass is necessary for operating orders through FMR's API
		public Connection connection
		{
			get;
			set;
		}

		/// <summary>
		/// this is a parameter storing the last time when any data was received from 
		/// orders stream. Only records past this time will be retrieved from the server, unless
		/// it's set to "00000000". We let PollOrderStream to keep this field updated.
		/// </summary>
		string OrdersLastTime;

		TimerTask timerTask;
		TimerList timers_fast;
		TimerList timers_TimeOut; //this one I use for TimeOut timers

		// watchdog timer - this timer is running most of the time
		// one second periodic timer. i start the timer in the constructor. when event arrives I print out that the event arrived and 
		// restart the timer. In the future I can add entry to the log. This can be useful to ensure that FSM was responsive to the 
		// external events
		TimerList timers_Watchdog;

		/// <summary>
		/// helpers - pool of threads to execute blocking operations which take
		/// time and can not be done in the context of the Order FSM
		/// </summary>
		JQuant.ThreadPool threadPool;

		/// <summary>
		/// I need a logger for the FSM
		/// </summary>
		FileLogger<MaofOrder> fileLoggerMaof;

		/// <summary>
		/// A storage where FSM keeps all the active orders, after their approval by the broker
		/// (after they get AsmachtaFMR). The key is AsmachtaFMR.
		/// I need this table in order to locate the reference to the order in case 
		/// I get updates when polling the orders stream.
		/// </summary>
		System.Collections.Hashtable OrdersHash;
	}
}//namespace FMRShell