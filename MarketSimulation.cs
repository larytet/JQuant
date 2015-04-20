
using System;
using System.ComponentModel;

namespace MarketSimulation
{
	public enum ReturnCode
	{
		[Description("NoError")]
		NoError,
		[Description("UnknownError")]
		UnknownError,
		[Description("OutOfMemory")]
		OutOfMemoryError,
		[Description("Fill")]
		Fill,
		[Description("PartialFill")]
		PartialFill,
		[Description("UnknownSecurity")]
		UnknownSecurity
	};

	public enum OrderState
	{
		[Description("Pending")]
		Pending,
		[Description("Canceled")]
		Canceled,
		[Description("Fill")]
		Fill,
		[Description("PartialFill")]
		PartialFill
	};

	/// <summary>
	/// I need a public API and I do not want to expose any details of LimitOrder
	/// Application will use objects of this type to place and cancel orders
	/// </summary>
	public interface ISystemLimitOrder
	{
		/// <summary>
		/// unique ID of the order
		/// </summary>
		int Id
		{
			set;
			get;
		}

		/// <summary>
		/// unique ID of the security
		/// </summary>
		int SecurityId
		{
			set;
			get;
		}

		/// <summary>
		/// limit price
		/// </summary>
		int Price
		{
			set;
			get;
		}

		/// <summary>
		/// fill price
		/// </summary>
		int FillPrice
		{
			set;
			get;
		}

		long FillTick
		{
			set;
			get;
		}
	}

	public class OrderPair : ICloneable
	{
		public OrderPair(int price, int size)
		{
			this.price = price;
			this.size = size;
		}

		public OrderPair()
		{
		}

		public int price;
		public int size;

		public object Clone()
		{
			OrderPair op = new OrderPair();
			op.price = this.price;
			op.size = this.size;
			return op;
		}

		/// <summary>
		/// I have no idea if CSharp array knows to clone itself
		/// I put here some trivial implementation
		/// </summary>
		public static OrderPair[] Clone(OrderPair[] src)
		{
			OrderPair[] dst = new OrderPair[src.Length];
			for (int i = 0; i < dst.Length; i++)
			{
				dst[i] = (OrderPair)src[i].Clone();
			}

			return dst;
		}

		public static string ToString(OrderPair[] src)
		{
			string res = "";
			foreach (OrderPair op in src)
			{
				res = res + op.price + ":" + op.size + " ";
			}

			return res;
		}
	}

	/// <summary>
	/// Describes an asset, for example, Maof option on TASE. 
	/// This class is used in the MarketSimulation. 
	/// All data (prices / quantities) are integers. If required in cents/agorots. 
	/// This object is expensive to create. 
	/// Application will reuse objects or create pool of objects.
	/// </summary>
	public class MarketData : ICloneable
	{
		/// <summary>
		/// On TASE order book has depth 3
		/// </summary>
		public MarketData()
		{
			Init(3);
		}

		public MarketData(int marketDepth)
		{
			Init(marketDepth);
		}


		protected void Init(int marketDepth)
		{
			bid = new OrderPair[marketDepth];
			ask = new OrderPair[marketDepth];


			for (int i = 0; i < marketDepth; i++)
			{
				bid[i] = new OrderPair();
				ask[i] = new OrderPair();
			}
		}


		public object Clone()
		{
			MarketData md = new MarketData(bid.Length);

			md.ask = OrderPair.Clone(this.ask);
			md.bid = OrderPair.Clone(this.bid);
			md.id = this.id;
			md.lastTrade = this.lastTrade;
			md.lastTradeSize = this.lastTradeSize;
			md.dayVolume = this.dayVolume;
			md.tick = this.tick;

			return md;
		}

		public override string ToString()
		{
			string res = "id=" + id + " bids=" + OrderPair.ToString(bid) + " asks=" + OrderPair.ToString(ask) + " lt=" + lastTrade + ":" + lastTradeSize + " dv=" + dayVolume;
			return res;
		}

		// security ID - unique number
		public int id;

		// three (or more, depending on the market depth) best asks and bids - price and size
		// best bid and best ask at the index 0
		public OrderPair[] bid;
		public OrderPair[] ask;


		// last deal price and size
		public int lastTrade;
		public int lastTradeSize;

		// Aggregated trading data over the trading period (day)
		public int dayVolume;        //volume

		// time stamp 
		public long tick;
	}


	/// <summary>
	/// Simulates real market behaviour. This simulaition is for back testing and requires
	/// feed of historical data. Lot of assumptions, like placed orders do not change the market, etc.
	/// 
	/// Notation:
	///     there are two types of limit orders in the market simulation:
	///     - "system" order is limit order simulated by the trading algorithm
	///     - "internal" order is limit order generated from the historical log, internally
	///     For the sake of simplicity we assume that system orders are invisible to the market, 
	///     and they don't affect trading (no. of transactions etc.). 
	/// </summary>
	public class Core : JQuant.IResourceStatistics, IDisposable
	{
		/// <summary>
		/// Market simulation will call this method if and when order state changes.
		/// Delegate method allows to place orders from different threads and state machines. 
		/// </summary>
		public delegate void OrderCallback(MarketSimulation.ReturnCode errorCode, ISystemLimitOrder lo, int quantity);

		/// <summary>
		/// Market simulation will call this method if and when any change in the security added to
		/// the watchlist
		/// </summary>
		public delegate void WatchlistCallback(MarketSimulation.MarketData md);

		/// <summary>
		/// There are two sources of the orders placed by the trading algorithm
		/// - system orders (simulated) - placed by the trading algorithm
		/// - non-system orders, placed internally (by market simulation itself), 
		///   based on the order book updates from the log
		/// </summary>
		protected class LimitOrder : JQuant.LimitOrderBase, ISystemLimitOrder
		{

			/// <summary>
			/// Order is placed by the system (algorithm)
			/// </summary>
			public LimitOrder(JQuant.LMTOrderParameters orderParams, OrderCallback callback)
				: base(orderParams)
			{
				Init(orderParams.Security.IdNum, (int)orderParams.Price, orderParams.Quantity, orderParams.TransactionType, callback);
			}

			/// <summary>
			/// this constructor is used to create non-system (internal, log) orders, 
			/// where only order size is important. 
			/// I keep price and transaction type for internal checks only.
			/// </summary>
			public LimitOrder(JQuant.LMTOrderParameters orderParams)
				: base(orderParams)
			{
				Init(0, (int)orderParams.Price, orderParams.Quantity, orderParams.TransactionType, null);
			}

			public void UpdateQuantity(int quantity)
			{
				this.Quantity = quantity;
			}

			/// <summary>
			/// Builds an order object tracking real (internal) or simulated (system) orders.
			/// </summary>
			/// <param name="id">Order's id number</param>
			/// <param name="price">limit price</param>
			/// <param name="quantity">order's quantity</param>
			/// <param name="transaction">buy or sell</param>
			/// <param name="callback">callback method </param>
			protected void Init(int security, int price, int quantity, JQuant.TransactionType transaction, OrderCallback callback)
			{
				this.SecurityId = security;
				this.callback = callback;
				this.Price = price;
				this.Quantity = quantity;
				state = OrderState.Pending;
				this.orderParameters.TransactionType = transaction;
			}

			/// <summary>
			/// Is the order an system (placed by the algorithm, True) 
			/// or internal (read from the log, False)?
			/// </summary>
			/// <returns>
			/// A <see cref="System.Boolean"/> false if the order is internal order
			/// </returns>
			public bool SystemOrder()
			{
				return (callback != null);
			}

			/// <summary>
			/// unique ID of the order
			/// This field is provided by the application and ignored by the simulation
			/// </summary>
			public int Id
			{
				set;
				get;
			}

			public int FillPrice
			{
				set;
				get;
			}

			public long FillTick
			{
				set;
				get;
			}

			public int SecurityId
			{
				set;
				get;
			}

			public new int Price
			{
				set;
				get;
			}


			/// <summary>
			/// method to call when the order status changes
			/// </summary>
			public OrderCallback callback;


			/// <summary>
			/// State of the order - pending, canceled, etc.
			/// </summary>
			public OrderState state;


			/// <summary>
			/// Order book at the moment when the order was placed. Queue size is amount of equal or better
			/// orders according to the order book. Every time deal is closed at better or equal price 
			/// i move the order in the queue - queueSize goes to zero.
			/// </summary>
			public int queueSize;

			/// <summary>
			/// daily aggregate trading volume when the order was placed
			/// change in its value indicates new transaction
			/// </summary>
			public int volume;
		}


		/// <summary>
		/// I keep object of this type for every security
		/// </summary>
		protected class FSM
		{
			public FSM(MarketData marketData, FillOrderBook fillOrderCallback)
			{
				// orders = new System.Collections.ArrayList(3);
				this.marketData = marketData;
				orderBookAsk = new OrderBook(marketData.id, JQuant.TransactionType.SELL, fillOrderCallback);
				orderBookBid = new OrderBook(marketData.id, JQuant.TransactionType.BUY, fillOrderCallback);
				watchlistCallback = null;
				watchEnable = false;
			}

			/// <summary>
			/// keep all ask orders here
			/// </summary>
			public OrderBook orderBookAsk;

			/// <summary>
			/// keep all bid orders here
			/// </summary>
			public OrderBook orderBookBid;

			/// <summary>
			/// I keep reference to the latest update for debug
			/// </summary>
			public MarketData marketData;

			/// <summary>
			/// if not null this security added to the watchlist
			/// every time there is an update i will call application callback
			/// </summary>
			public WatchlistCallback watchlistCallback;
			public bool watchEnable;
		}


		/// <summary>
		/// OrderQueue and OrderBook will call this method to let know upper layers that the
		/// system order (order placed by the application) got fill
		/// </summary>
		protected delegate void FillOrderBook(OrderBook orderBook, LimitOrder order, int quantity);
		protected delegate void FillOrderQueue(OrderQueue orderQueue, LimitOrder order, int quantity);

		protected class OrderQueue : JQuant.IResourceStatistics
		{

			/// <summary>
			/// i keep price and transaction type only for debug purposes
			/// </summary>
			public OrderQueue(int securityId, int price, JQuant.TransactionType transaction, FillOrderQueue fillOrder)
			{
				this.price = price;
				this.transaction = transaction;
				this.securityId = securityId;
				this.fillOrder = fillOrder;

				// crete queue of orders and preallocate a couple some entries.                
				orders = new System.Collections.Generic.LinkedList<MarketSimulation.Core.LimitOrder>();
			}

			/// <summary>
			/// Add a non-system (internal) order to the end of the queue
			/// I have different cases here
			/// - list is empty
			/// - list's tail is occupied by the system order
			/// - list's tail is occupied by non-sysem order (placed by algorithm)
			/// </summary>
			public void AddOrder(int quantity, bool removeSystem, long tick)
			{
				if (quantity < 0)
				{
					RemoveOrder(-quantity, removeSystem, tick); //System order cancellation?
					return;
				}
				// i am adding entries to the list and I have no idea how many threads
				// will attempt the trick concurrently
				lock (orders)
				{
					countAddOrderTotal++;

					if ((orders.Count == 0) ||  // list is empty OR
					   orders.Last.Value.SystemOrder()) // last order is a system order
					{
						if (orders.Count == 0) countAddOrderToEmpty++;
						else countAddOrderAfter++;

						// create a new order, add to the end of list
						JQuant.LMTOrderParameters p = new JQuant.LMTOrderParameters();
						p.Price = price;
						p.Quantity = quantity;
						p.TransactionType = transaction;

						LimitOrder lo = new LimitOrder(p);
						orders.AddLast(lo);
					}
					else // I keep the order queue as short as possible
					// because the exact structure of the queue is unimportant
					// - keep only blocks of system orders, around non-system ones
					{    // increment quantity in the last (tail) internal order
						countAddOrderUpdate++;
						LimitOrder lastOrder = orders.Last.Value;
						lastOrder.UpdateQuantity(quantity + lastOrder.Quantity);
					}

					sizeTotal += quantity;
					sizeInternal += quantity;
				} // lock (orders)
			}

			/// <summary>
			/// Add system order - always to the end of list
			/// </summary>
			public void AddOrder(LimitOrder order)
			{
				lock (orders)
				{
					System.Console.WriteLine(ShortDescription() + ": place order " + order.TransactionType + " price=" + order.Price + " system=" + order.SystemOrder() + " placeInQ=" + sizeTotal);
					orders.AddLast(order);
					countAddSystemOrder++;
					sizeSystem += order.Quantity;
					// sizeTotal += order.Quantity;
				}
			}

			/// <summary>
			/// Remove specified number of securities from the head of the list
			/// This method handles case when a system order gets fill
			/// </summary>
			public void RemoveOrder(int quantity, bool removeSystem, long tick)
			{
				lock (orders)
				{
					counRemoveOrderTotal++;

					// can I remove so many securities ?
					if ((sizeTotal + sizeSystem) < quantity)
					{
						// System.Console.WriteLine(ShortDescription() + " Can't remove " + quantity + " from " + sizeTotal + " total");
						quantity = (sizeTotal + sizeSystem);
					}

					// loop until quantity is removed or the list is empty
					// i handle two cases here - (1) remove of internal and (2) remove of system orders
					// (1) - this is order placed by the system. Because it's is a simulation, i have to "fix" the order queue:
					// i will remove both system orders and internal orders - i double the quantity of orders removed. 
					// This way i restore the queue like system order fill never happened.
					while ((quantity > 0) && (orders.Count > 0))
					{
						LimitOrder lo = orders.First.Value;

						if (removeSystem && lo.SystemOrder())
						{
							// quantity -= lo.Quantity;  - i am NOT going to do this. market does NOT see me

							int fillSize = Math.Min(quantity, lo.Quantity);  // can be partial fill - let upper layer handle this
							// sizeSystem -= fillSize;
							sizeSystem -= lo.Quantity; // no partial fills
							lo.FillPrice = price;
							lo.FillTick = tick;

							// if not partial fill - remove the order from the queue
							if (fillSize >= lo.Quantity) //i got fill
							{
								orders.RemoveFirst();
								// notify upper layer (orderbook) that the order got fill
								fillOrder(this, lo, fillSize);
								// sizeTotal -= lo.Quantity;
							}
							else // this is partial fill - just update the quantity
							{
								fillSize = lo.Quantity;
								// lo.UpdateQuantity(lo.Quantity - fillSize);
								// i have to skip the node in the list, but at this point i print error and remove the entry 
								System.Console.WriteLine(ShortDescription() + " Don't know to hanle partial fills order " + lo.Id);
								orders.RemoveFirst();
								// notify upper layer that the order got fill
								fillOrder(this, lo, fillSize);
								// sizeTotal -= fillSize;
							}
						}
						else if (!removeSystem && lo.SystemOrder())
						{
							break;  // keep system order in the head of list
						}
						else  // internal order
						{
							if (lo.Quantity <= quantity)  // remove the whole node
							{
								quantity -= lo.Quantity;
								sizeTotal -= lo.Quantity;
								orders.RemoveFirst();
							}
							else  // just update the Quantity in the first node
							{
								JQuant.LMTOrderParameters p = new JQuant.LMTOrderParameters();
								p.Price = this.price;
								p.Quantity = lo.Quantity - quantity;
								p.TransactionType = this.transaction;

								lo = new LimitOrder(p);
								orders.First.Value = lo;
								sizeTotal -= quantity;
								quantity = 0;
							}
							sizeInternal -= quantity;
						}
					}
				}  // lock (orders)

			}


			/// <summary>
			/// Remove system order. This is result of order cancel
			/// </summary>
			public void RemoveOrder(LimitOrder order)
			{
				lock (orders)
				{
					System.Collections.Generic.LinkedListNode<LimitOrder> node = orders.Find(order);
					if (node != null)
					{
						sizeSystem -= order.Quantity;
						// sizeTotal -= order.Quantity;
						orders.Remove(node);
					}
					else
					{
						System.Console.WriteLine(ShortDescription() + "Can't remove order " + order.Id + ". Not found in the list");
					}
				}
			}


			protected string ShortDescription()
			{
				string sd = "Order queue " + this.securityId + " " + this.transaction.ToString() + " " + this.price;
				return sd;
			}


			public void GetEventCounters(out System.Collections.ArrayList names, out System.Collections.ArrayList values)
			{
				names = new System.Collections.ArrayList(12);
				values = new System.Collections.ArrayList(12);

				/*0*/
				names.Add("securityId");
				values.Add(securityId);
				/*1*/
				names.Add("price");
				values.Add(price);
				/*2*/
				names.Add("Sell");
				values.Add(transaction == JQuant.TransactionType.SELL);
				/*3*/
				names.Add("ListSize");
				values.Add(orders.Count);
				/*4*/
				names.Add("SizeTotal");
				values.Add(sizeTotal);
				/*5*/
				names.Add("sizeInternalTotal");
				values.Add(sizeInternal);
				/*6*/
				names.Add("sizeSystemTotal");
				values.Add(sizeSystem);
				/*7*/
				names.Add("countAddOrderToEmpty");
				values.Add(countAddOrderToEmpty);
				/*8*/
				names.Add("countAddOrderAfter");
				values.Add(countAddOrderAfter);
				/*9*/
				names.Add("countAddOrderUpdate");
				values.Add(countAddOrderUpdate);
				/*10*/
				names.Add("countAddOrderTotal");
				values.Add(countAddOrderTotal);
				/*11*/
				names.Add("countAddSystemOrder");
				values.Add(countAddSystemOrder);
			}

			/// <summary>
			/// Returns aggregated size of all orders waiting in the queue
			/// </summary>
			public int GetSize()
			{
				return (sizeTotal + sizeSystem);
			}

			public bool ContainsSystemOrders()
			{
				return (sizeSystem > 0);
			}

			public int GetSizeInernal()
			{
				return sizeInternal;
			}

			public int GetPrice()
			{
				return price;
			}

			protected System.Collections.Generic.LinkedList<LimitOrder> orders;

			protected int price;
			protected int securityId;
			protected JQuant.TransactionType transaction;

			/// <summary>
			/// total size of orders - summ of all bids (asks)
			/// </summary>
			protected int sizeTotal;

			/// <summary>
			/// size of internal orders
			/// </summary>
			protected int sizeInternal;

			/// <summary>
			/// size of orders placed by the application (by system)
			/// </summary>
			protected int sizeSystem;

			protected int countAddOrderToEmpty;
			protected int countAddOrderAfter;
			protected int countAddOrderUpdate;
			protected int countAddOrderTotal;
			protected int countAddSystemOrder;
			protected int counRemoveOrderTotal;
			protected FillOrderQueue fillOrder;
		}  // class OrderQueue

		/// <summary>
		/// TASE publishes order book of depth three - three slots for asks and three slots for bids.
		/// I arrange book order as a list of slots ordered by price. 
		/// The size of the list is not neccessary three.
		/// </summary>
		protected class OrderBook : JQuant.IResourceStatistics
		{
			/// <summary>
			/// Create OrderBook - i keep two books for every security. 
			/// One book for asks and another book for bids.
			/// I keep security id for debug.
			/// Argument "fillOrder" is a method to call when a system order got fill.
			/// </summary>
			public OrderBook(int securityId, JQuant.TransactionType transaction, FillOrderBook fillOrder)
			{
				this.securityId = securityId;
				this.transaction = transaction;
				this.fillOrder = fillOrder;

				System.Collections.Generic.IComparer<int> iComparer;

				// i keep order queue in the list slots ordered
				// best bid (ask) should be at the head of the list index 0
				if (transaction == JQuant.TransactionType.SELL)
					iComparer = new AskComparator();
				else
					iComparer = new BidComparator();

				slots = new System.Collections.Generic.SortedList<int, OrderQueue>(10, iComparer);

				// i am creating a dummy data - mostly zeros. Any first real data will cause 
				// the queue to reinitialize
				marketData = new MarketData(3);
				marketData.id = securityId;
				marketData.lastTrade = 0;
				marketData.lastTradeSize = 0;
				marketData.dayVolume = 0;
			}

			/// <summary>
			/// If there is no system orders updating of the book is trivial - i just store the market data
			/// The things get interesting when a first order is placed. I allocate the order queues and 
			/// update will update the queues.
			/// </summary>
			public void Update(MarketData md)
			{
				if (sizeSystem <= 0) // most likely there are no system orders - i will store the market data and get out
				{                    // i want to be fast 
					marketData = md;
				}
				else // there are pending system orders
				{
					// i have previous record (marketData) and current record (md)
					// let's figure out what happened
					Update_trade(md);  // probably a trade ?
					Update_queue(md);  // probably some orders are pulled out or added ?

					// finally replace the data
					marketData = md;
				}
			}

			/// <summary>
			/// Add a system order to the order book.
			/// </summary>
			public void PlaceOrder(LimitOrder order)
			{
				OrderQueue orderQueue = null;
				int price = (int)order.Price;

				// i am looking for an order queue with specific price
				lock (slots)
				{
					bool containsKey = slots.TryGetValue(price, out orderQueue);
					if (!containsKey)  // there is no such order queue  - create a new one 
					{                        // and add it to the order book
						orderQueue = new OrderQueue(this.securityId, price, this.transaction, FillOrderCallback);
						if (enableTrace)
						{
							System.Console.WriteLine("OrderBook placeOrder add slot price=" + order.Price);
						}
						slots.Add(price, orderQueue);
					}
					// add order to the queue - existing queue or one just created 
					orderQueue.AddOrder(order);
					// i have a system order in the book. update counter
					sizeSystem += order.Quantity;
				}
			}

			/// <summary>
			/// The method is called if in the Update() i discover that there was a trade
			/// I have to figure out size of the trade and remove the traded securities from the queue
			/// </summary>
			protected void Update_trade(MarketData md)
			{
				long tick = md.tick;
				int tradePrice = md.lastTrade;
				int tradeSize = (md.dayVolume - marketData.dayVolume);
				int totalToRemove = tradeSize;
				if (tradeSize < 0)
					System.Console.WriteLine(ShortDescription() + " negative change in day volume from " + md.dayVolume + " to " +
											marketData.dayVolume + " are not consistent");
				if ((tradeSize > 0) && (tradeSize != md.lastTradeSize))  // sanity check - any misssing records out there ?
					System.Console.WriteLine(ShortDescription() + " last trade size " + md.lastTradeSize +
											" and change in day volume from " + md.dayVolume + " to " + marketData.dayVolume + " are not consistent");

				if (tradeSize > 0) // there was a trade ? let's check the price 
				{                       // of the deal and size of the deal					
					lock (slots)
					{
						foreach (OrderQueue orderQueue in slots.Values)  // i remove the traded securities from the queue(s) starting 
						{                                                // from the best (head of the ordered list "slots")
							int price = orderQueue.GetPrice();

							if ((transaction == JQuant.TransactionType.BUY) && (tradePrice > price)) break; // i can't remove queues for
							if ((transaction == JQuant.TransactionType.SELL) && (tradePrice < price)) break; // which the last trade price is not good enough

							int size0 = orderQueue.GetSize();           // get queue size
							orderQueue.RemoveOrder(tradeSize, true, tick);    // remove the trade, "true" means handle system orders too
							int size1 = orderQueue.GetSize();           // get queue size

							int removed = size0 - size1;
							tradeSize -= removed;
							if (tradeSize == 0)  // i removed the trade completely ?
							{
								break;
							}
						} // foreach
					}
				} // tradeSize > 0 - there was a trade

				if (tradeSize > 0)
				{
					// if i reached here i have accounting problem. The trade i see in the log is larger than 
					// total of all positions in the order queue. such large trade could not take place unless
					// i have wrong total position or order book information is not complete
					if (enableTrace)
					{
						System.Console.WriteLine(ShortDescription() + " failed to remove the trade remains " + tradeSize + " from " + totalToRemove);
						System.Console.WriteLine("NewData=" + md.ToString());
						System.Console.WriteLine("CurData=" + marketData.ToString());
					}
				}
			}


			/// <summary>
			/// This method is called to check if there was a change in the order queues, for
			/// example some orders were pulled or some orders were added to the order queues
			/// </summary>
			protected void Update_queue(MarketData md)
			{
				long tick = md.tick;
				OrderPair[] mdBookOrders;
				OrderPair[] marketDataBookOrders;

				if (this.transaction == JQuant.TransactionType.SELL) // i am in an ask book
				{
					mdBookOrders = md.ask;
					marketDataBookOrders = marketData.ask;
				}
				else  // i am in a bid book 
				{
					mdBookOrders = md.bid;
					marketDataBookOrders = marketData.bid;
				}

				int size = mdBookOrders.Length;
				System.Collections.ArrayList touchedQueues = new System.Collections.ArrayList(5);
				for (int i = 0; i < size; i++)                              // get a slot (price) from the order book
				{                                                           // and check if I already have such slot
					int mdPrice = mdBookOrders[i].price;
					int mdSize = mdBookOrders[i].size;
					// find slot with this price
					OrderQueue orderQueue = null;
					lock (slots)
					{
						slots.TryGetValue(mdPrice, out orderQueue);
						// if (slots.ContainsKey(mdPrice)) orderQueue = (OrderQueue)(slots[mdPrice]);
					}
					if (orderQueue != null)  // two cases - i already have a slot for this price (typical) ...
					{
						int size_cur = orderQueue.GetSize();

						// i just update size of the queue. i take optimistic approach 
						// and add the orders to the tail of the queue. 
						// and remove from the head. I do not remove the system orders here
						// Method AddOrder() handles negative numbers too
						orderQueue.AddOrder(mdSize - size_cur, false, tick);
					}
					else                      // ... and there is no such slot - add a new slot
					{
						orderQueue = new OrderQueue(this.securityId, mdPrice, this.transaction, FillOrderCallback);
						orderQueue.AddOrder(mdSize, false, tick);
						lock (slots)
						{
							slots.Add(mdPrice, orderQueue);  // add newly created queue to the list of queues sorted by price
						}
						if (enableTrace)
						{
							System.Console.WriteLine("OrderBook add slot price=" + mdPrice); System.Console.WriteLine("Cur=" + marketData.ToString()); System.Console.WriteLine("New=" + md.ToString());
						}
					}
					// add the queue to the list of touched queues
					touchedQueues.Add(orderQueue);
				}


				// remove all queues which are not touched and do not contain system orders
				RemoveQueues(touchedQueues, md);
			}

			/// <summary>
			/// Helper removes all queues which are NOT on the list
			/// </summary>
			protected void RemoveQueues(System.Collections.ArrayList keepQueues, MarketData md)
			{
				lock (slots)
				{
					foreach (OrderQueue oq in slots.Values)
					{
						bool containsSystemOrders = oq.ContainsSystemOrders();
						if ((!containsSystemOrders) && (!keepQueues.Contains(oq)))
						{
							int price = oq.GetPrice();
							slots.Remove(price);
							if (enableTrace)
							{
								System.Console.WriteLine("OrderBook remove slot price=" + price);
								System.Console.WriteLine("Cur=" + marketData.ToString());
								System.Console.WriteLine("New=" + md.ToString());
							}
						}
					} // foreach 
				} // lock (slots)
			}


			/// <summary>
			/// This method is called from the order queue
			/// I forward the call to application
			/// </summary>
			protected void FillOrderCallback(OrderQueue queue, LimitOrder order, int fillSize)
			{
				// this is actually call to the application callback and 
				// last chance for the MarketSimulation to manipulate the order
				this.fillOrder(this, order, fillSize);
			}

			public void EnableTrace(bool enable)
			{
				this.enableTrace = enable;
			}

			/// <summary>
			/// Application calls the method to check if there is immediate fill is possible.
			/// For the ask order books of bids will be checked.
			/// </summary>
			/// <returns>
			/// A <see cref="System.Boolean"/>
			/// </returns>
			public bool CheckImmediateFill(LimitOrder order)
			{
				JQuant.TransactionType orderTransaction = order.TransactionType;
				bool res = false;

				// book of asks for the buy order
				// book of bids for the sell order
				if (this.transaction == orderTransaction)
				{
					System.Console.WriteLine(ShortDescription() + " Check fill for order " + order.Id + " " + order.TransactionType + " at " +
											 order.Price + " in the book " + BookTypeName());
				}
				else if (orderTransaction == JQuant.TransactionType.SELL) // check if there is a higher or equal bid 
				{
					if (order.Price <= marketData.bid[0].price)
					{
						res = true;
						order.FillPrice = marketData.bid[0].price;
					}
				}
				else // buy order - check if there is lower or equal ask
				{
					if (order.Price >= marketData.ask[0].price)
					{
						res = true;
						order.FillPrice = marketData.ask[0].price;
					}
				}
				return res;
			}

			/// <summary>
			/// Return "BID" or "ASK"
			/// </summary>
			protected string BookTypeName()
			{
				if (this.transaction == JQuant.TransactionType.SELL)
				{
					return "ASK";
				}
				return "BID";
			}

			protected string ShortDescription()
			{
				string sd = "Order book " + this.securityId + " " + BookTypeName() + " ";
				return sd;
			}

			public void GetEventCounters(out System.Collections.ArrayList names, out System.Collections.ArrayList values)
			{
				names = new System.Collections.ArrayList(4);
				values = new System.Collections.ArrayList(4);
				/* 0 */
				names.Add("SecurityId");
				values.Add(securityId);
				/* 1 */
				names.Add("Sell");
				values.Add(transaction == JQuant.TransactionType.SELL);
				/* 2 */
				names.Add("ListSize");
				values.Add(slots.Count);
				/* 3 */
				// name SizeSystem is used in the debug 
				names.Add("SizeSystem");
				values.Add(sizeSystem);
			}

			/// <summary>
			/// Used to help sort and compare order book of bids.
			/// </summary>
			protected class BidComparator : System.Collections.Generic.IComparer<int>
			{
				public int Compare(int x, int y)
				{
					// higher bid is closer to the head of the sorted list
					return (y - x);
				}
			}

			/// <summary>
			/// Used to help sort and compare order book of asks.
			/// </summary>
			protected class AskComparator : System.Collections.Generic.IComparer<int>
			{
				public int Compare(int x, int y)
				{
					// higher ask is closer to the tail of the sorted list
					return (x - y);
				}
			}

			public OrderQueue[] GetQueues()
			{
				System.Collections.Generic.IList<OrderQueue> collection = slots.Values;
				OrderQueue[] oqs = new OrderQueue[collection.Count];
				collection.CopyTo(oqs, 0);

				return oqs;
			}

			public MarketData GetMarketData()
			{
				return this.marketData;
			}

			/// <summary>
			/// Slots are ordered by price
			/// Ask order queues are ordered from lower price to higher price and 
			/// book of bids aranged from higher price to lower price.
			/// </summary>
			protected System.Collections.Generic.SortedList<int, OrderQueue> slots;

			/// <summary>
			/// total size of all orders placed by the system.
			/// </summary>
			protected int sizeSystem;
			protected MarketData marketData;
			protected int securityId;
			protected JQuant.TransactionType transaction;
			protected FillOrderBook fillOrder;
			protected bool enableTrace;
		} // class OrderBook

		/// <summary>
		/// Use child class (wrapper) like MarketSimulationMaof to create instance of the MarketSimulation
		/// </summary>
		protected Core()
		{
			// create hash table where all securities are stored
			securities = new System.Collections.Hashtable(200);
			enableTrace = new System.Collections.Hashtable(200);

			filledOrdersThread = new FilledOrdersThread();
			filledOrdersThread.Start();

			watchThread = new WatchThread();
			watchThread.Start();
		}


		/// <summary>
		/// Implement IDisposable.
		/// Call this method to clean up the MarketSimulation
		/// The method stops threads 
		/// </summary>
		public void Dispose()
		{
			filledOrdersThread.Stop();  // stop the helper threads - 
			watchThread.Stop();         // Stop() calls MailboxThread.Dispose()

			// help the grabage collector to free large data blocks faster
			filledOrdersThread = null;
			watchThread = null;
			securities = null;
		}

		public void GetEventCounters(out System.Collections.ArrayList names, out System.Collections.ArrayList values)
		{
			names = new System.Collections.ArrayList(8);
			values = new System.Collections.ArrayList(8);
			/*1*/
			names.Add("Events");    //total number of lines (events) read from the historical log
			values.Add(eventsCount);
			/*2*/
			names.Add("OrdersPlaced");  //number of system order placed
			values.Add(ordersPlacedCount);
			/*3*/
			names.Add("OrdersFilled");
			values.Add(ordersFilledCount);
			/*4*/
			names.Add("OrdersCanceled");
			values.Add(ordersCanceledCount);
			/*5*/
			names.Add("Securities");    //number of tradable securities (in the hashtable)
			values.Add(securities.Count);
		}

		/// <summary>
		/// The method is being called by Event Generator to notify the market simulation, that
		/// there is a new event went through, for example change in the order book.
		/// Argument "data" can be reused by the calling thread. If the data is processed 
		/// asynchronously, Notify() should clone the object.
		/// In the current setup MarketSimulationMaof.Notify calls the method
		/// </summary>
		public void Notify(int count, MarketData dataIn)
		{
			// clone the data first - cloning is expensive, but i have no choice right now
			MarketData data = (MarketData)dataIn.Clone();

			// GetKey() will return security id
			object key = GetKey(data);
			object fsm;

			lock (securities)
			{
				// hopefully Item() will return null if there is no key in the hashtable
				fsm = securities[key];

				// Do I see this security very first time? Then add new entry to the hashtable.
				// This is not likely outcome (occurs several tens of times at the start of the trading sesssion). 
				// Performance is not an issue at this point
				if (fsm == null)
				{
					fsm = new FSM(data, FillOrderCallback);
					securities[key] = fsm;

					// get the security from the hash table in all cases
					// this line can be removed
					fsm = securities[key];

					// enable trace in the order books if required
					if (enableTrace[((FSM)fsm).marketData.id] != null)
					{
						((FSM)fsm).orderBookAsk.EnableTrace(true);
						((FSM)fsm).orderBookBid.EnableTrace(true);
					}

				}
			}

			// i am going to call update in case if an order just being placed
			// by concurrently running thread
			UpdateSecurity((FSM)fsm, data);
		}

		/// <summary>
		/// This method is called from the order book
		/// I forward the call to the application
		/// </summary>
		protected void FillOrderCallback(OrderBook book, LimitOrder order, int fillSize)
		{
			// add order to the list of orders which got fill
			filledOrdersThread.Send(order);
			ordersFilledCount++;
		}

		/// <summary>
		/// Returns key for the hashtable
		/// The implemenation is trivial - return BNO_Num
		/// </summary>
		protected static object GetKey(MarketData data)
		{
			return data.id;
		}


		/// <summary>
		/// Let the order books handle the update, in case there is a log or system event. 
		/// </summary>
		protected void UpdateSecurity(FSM fsm, MarketData marketData)
		{
			// bump event counter
			eventsCount++;

			fsm.marketData = marketData;

			fsm.orderBookAsk.Update(marketData);
			fsm.orderBookBid.Update(marketData);

			// notify application on change
			if (fsm.watchEnable)
			{
				watchThread.Send(fsm);
			}
		}

		/// <summary>
		/// Start not from zero - looks better in  print 
		/// </summary>
		protected int orderId = 111;

		protected int GenerateOrderId()
		{
			int res;

			lock (this)
			{
				res = orderId;
				orderId++;
			}

			return res;
		}

		/// <summary>
		/// Use this method to create orders. Property ISystemLimitOrder.Id will contain unique order Id
		/// </summary>
		/// <returns>
		/// A <see cref="ISystemLimitOrder"/>
		/// </returns>
		public ISystemLimitOrder CreateOrder(int security, int price, int quantity, JQuant.TransactionType transaction, OrderCallback callback)
		{
			JQuant.LMTOrderParameters p = new JQuant.LMTOrderParameters();
			p.Security.IdNum = security;
			p.Price = price;
			p.Quantity = quantity;
			p.TransactionType = transaction;
			LimitOrder lo = new LimitOrder(p, callback);
			lo.Id = GenerateOrderId();

			return lo;
		}

		/// <summary>
		/// Place a system order. First checks for the possibility of immediate fill,
		/// if not possible - places it to the limit order book.
		/// Currently only limit ordres are supported (suitable for maof options).
		/// In the future child class MarketSimulationMaof will provide API which calls TaskBarLibSim
		/// </summary>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// Returns false on failure. Calling method will analyze errorCode to figure out 
		/// what went wrong with the order
		/// </returns>
		public bool PlaceOrder(ISystemLimitOrder systemOrder, out ReturnCode errorCode)
		{
			bool res = false;

			do
			{
				LimitOrder lo = (LimitOrder)systemOrder;
				object o;

				lock (securities)
				{
					o = securities[lo.SecurityId];
				}

				if (o == null)
				{
					errorCode = ReturnCode.UnknownSecurity;
					break;
				}

				FSM fsm = (FSM)o;
				OrderBook orderBook, orderBookImmediateFill;

				if (lo.TransactionType == JQuant.TransactionType.SELL)
				{
					orderBook = fsm.orderBookAsk; // sell order - book of asks
					orderBookImmediateFill = fsm.orderBookBid;
				}
				else
				{
					orderBook = fsm.orderBookBid;  // this is buy order - books of bids
					orderBookImmediateFill = fsm.orderBookAsk;
				}


				// maybe i can get fill immediately?
				bool haveFill = orderBookImmediateFill.CheckImmediateFill(lo);
				if (haveFill)
				{
					filledOrdersThread.Send(lo);
					ordersFilledCount++;
				}
				else  // add the order to the order book
				{
					orderBook.PlaceOrder(lo);
					ordersPlacedCount++;
				}

				res = true;
				errorCode = ReturnCode.NoError;
			}
			while (false);

			return res;
		}

		public OrderPair[] GetOrderQueue(int securityId, JQuant.TransactionType transType)
		{
			OrderPair[] op = null;
			MarketData md;

			object o;
			lock (securities)
			{
				o = securities[securityId];
			}

			if (o != null)
			{
				FSM fsm = (FSM)o;

				if (transType == JQuant.TransactionType.SELL)
				{
					md = fsm.orderBookAsk.GetMarketData();
					if (md != null)
						op = md.ask;
				}
				else
				{
					md = fsm.orderBookBid.GetMarketData();
					if (md != null)
						op = md.bid;
				}
			}


			return op;
		}

		/// <summary>
		/// Small thread which calls application's callbacks
		/// every time an order filled MarketSimulation.Core sends a message to the thread. Thread
		/// wakes up and calls application callback - informs the application that the order got fill
		/// The idea is to execute the application hook from a separate (different priority) context.
		/// </summary>
		protected class FilledOrdersThread : JQuant.MailboxThread<LimitOrder>
		{
			public FilledOrdersThread()
				: base("msFldOrdr", 100)
			{
			}

			protected override void HandleMessage(LimitOrder lo)
			{
				lo.callback(ReturnCode.Fill, lo, lo.Quantity);
			}

		}

		/// <summary>
		/// Small thread which notifies application when a change for security added to the
		/// watch list. Mainly debug 
		/// </summary>
		protected class WatchThread : JQuant.MailboxThread<FSM>
		{
			public WatchThread()
				: base("msWatch", 100)
			{
			}

			protected override void HandleMessage(FSM fsm)
			{
				fsm.watchlistCallback(fsm.marketData);
			}
		}

		/// <summary>
		/// Called from CLI to display counters and debug info.
		/// Returns the current state of the limit order book.
		/// </summary>
		public JQuant.IResourceStatistics GetOrderBook(int securityId, JQuant.TransactionType transaction)
		{
			JQuant.IResourceStatistics ob = null;

			do
			{
				object o;

				lock (securities)
				{
					// hopefully Item() will return null if there is no key in the hashtable
					o = securities[securityId];
				}

				if (o == null)
				{
					break;
				}

				FSM fsm = (FSM)o;
				if (transaction == JQuant.TransactionType.SELL)
					ob = fsm.orderBookAsk;
				else
					ob = fsm.orderBookBid;
			}
			while (false);

			return (ob);
		}

		/// <summary>
		/// Called from CLI to display counters and debug info
		/// and returns order queues statistics.
		/// There is a different method which returns current situation in the bid/ask queues
		/// and a another method which returns the most recent log entry.
		/// </summary>
		public JQuant.IResourceStatistics[] GetOrderQueues(int securityId, JQuant.TransactionType transaction)
		{
			JQuant.IResourceStatistics[] oqs = null;

			do
			{
				// GetOrderBook will take care of exclusive locks
				OrderBook ob = (OrderBook)GetOrderBook(securityId, transaction);

				if (ob == null)
				{
					System.Console.WriteLine("Order book for security " + securityId + " not found");
					break;
				}

				oqs = (JQuant.IResourceStatistics[])(ob.GetQueues());
			}
			while (false);

			return (oqs);
		}

		public bool GetEnableTrace(int securityId)
		{
			bool res = false;
			object fsm = securities[securityId];

			if (fsm != null)
			{
				res = (enableTrace[((FSM)fsm).marketData.id] != null);
			}

			return res;
		}

		/// <summary>
		/// Add security to the watch list 
		/// </summary>
		/// <param name="securityId">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <param name="callback">
		/// A <see cref="MarketSimulation.WatchlistCallback"/>
		/// MarketSimulation will call the application hook asynchronously when any update
		/// in the security state
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// Return true id success
		/// </returns>
		public bool AddWatch(int securityId, MarketSimulation.Core.WatchlistCallback callback)
		{
			bool res = false;
			object o = securities[securityId];

			if (o != null)
			{
				FSM fsm = (FSM)o;
				fsm.watchlistCallback = callback;
				fsm.watchEnable = true;
				res = true;
			}

			return res;
		}

		/// <summary>
		/// Remove security from the watch list. Because callbacks are asynchronous and done fom a
		/// separate thread application can get one or more callback even after call to RemoveWatch()
		/// </summary>
		public bool RemoveWatch(int securityId)
		{
			bool res = false;
			object o = securities[securityId];

			if (o != null)
			{
				FSM fsm = (FSM)o;
				// i do not set fsm.watchlistCallback to null because a separate thread can try to call
				// the method. This is up to application to remember that RemoveWatch is not an immediate
				// operation
				fsm.watchEnable = false;
				res = true;
			}

			return res;
		}

		/// <summary>
		/// Return list of securities added to watch
		/// </summary>
		public int[] WatchList()
		{
			System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>(securities.Count);
			System.Collections.ICollection values;
			lock (securities)
			{
				values = securities.Values;
			}
			foreach (object o in values)
			{
				FSM fsm = (FSM)o;
				if (fsm.watchEnable)
				{
					list.Add(fsm.marketData.id);
				}
			}

			int[] res = list.ToArray();
			return res;
		}

		public void EnableTrace(int securityId)
		{
			EnableTrace(securityId, true);
		}

		public void EnableTrace(int securityId, bool enable)
		{
			do
			{
				// GetOrderBook will take care of exclusive locks
				OrderBook obBuy = (OrderBook)GetOrderBook(securityId, JQuant.TransactionType.BUY);
				OrderBook obSell = (OrderBook)GetOrderBook(securityId, JQuant.TransactionType.SELL);

				if (obBuy != null)
				{
					obBuy.EnableTrace(enable);
				}
				if (obSell != null)
				{
					obSell.EnableTrace(enable);
				}

				enableTrace.Remove(securityId);
				// add flag to the hashtable. When i create a book i will see the trace flag
				if (enable)
				{
					enableTrace.Add(securityId, true);
				}

			}
			while (false);
		}

		/// <summary>
		/// Returns list of securities.
		/// </summary>
		public int[] GetSecurities()
		{
			int[] ids;

			// i do not know where the hashtable is getting copied
			// just in case i lock everything
			lock (securities)
			{
				System.Collections.ICollection keys = securities.Keys;

				int size = keys.Count;

				ids = new int[size];

				keys.CopyTo(ids, 0);
			}

			return ids;
		}

		public MarketData GetSecurity(int securityId)
		{
			object o;

			lock (securities)
			{
				o = securities[securityId];
			}

			FSM fsm = default(FSM);
			MarketData md = default(MarketData);

			if (o != null)
			{
				fsm = (FSM)o;
				md = fsm.marketData;
			}


			return md;
		}

		/// <summary>
		/// Collection of all traded symbols (different BNO_Num for TASE). 
		/// I keep objects of type FSM in the hashtable.
		/// </summary>
		protected System.Collections.Hashtable securities;
		protected FilledOrdersThread filledOrdersThread;
		protected WatchThread watchThread;

		/// <summary>
		/// total number of lines (events) read from the historical log
		/// </summary>
		protected int eventsCount;
		/// <summary>
		/// number of system order placed
		/// </summary>
		protected int ordersPlacedCount;
		protected int ordersFilledCount;
		protected int ordersCanceledCount;

		/// <summary>
		/// Keep debug trace enable flags  
		/// </summary>
		protected System.Collections.Hashtable enableTrace;
	}

}
