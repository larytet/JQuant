
using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;

#if USEFMRSIM
using TaskBarLibSim;
#else
using TaskBarLib;
#endif

namespace JQuant
{
	// Here goes all the orders-related functionality:
	// orders, limit books etc.

	/// <summary>
	/// Either BUY or SELL
	/// </summary>
	public enum TransactionType
	{
		[Description("SELL")]
		SELL,

		[Description("BUY")]
		BUY
	}

	/// <summary>
	/// The values match all the order types possible on TASE.
	/// Attribute [Flags] allow to use boolean combinations of the order types
	/// for example order can be of type (LMT | FOK | IOC)
	/// </summary>
	[Flags]
	public enum OrderType
	{
		[Description("LMO")]
		LMO = 0x0001,    // Limit Opening - for Rezef securities only

		[Description("MKT")]
		MKT = 0x0002,    // MKT - for Rezef securities only

		[Description("LMT")]
		LMT = 0x0004,    // Limit

		[Description("IOC")]
		IOC = 0x0008,    // Immediate or Cancel - for options only

		[Description("FOK")]
		FOK = 0x0010,    // Fill or Kill - for options only

		[Description("GTC")]
		GTC = 0x0020,    // Good till cancel - this one is not on TASE, there are continous orders (esp. Rezef)

		[Description("EOD")]
		EOD = 0x0040     // End of day (on close) - this one is not on TASE
	}

	public enum CurrencyType
	{
		[Description("USD")]
		USD,    //thinking NASDAQ?

		[Description("EUR")]
		EUR,    //who knows - maybe one day ...

		[Description("GBP")]
		GBP,    //LSE is very strong in algos :)

		[Description("ILS")]
		ILS,     //humble me :) - I'd prefer to have it in ISO 4217

		[Description("CNY")]
		CNY     //now guess what is this one... :D
	}


	/// <summary>
	/// Base Order, from which all the other Order types inherit
	/// basically is a data holder
	/// </summary>
	public abstract class OrderBase
	{
	}

	/// <summary>
	/// This class inherits from the basic and serves itself as a base class for all limit
	/// orders - that is, for TASE, it's base for LMT, LMO, IOC and FOK order types
	/// The difference is that this class contains limit price, which is not present for MKT orders
	/// </summary>
	public abstract class LimitOrderBase : OrderBase
	{
		//constructor
		public LimitOrderBase(LMTOrderParameters orderParams)
		{
			this.orderParameters = orderParams;
		}

		public double Price
		{
			get
			{
				return orderParameters.Price;
			}

			set
			{
				orderParameters.Price = value;
			}
		}

		public int Quantity
		{
			get
			{
				return orderParameters.Quantity;
			}

			set
			{
				orderParameters.Quantity = value;
			}
		}

		public TransactionType TransactionType
		{
			get
			{
				return orderParameters.TransactionType;
			}
			//no set here because it's illegal to turn buy to sell and vice versa for existing order
		}

		public Security Security
		{
			get
			{
				return orderParameters.Security;
			}
			//no set here because it's impossible to change the security for existing order
		}

		protected LMTOrderParameters orderParameters;
	}

	/// <summary>
	/// Base class for MKT orders. No limit price is provided 
	/// </summary>
	public class MKTOrderBase : OrderBase
	{
	}

	/// <summary>
	/// this is a dataholder used for effective communication between 
	/// the order producer (algorithm) and the order processor (FSM)
	/// we are going to use exclusively LMT orders at this stage
	/// </summary>
	public struct LMTOrderParameters
	{
		public Security Security;               //what to trade?
		public TransactionType TransactionType; //buy or sell?
		public int Quantity;                    //how many?
		public double Price;                    //how much per unit?
	}


	/// <summary>
	/// contains price and size. can represent either a single order or limit book level
	/// TODO: we have a similar object in MarketSimulation, probably we can merge the two?
	/// </summary>
	public class LimitOrderPair : ICloneable
	{
		public LimitOrderPair(int price, int size)
		{
			this.price = price;
			this.size = size;
		}

		public LimitOrderPair()
		{
		}

		public double price;
		public int size;

		public object Clone()
		{
			LimitOrderPair op = new LimitOrderPair();
			op.price = this.price;
			op.size = this.size;
			return op;
		}

		public static string ToString(LimitOrderPair[] src)
		{
			string res = "    ";

			foreach (LimitOrderPair op in src)
			{
				res = res + op.price + " : " + op.size + "\t\t";
			}

			return res;
		}
	}

	/// <summary>
	/// Limit order book - an array of order pairs
	/// </summary>
	public class LimitOrderBook
	{
		/// <summary>
		/// On TASE order book has depth 3
		/// </summary>
		public LimitOrderBook()
		{
			Init(3);
		}

		public LimitOrderBook(int marketDepth)
		{
			Init(marketDepth);
		}


		protected void Init(int marketDepth)
		{
			bid = new LimitOrderPair[marketDepth];
			ask = new LimitOrderPair[marketDepth];


			for (int i = 0; i < marketDepth; i++)
			{
				bid[i] = new LimitOrderPair();
				ask[i] = new LimitOrderPair();
			}
		}
		public override string ToString()
		{
			string res =
				"-----------------------------------------------------------------------------" +
				Environment.NewLine +
				this.timestamp.ToString("hh:mm:ss.fff") + 
				Environment.NewLine + 
				Environment.NewLine + 
				"\t id= " + id + 
				"\t\t name= " + name + 
				Environment.NewLine + 
				Environment.NewLine +
				"\t BID: " + LimitOrderPair.ToString(bid) + Environment.NewLine +
				"\t ASK: " + LimitOrderPair.ToString(ask) + Environment.NewLine +
				Environment.NewLine +
				"\t LAST: \t" + lastTradeSize + " @ " + lastTradePrice +
				"\t time= " + this.lastTradeTime.ToString("hh:mm:ss.fff") + 
				"\t dv= " + dayVolume +
				Environment.NewLine + 
				"-----------------------------------------------------------------------------" 
				;

			res += "";
			return res;
		}

		// security ID - unique number (what is called Bno_Num on FMR)
		public int id;
		// security's name
		public string name;

		// three (or more, depending on the market depth) best asks and bids - price and size
		// best bid and best ask at the index 0
		public LimitOrderPair[] bid;
		public LimitOrderPair[] ask;


		// last deal price, size and time
		public double lastTradePrice;
		public int lastTradeSize;
		public DateTime lastTradeTime;

		// Aggregated trading data over the trading period (day)
		public int dayVolume;        //volume

		// time stamps - latest change in the book
		public DateTime timestamp; 
		public long tick;
	}


	#region Interfaces


	public enum OrderProcessorEvent
	{
		[Description("New order initialized by Order Processor")]
		InitOrder,
		[Description("Order Processor submitted the order for trading")]
		OrderSent,
		[Description("Error - the current operation can't be completed")]
		Error,
		[Description("Order was approved for trading by the exchange")]
		TradingApproved,
		[Description("Order Processor have sent Cancel request for the order")]
		CancelSent,
		[Description("Order Processor have sent Update request for the order")]
		UpdateSent,
		[Description("Order update was successfully accomplished")]
		Updated,
		[Description("Order cancelation was successfully accomplished")]
		Canceled,
		[Description("The order got partial fill")]
		PartialFill,
		[Description("Order fill fully completed")]
		CompleteFill,
		[Description("Order cleaned and discarded from the processor")]
		Cleanup,
	};

	public delegate void OrderEventNotifier(ref object cookie, FMRShell.OrderEventNotifierMessage msg);

	/// <summary>
	/// Any order processor (OrderFSM) will implement this interface
	/// </summary>
	public interface IOrderProcessor
	{
	}

	/// <summary>
	/// this interface is what external world sees
	/// Order processor will see all the fields
	/// </summary>
	public interface IOrder
	{
	}
	#endregion


}//namespace JQuant
