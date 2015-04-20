
using System;
using System.Collections.Generic;
using JQuant;
using System.Text;
using System.Xml;
using System.Reflection;
using System.ComponentModel;

using FMRShell;

// I need definition of K300RzfType.
#if USEFMRSIM
using TaskBarLibSim;
#else
using TaskBarLib;
#endif

namespace Algo
{
	/// <summary>
	/// List of events possible in the semiautomatic state machine. 
	/// This is system level state machine which takes care of looking 
	/// for the trigger and accepting orders from the user.
	/// Supports both semiautomatic and fully automatic modes.
	/// </summary>
	internal enum Event
	{
		[Description("Start/resume the algo")]
		Start,
		[Description("Stop/pause the algo")]
		Stop,
		[Description("New market data")]
		DataUpdate,
		[Description("Ignore trigger")]
		Skip,
		[Description("Real cash trade")]
		RealTrade,
		[Description("Paper trade")]
		PaperTrade,
		[Description("Fatal error reported by the OrderFSM")]
		OrderError,
		[Description("Partial order fill")]
		PartialFill,
		[Description("Complete Order fill")]
		Fill,
		[Description("Order placed on the limit book, waiting for fill")]
		PlacedLimitBook,
		[Description("Trigger disappeared")]
		TriggerCanceled,
		[Description("Order FSM notified about canceled order")]
		OrderCanceled,
		[Description("Timer expired")]
		TimerReactionExpired
	}


	/// <summary>
	/// Objects of this type are used internally by the Algo for sending messages to the Algo FSM 
	/// </summary>
	internal class MbxMessage
	{
		public MbxMessage(Event msgEvent, object msgData)
		{
			this.msgEvent = msgEvent;
			this.msgData = msgData;
		}

		public MbxMessage(Event msgEvent, object msgData, object msgData1)
		{
			this.msgEvent = msgEvent;
			this.msgData = msgData;
			this.msgData1 = msgData1;
		}

		public Event msgEvent;
		public object msgData;
		public object msgData1;
	}


	/// <summary>
	/// This class implements the Algo. 
	/// Algo.Base keeps a single object of this type, 
	/// and serves an external shell to it.
	/// </summary>
	internal class AlgoFSM : MailboxThread<MbxMessage>, IResourceStatistics
	{
		/// <summary>
		/// The state of the FSM (kept inside an AlgoStock object).
		/// 
		/// Notes:
		///	1.	'WaitingOperatorReset' state is active when a trigger disappeared before 
		///		the operator responded in WaitingOperator state. 
		///		Any additional triggers will be ignored until the operator hits 'Skip' command.
		///		
		///	2.	'PaperOpened'/'PaperClosed' states support paper trades.
		///		The state is assigned to the fsm along with sending its matching event (such as 'Fill').
		///		In any of these states no further paper trading tests are performed.
		///		The fsm remains in these states until the event associated with transition to one of these states
		///		arrives and is processed, causing transition to other state.
		///		Example:
		///		Fill event in 'PaperClosed' state resets the trigger and moves the FSM back to 'WaitingTrigger' state.
		/// </summary>
		public enum State
		{
			[Description("Idle")]
			Idle,
			[Description("Paused by operator")]
			Paused,
			[Description("Wait for the instructions from the operator")]
			WaitingOperator,
			[Description("Wait for the operator to reset")]
			WaitingOperatorReset,
			[Description("Look for trigger")]
			WaitingTrigger,
			[Description("Pending open position")]
			OpeningPosition,
			[Description("Pending closed position")]
			ClosingPosition,
			[Description("Paper trade opened - intermediate state")]
			PaperOpened,
			[Description("Paper trade closed - intermediate state")]
			PaperClosed,
		}

		/// <summary>
		/// Describes roundtrip trigger.
		/// Holds the data and methods needed for risk and profitabilty computations.
		/// </summary>
		protected class Trigger
		{
			//TODO: move it into XML containing trading parameters.
			const double COMMISSION_RATE = 0.001;

			public Trigger()
			{
				Size = 0;
				EnterPrice = 0.0;
				ExitPrice = 0.0;
				SL = 0.0;
			}

			/// <summary>
			/// Is this trigger on bid or ask side of the book?
			/// </summary>
			public bool Bid
			{
				get;
				set;
			}

			public int Size
			{
				get;
				set;
			}

			public double EnterPrice
			{
				get;
				set;
			}

			public double ExitPrice
			{
				get;
				set;
			}

			/// <summary>
			/// Stop loss can happen at several price levels
			/// so no single price is defined here, but rather
			/// the total amount of NIS.
			/// </summary>
			public double SL
			{
				get;
				set;
			}

			public double EnterNIS
			{
				get
				{
					return Math.Round(Size * EnterPrice / 100, 2);
				}
			}

			public double ExitNIS
			{
				get
				{
					return Math.Round(Size * ExitPrice / 100, 2);
				}
			}

			public double PnL_NIS
			{
				get
				{
					double result;
					if (Bid)
						result = Math.Round(Size * (EnterPrice - ExitPrice) / 100, 2);
					else
						result = Math.Round(Size * (ExitPrice - EnterPrice) / 100, 2);
					return result;
				}
			}

			public double PnLPct
			{
				get
				{
					return PnL_NIS / EnterNIS;
				}
			}

			public double SL_NIS
			{
				get
				{
					return Math.Abs(EnterNIS - SL);
				}
			}

			public double Fees
			{
				get
				{
					return Math.Round(COMMISSION_RATE * (EnterNIS + ExitNIS), 2);
				}
			}

			public double PnL_Fees
			{
				get
				{
					return PnL_NIS - Fees;
				}
			}

			public double SL_Pct
			{
				get
				{
					return SL_NIS / EnterNIS;
				}
			}

			/// <summary>
			/// print the trigger
			/// </summary>
			/// <returns></returns>
			public override string ToString()
			{
				string result = Environment.NewLine +
					"\t\tSIZE \t\t" + "PRICE \t\t" + "NIS \t\t" + "PCT" + Environment.NewLine +
					"------------------------------------------------------------------------" + Environment.NewLine +
					"ENTER \t\t" + Size + "\t\t" + EnterPrice + "\t\t" + EnterNIS + Environment.NewLine +
					"EXIT \t\t\t\t" + ExitPrice + "\t\t" + ExitNIS + Environment.NewLine +
					"STOP \t\t\t\t\t\t" + SL + Environment.NewLine + Environment.NewLine +
					"------------------------------------------------------------------------" + Environment.NewLine +
					Environment.NewLine +
					"\t\t\tSL \t\t\t" + "-" + SL_NIS + "\t\t" + SL_Pct.ToString(".000 %") + Environment.NewLine +
					"\t\t\tPnL \t\t\t" + PnL_NIS + "\t\t" + PnLPct.ToString(".000 %") + Environment.NewLine +
					"\t\t\tFees \t\t\t" + "-" + Fees + "\t\t" + Environment.NewLine +
					"\t\t\tPnL-Fees \t\t" + PnL_Fees + "\t\t" + Environment.NewLine +
					"------------------------------------------------------------------------" + Environment.NewLine;
				return result;
			}
		}//class Trigger


		/// <summary>
		/// I need this type to fast copy from the AlgoStock object
		/// the data required for log. String operations are time consuming
		/// and they will be done in the context of a separate thread (logger)
		/// </summary>
		protected class AlgoLoggerData : ILoggerData
		{
			/// <summary>
			/// constructor - copy the data from the AlgoStock object.
			/// </summary>
			/// <param name="stock">the stock to copy from</param>
			public AlgoLoggerData(AlgoStock stock)
			{
				lob = stock.latestTicks[stock.latestTicksCounter - 1];
				IdNum = stock.IdNum;
				Name = stock.Name;
				state = stock.state;
				lastEvent = stock.lastEvent;
				timeTrigger = stock.timeTrigger;
				_trigger = stock._trigger;
			}

			/// <summary>
			/// implement ILoggerData
			/// </summary>
			/// <returns>a<see cref="string"/>log string</returns>
			public string ToLogString()
			{
				StringBuilder sb = new StringBuilder(250);

				sb.Append(lob.timestamp.ToString("hh:mm:ss.fff"));
				sb.Append(",");
				sb.Append(lob.tick);
				sb.Append(",");
				sb.Append(this.IdNum);
				sb.Append(",");
				sb.Append(this.Name);
				sb.Append(",");
				sb.Append(lob.bid[2].size);
				sb.Append(",");
				sb.Append(lob.bid[1].size);
				sb.Append(",");
				sb.Append(lob.bid[0].size);
				sb.Append(",");
				sb.Append(lob.bid[2].price);
				sb.Append(",");
				sb.Append(lob.bid[1].price);
				sb.Append(",");
				sb.Append(lob.bid[0].price);
				sb.Append(",");
				sb.Append(lob.ask[0].price);
				sb.Append(",");
				sb.Append(lob.ask[1].price);
				sb.Append(",");
				sb.Append(lob.ask[2].price);
				sb.Append(",");
				sb.Append(lob.ask[0].size);
				sb.Append(",");
				sb.Append(lob.ask[1].size);
				sb.Append(",");
				sb.Append(lob.ask[2].size);
				sb.Append(",");
				sb.Append(this.state);
				sb.Append(",");
				sb.Append(this.lastEvent);
				sb.Append(",");
				sb.Append(this.timeTrigger.ToString("hh:mm:ss.fff"));
				sb.Append(",");
				sb.Append(_trigger.Size);
				sb.Append(",");
				sb.Append(_trigger.EnterPrice);
				sb.Append(",");
				sb.Append(_trigger.ExitPrice);
				sb.Append(",");
				sb.Append(_trigger.PnL_NIS);
				sb.Append(",");
				sb.Append(_trigger.PnLPct);
				sb.Append(",");
				sb.Append(_trigger.SL_NIS);
				sb.Append(",");
				sb.Append(_trigger.SL_Pct);

				return sb.ToString();
			}

			int IdNum;
			string Name;
			LimitOrderBook lob;
			State state;
			Event lastEvent;
			DateTime timeTrigger;
			Trigger _trigger;
		}//class AlgoLoggerData


		/// <summary>
		/// receives data from AlgoLoggerData object and writes it to file log
		/// </summary>
		protected class AlgoLogger : FileLogger<AlgoLoggerData>
		{
			public AlgoLogger(string name, string filename, bool append, string legend)
				: base(name, filename, append, legend)
			{
			}

			public void AddEntry(AlgoStock stock)
			{
				// create logger data and copy all required fields into it
				AlgoLoggerData loggerData = new AlgoLoggerData(stock);
				base.AddEntry(loggerData);
			}
		}


		/// <summary>
		/// Data holder. Contains all information required by Algo FSM to handle the incoming 
		/// events and generate orders. There is no other algo-realted information besides the
		/// information stored in this object.
		/// Algo works with stocks, so it inherits from Stock rather than JQuant.Security
		/// Objects of this type are stored in a hash table within AlgoFSM class.
		/// </summary>
		protected class AlgoStock : JQuant.Stock
		{
			/// <summary>
			/// Constructor. Called once for each stock, only upon arrival of
			/// market (limit book) data with uncommon id.
			/// </summary>
			/// <param name="initValue">a<see cref="LimitOrderBook"/>market data</param>
			public AlgoStock(LimitOrderBook initValue)
				: base()
			{
				// market data
				latestTicks[0] = initValue;
				latestTicksCounter = 1;

				// unique TASE id
				IdNum = initValue.id;
				// stock's name. Sometimes comes broken with extra blanks, so cleanup.
				Name = JQuant.OutputUtils.CleanBlanks(initValue.name);

				// Minimal lot size. 
				// For TA75 stocks it's 2000 NIS, which I set to be defaults here; for TA25 it's 5000 NIS.
				MIN_LOT_NIS = 2000.0;

				//initilaize limit orders data
				orderOpen = new LMTOrderParameters();
				orderOpen.Security = this;

				orderClose = new LMTOrderParameters();
				orderClose.Security = this;

				_trigger = new Trigger();

				// by default all the trades are paper
				// change this flag from outside if we want to do real trade
				IsRealTrade = false;

				// reference to the order FSM order object associated with the current 
				// active order, set by the order FSM
				_orderFSM_Cookie = new object();

				// Idle state is an entry point to the FSM.
				// this is the only place outside the FSM where the state is changed.
				state = AlgoFSM.State.Idle;
			}

			/// <summary>
			/// new data record arrived? store it for furher processing
			/// </summary>
			/// <param name="data"></param>
			public void UpdateMarketData(LimitOrderBook data)
			{
				//if the history data storage is full, move all the records forward, discarding the oldest one
				if (latestTicksCounter >= HISTORY_SIZE)
				{
					for (int i = 0; i < (latestTicksCounter - 1); i++)
					{
						latestTicks[i] = latestTicks[i + 1];
					}
				}

				// if history is not filled with data to the required depth,
				// just add the newest record to the end and 
				// keep all the rest where they are, so i just advance the counter
				else
				{
					latestTicksCounter++;
				}

				// in any case keep the recent record the last
				latestTicks[latestTicksCounter - 1] = data;

				// in case there is a new transaction, update the last transaction timestamp
				if (latestTicks.Length > 1)
				{
					// new transaction indicated? - update its time
					if (this.NewTransaction)
					{
						latestTicks[latestTicksCounter - 1].lastTradeTime = data.timestamp;
					}
					// else keep last transaction's time from the previous record
					else
					{
						latestTicks[latestTicksCounter - 1].lastTradeTime = latestTicks[latestTicksCounter - 2].lastTradeTime;
					}
				}
			}

			/// <summary>
			/// True if a new transaction took place
			/// </summary>
			public bool NewTransaction
			{
				get
				{
					return (latestTicks[latestTicksCounter - 1].dayVolume > latestTicks[latestTicksCounter - 2].dayVolume);
				}
			}

			/// <summary>
			/// Display the trigger in the terminal.
			/// </summary>
			/// <param name="buySignal"></param>
			public void PrintTriggerCondition(bool buySignal)
			{
				if (!Resources.isUnix)
					Console.ForegroundColor = ConsoleColor.Cyan;

				if (buySignal)
					System.Console.Write(Environment.NewLine + "BID condition");
				else
					System.Console.Write(Environment.NewLine + "ASK condition ");

				//print the current limit book
				Console.WriteLine(this.ToString());
				//print the trigger
				Console.WriteLine(_trigger.ToString());

				if (!Resources.isUnix)
					Console.ForegroundColor = ConsoleColor.Green;
			}

			/// <summary>
			/// Displays the stock as a snapshot of its most recent limit book.
			/// </summary>
			/// <returns></returns>
			public override string ToString()
			{
				return Environment.NewLine + this.latestTicks[latestTicksCounter - 1].ToString();
			}

			/// <summary>
			/// Display the last trade
			/// </summary>
			public void PrintLastTrade()
			{
				LimitOrderBook lob = latestTicks[latestTicksCounter - 1];
				Console.WriteLine(lob.lastTradeTime.ToString("hh:mm:ss.fff") + "\t\t" +
					lob.lastTradeSize.ToString("###,###,###") + "\t@\t" + lob.lastTradePrice.ToString("###,###,###.##") +
					"\t\t" + IdNum + "\t\t" + Name);
			}

			/// <summary>
			/// Resets trigger-related members.
			/// Call this method when a trigger is no longer active
			/// (roundtrip is complete or the trigger disappeared before placing a trade)
			/// </summary>
			public void TriggerCleanup()
			{
				orderOpen = new LMTOrderParameters();
				orderOpen.Security = this;

				orderClose = new LMTOrderParameters();
				orderClose.Security = this;

				trigger_lmt2 = default(double);
				timeTrigger = default(DateTime);
				_trigger = new Trigger();
			}

			// i am going to keep 3 latest records
			public const int HISTORY_SIZE = 3;

			public double MIN_LOT_NIS
			{
				get;
				set;
			}

			/// <summary>
			/// Keeps a number of latest data ticks (say, 3)
			/// </summary>
			public LimitOrderBook[] latestTicks = new LimitOrderBook[HISTORY_SIZE];

			// in case of trigger activation, I need to keep the details of 2 orders:
			//     1. enter (open) the position 
			//     2. close the position
			// the FSM will initialize and update them as needed
			//
			// note: stop loss will update the existing close order, 
			// so there is no separate order for the stop
			public LMTOrderParameters orderOpen;
			public LMTOrderParameters orderClose;

			/// <summary>
			/// Trigger object contains trigger-related data and computations
			/// </summary>
			public Trigger _trigger;

			/// <summary>
			/// a flag, indicating whether the current trade is real cash or paper trade
			/// </summary>
			public bool IsRealTrade;

			/// <summary>
			/// Reference to the order FSM order object associated 
			/// with the current trade. Manipulated by the order FSM.
			/// </summary>
			public object _orderFSM_Cookie;


			// Data for paper trading tests

			// Time of the trigger
			// in the RT trading can be used as an alternative to timer
			// i.g., I can compare it to the current timestamp
			public DateTime timeTrigger;

			// 2nd best limit at the time of the trigger
			// either bid or ask - depending on the trigger's side
			public double trigger_lmt2;


			public int latestTicksCounter;
			public AlgoFSM.State state;
			public Base.Mode mode;

			//keep latest event for logging purposes
			public Event lastEvent;

		}//class AlgoStock


		/// <summary>
		/// summary statistics for the FSM
		/// </summary>
		protected class Statistics
		{
			//public string timestamp;
			public int dataUpdates;
			public int triggersTotal;
			public int triggersHandled;
			public int triggersIgnoredSA;
			public int skippedTriggers;
			public int paperTrades;
			public int realTrades;
			public int openedPositions;
			public int completePositions;
			public int stopLosses;
			public int missedTriggers;
			public int eventsInPaused;
			public double PnL;
			public double fees;
			public double PnL_fees;

			public Statistics()
			{
				FieldInfo[] fields = this.GetType().GetFields();
				int initValue = 0;
				foreach (FieldInfo fi in fields)
				{
					fi.SetValue(this, initValue);
				}
			}
		}//class Statistics

		/// <summary>
		/// The legend string to be written on top of the AlgoFSM log
		/// </summary>
		/// <returns></returns>
		public static string LogLegend()
		{
			StringBuilder sb = new StringBuilder(250);

			sb.Append("Timestamp,");
			sb.Append("Ticks,");
			sb.Append("StockId,");
			sb.Append("Name,");
			sb.Append("Bid3Q,");
			sb.Append("Bid2Q,");
			sb.Append("Bid1Q,");
			sb.Append("Bid3P,");
			sb.Append("Bid2P,");
			sb.Append("Bid1P,");
			sb.Append("Ask1P,");
			sb.Append("Ask2P,");
			sb.Append("Ask3P,");
			sb.Append("Ask1Q,");
			sb.Append("Ask2Q,");
			sb.Append("Ask3Q,");
			sb.Append("State,");
			sb.Append("Event,");
			sb.Append("TrigTime,");
			sb.Append("TrigSize,");
			sb.Append("EnterP,");
			sb.Append("ExitP,");
			sb.Append("PnL_NIS,");
			sb.Append("PnLPct,");
			sb.Append("SL_NIS,");
			sb.Append("SL_Pct");

			return sb.ToString();
		}

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="orderFsm">orders processor</param>
		public AlgoFSM(FMRShell.RezefOrderFSM orderFsm)
			: base("Algo", 500)
		{
			TA25List = new List<int>();

			//TODO: this is a patch. The data should be taken from an XML file.
			//Note:	the list is as of June 2010. 
			//		The TA25 index composition changes twice a year
			//		- Jan 01 and Jul 01
			TA25List.Add(126011);
			TA25List.Add(230011);
			TA25List.Add(260018);
			TA25List.Add(268011);
			TA25List.Add(273011);
			TA25List.Add(281014);
			TA25List.Add(304014);
			TA25List.Add(475020);
			TA25List.Add(576017);
			TA25List.Add(585018);
			TA25List.Add(604611);
			TA25List.Add(629014);
			TA25List.Add(639013);
			TA25List.Add(662577);
			TA25List.Add(691212);
			TA25List.Add(695437);
			TA25List.Add(746016);
			TA25List.Add(1081124);
			TA25List.Add(1081819);
			TA25List.Add(1083484);
			TA25List.Add(1084128);
			TA25List.Add(1092428);
			TA25List.Add(1100007);
			TA25List.Add(1101534);
			TA25List.Add(2590248);


			//set the reference to the orders processor
			_orderFSM = orderFsm;
			secList = new Dictionary<int, AlgoStock>();
			statistics = new Statistics();

			//timer task
			timerTask = new TimerTask("TimerAlgoFSM");
			reactionTimers = new TimerList("ReactionTimer", MIN_REACTION_TIME, 20, ReactionTimerCallback, timerTask);
			timerTask.Start();

			//logger
			//name the log file
			string filename = Resources.CreateLogFileName("AlgoLog_", LogType.CSV);
			// create logger
			logger = new AlgoLogger("AlgoLogger", filename, true, LogLegend());
			//and start it
			logger.Start();
		}

		/// <summary>
		/// Call this method to release associated resources 
		/// </summary>
		public override void Dispose()
		{
			//timers
			reactionTimers.Dispose();
			timerTask.Stop();
			timerTask.Dispose();

			// references
			this._base = default(Algo.Base);
			this.secList = default(Dictionary<int, AlgoStock>);
			this.statistics = default(Statistics);
			this._triggeredStock = null;

			// logger
			this.logger.Stop();
			this.logger.Dispose();

			// stop MailboxThread
			this.Stop();
			// finaly dispose this
			base.Dispose();
		}

		private void ReactionTimerCallback(ITimer timer)
		{
			AlgoStock stock = (AlgoStock)timer.ApplicationHookGet();
			MbxMessage msg = new MbxMessage(Event.TimerReactionExpired, stock);
			this.Send(msg);
		}

		/// <summary>
		/// Interface JQuant.IResourceStatistics requires this method to be implemented
		/// Return all debug counters
		/// </summary>
		public void GetEventCounters(out System.Collections.ArrayList names, out System.Collections.ArrayList values)
		{
			names = new System.Collections.ArrayList(12);
			values = new System.Collections.ArrayList(12);

			FieldInfo[] fields = statistics.GetType().GetFields();

			// set all fields in the new data object
			foreach (FieldInfo fi in fields)
			{
				names.Add(fi.Name);
				values.Add(fi.GetValue(statistics));
			}
		}

		/// <summary>
		/// toggles FSM mode
		/// </summary>
		/// <param name="mode">Either automatic or semi automatic</param>
		public void SetMode(Base.Mode mode)
		{
			// set Mode in the FSM
			this.Mode = mode;
		}


		#region FSM Message Handlers;
		/// <summary>
		/// Switch first by message event type, call the even handler where I switch by state.
		/// Supports both the semiautomatic and fully automatic modes.
		/// </summary>
		protected override void HandleMessage(MbxMessage msg)
		{
			Event e = msg.msgEvent;

			// There are very many data updates and transactions, I print them only for the triggered stock
			if (e != Event.DataUpdate)
				Console.WriteLine(msg.msgEvent);

			// Ignore all incoming events if the FSM is paused.
			// TODO: upadting market data from the stream can be critical, because
			// a gap in it can break the FSM logic. Consider keeping updating the data.
			if (_paused)
			{
				statistics.eventsInPaused++;
				return;
			}

			switch (e)
			{
				case Event.Start:
					HandleMessage_Start();
					break;
				case Event.Stop:
					HandleMessage_Stop();
					break;
				case Event.DataUpdate:
					HandleMessage_DataUpdate(msg);
					break;
				case Event.Skip:
					HandleMessage_Skip(msg);
					break;
				case Event.PaperTrade:
				case Event.RealTrade:
					HandleMessage_Trade(msg);
					break;
				case Event.TriggerCanceled:
					HandleMessage_TriggerCanceled(msg);
					break;
				case Event.PlacedLimitBook:
					HandleMessage_PlacedLimitBook(msg);
					break;
				case Event.PartialFill:
					HandleMessage_PartialFill(msg);
					break;
				case Event.Fill:
					HandleMessage_Fill(msg);
					break;
				case Event.OrderError:
					HandleMessage_Error(msg);
					break;
				case Event.OrderCanceled:
					HandleMessage_OrderCanceled(msg);
					break;
				case Event.TimerReactionExpired:
					HandleMessageTimerReactionExpired(msg);
					break;
				default:
					System.Console.WriteLine("Unhandled Algo event " + msg.msgEvent);
					break;
			}
		}

		/// <summary>
		/// In the semiautomatic trading I have at most one pending trade at any time.
		/// If there is an active trade in the system - ignore any new triggers, 
		/// but continue test and log new triggers for other securities.
		/// </summary>
		/// <param name="msg"></param>
		protected void HandleMessage_DataUpdate(MbxMessage msg)
		{
			//parse the message data - I expect to get a new limit book update
			LimitOrderBook data = (LimitOrderBook)msg.msgData;

			statistics.dataUpdates++;
			//statistics.timestamp = data.timestamp.ToString("hh:mm:ss.fff");

			//and update the stock with the new data
			AlgoStock stock = UpdateDictionary(data);
			stock.lastEvent = msg.msgEvent;

			//process the event
			switch (stock.state)
			{
				//no trigger in the system, or this is not the triggered stock
				case State.WaitingTrigger:
					if (this.Mode == Base.Mode.Automatic || this._triggeredStock == null)
						HandleDataUpdateWaiting(stock);	// no trigger or automatic mode
					else
						HandleDataUpdateTrigger(stock);	// trigger in sa mode, but not this stock
					break;
				// any other state means we're in the triggered stock
				case State.ClosingPosition:
					HandleDataUpdateClosingPosition(stock);
					break;
				case State.OpeningPosition:
					HandleDataUpdateOpeningPosition(stock);
					break;
				case State.WaitingOperator:
					HandleDataUpdateWaitingOperator(stock);
					break;
				case State.WaitingOperatorReset:
					//log
					logger.AddEntry(stock);
					//TODO - add here some logic
					Console.WriteLine("Trigger canceled, type 's' to reset the FSM.");
					break;
				case State.PaperOpened:
				case State.PaperClosed:
					logger.AddEntry(stock);
					//do nothing special - waiting for the paper message to be processed soon
					Console.WriteLine("Data update in " + stock.state + " state");
					break;
				default:
					HandleMessage_Default(msg, stock.state);
					break;
			}
		}

		protected void HandleDataUpdateOpeningPosition(AlgoStock stock)
		{
			//log
			logger.AddEntry(stock);

			MbxMessage message;

			//test if the trigger that caused trading is still active
			//if not - send message to the FSM
			if (!IsTriggerStillActive(stock))
			{
				//create and send message to the FSM
				message = new MbxMessage(Event.TriggerCanceled, stock);
				this.Send(message);
			}
		}

		protected void HandleDataUpdateWaitingOperator(AlgoStock stock)
		{
			//log
			logger.AddEntry(stock);
			//test if the trigger that caused trading is still active
			//if not - send message to the FSM
			if (!IsTriggerStillActive(stock))
			{
				//create and send message to the FSM
				MbxMessage message = new MbxMessage(Event.TriggerCanceled, stock);
				this.Send(message);
			}
		}

		protected void HandleDataUpdateClosingPosition(AlgoStock stock)
		{
			MbxMessage message;
			//log
			logger.AddEntry(stock);
			//if paper trade - test for paper fill
			if (!stock.IsRealTrade &&
				ClosePaperPositionIndication(stock))
			{
				stock.state = State.PaperClosed;
				message = new MbxMessage(Event.Fill, stock);
				this.Send(message);
			}
			else
			{
				//test the data for stop loss conditions, update limit price if necessary
				double newPrice;
				bool loss;
				if (StopLossIndication(stock, out newPrice, out loss))
				{
					stock.orderClose.Price = newPrice;
					stock._trigger.ExitPrice = newPrice;

					//real tarde - i work with LMT orders only - just update the price
					if (stock.IsRealTrade)
						_orderFSM.Update(newPrice, ref stock._orderFSM_Cookie);
					else
					{
						//papre trade - here I assume immediate fill at the stop price
						if (loss)
						{
							stock.state = State.PaperClosed;
							message = new MbxMessage(Event.Fill, stock);
							this.Send(message);
						}
					}

					if (loss)
					{
						statistics.stopLosses++;
						Console.WriteLine("Stop loss @ " + newPrice);
					}
					else
					{
						Console.WriteLine("Closing price updated to " + newPrice);
					}
				}
			}
		}

		/// <summary>
		/// Data update in the waiting for trigger state
		/// Constantly looks for tigger in the data
		/// </summary>
		protected void HandleDataUpdateWaiting(AlgoStock stock)
		{
			bool buySignal;
			bool trigger = TriggerCondition(stock, out buySignal);

			if (trigger)
			{
				//log only if trigger
				logger.AddEntry(stock);

				// bump counters
				JQuant.Resources.systemEventCounters.saTrading++;
				statistics.triggersHandled++;
				statistics.triggersTotal++;

				//message
				MbxMessage msg;

				//in SA mode:
				if (this.Mode == Base.Mode.Semiautomatic)
				{
					// store reference to the triggered stock 
					// I will need it when commands from human operator arrive
					// or as an indication that the system is triggered
					_triggeredStock = stock;

					// print the trigger for the operator
					stock.PrintTriggerCondition(buySignal);

					// change the state of the stock
					stock.state = State.WaitingOperator;
				}
				else
				{
					//TODO - here we need add logic that decides to launch Real or Paper trade or Skip
					if (stock.IsRealTrade)
						msg = new MbxMessage(Event.RealTrade, stock);
					else
						msg = new MbxMessage(Event.PaperTrade, stock);
					this.Send(msg);

					stock.state = State.OpeningPosition;
				}
			}
		}

		/// <summary>
		/// In SA mode with an active trigger in the system 
		/// I just check for presence of a new trigger in the data
		/// no trade is possible anyway
		/// </summary>
		/// <param name="stock"></param>
		protected void HandleDataUpdateTrigger(AlgoStock stock)
		{
			bool buySignal;
			bool trigger = TriggerCondition(stock, out buySignal);

			if (trigger)
			{
				//log
				logger.AddEntry(stock);

				// bump counters
				JQuant.Resources.systemEventCounters.saTrading++;
				statistics.triggersTotal++;
				statistics.triggersIgnoredSA++;

				// inform the operator
				Console.WriteLine("A trigger has been ignored by SA in triggered state for " + stock.Name);
			}
		}

		/// <summary>
		/// return to the waiting state
		/// </summary>
		protected void HandleMessage_Skip(MbxMessage msg)
		{
			//parse the message
			AlgoStock stock = (AlgoStock)msg.msgData;
			stock.lastEvent = msg.msgEvent;
			//log
			logger.AddEntry(stock);

			switch (stock.state)
			{
				case State.WaitingOperator:
				case State.WaitingOperatorReset:
					statistics.skippedTriggers++;
					//reset the FSM
					this.TriggerCleanup(stock);
					break;
				default:
					HandleMessage_Default(msg, stock.state);
					break;
			}
		}

		protected void HandleMessage_Trade(MbxMessage msg)
		{
			AlgoStock stock = (AlgoStock)msg.msgData;
			stock.lastEvent = msg.msgEvent;
			//log
			logger.AddEntry(stock);

			//Console.WriteLine("Paper trade in state " + stock.state);
			switch (stock.state)
			{
				case State.WaitingOperator:
				case State.OpeningPosition:
					if (stock.state == State.WaitingOperator)
					{
						stock.state = State.OpeningPosition;
					}

					if (msg.msgEvent == Event.RealTrade)
					{
						statistics.realTrades++;
						//place real trade
						stock.IsRealTrade = true;
						_orderFSM.Create(stock.orderOpen, _base.OrderFSM_EventNotifier, stock, out stock._orderFSM_Cookie);
					}
					else
					{
						statistics.paperTrades++;
						stock.IsRealTrade = false;
						//start timer
						ITimer timer;
						long timerId;
						reactionTimers.Start(out timer, out timerId, stock, false);
					}
					break;
				case State.WaitingOperatorReset:
					Console.WriteLine("Trigger canceled, can't place " + msg.msgEvent + " in state " + stock.state);
					break;
				default:
					HandleMessage_Default(msg, stock.state);
					break;
			}
		}

		private void HandleMessage_TriggerCanceled(MbxMessage msg)
		{
			AlgoStock stock = (AlgoStock)msg.msgData;
			stock.lastEvent = msg.msgEvent;
			//log
			logger.AddEntry(stock);

			switch (stock.state)
			{
				case State.WaitingOperator:
					//we can get here only in semiautomatic mode, no additional mode checks
					stock.state = State.WaitingOperatorReset;
					break;
				case State.OpeningPosition:
					//if real trade - cancel the pending order
					if (stock.IsRealTrade)
						_orderFSM.Cancel(ref stock._orderFSM_Cookie);

					//if paper trade - send message to the fsm informing that the order hase been canceled
					else
					{
						stock.state = State.PaperClosed;
						MbxMessage message = new MbxMessage(Event.OrderCanceled, stock);
						this.Send(message);
					}
					//in any case no change of the state yet - wait for OrderCanceled notification to arrive
					break;
				case State.ClosingPosition:
				case State.PaperOpened:
					break;
				case State.WaitingTrigger:
					Console.WriteLine("The cancel was already processed for " + stock.Name);
					break;
				default:
					HandleMessage_Default(msg, stock.state);
					break;
			}
		}

		private void HandleMessage_PlacedLimitBook(MbxMessage msg)
		{
			AlgoStock stock = (AlgoStock)msg.msgData;
			stock.lastEvent = msg.msgEvent;
			//log
			logger.AddEntry(stock);


			switch (stock.state)
			{
				case State.OpeningPosition:
					//if real trade - send cancel request to the order FSM
					if (stock.IsRealTrade)
						_orderFSM.Cancel(ref stock._orderFSM_Cookie);
					else
					{
						//if paper trade - send message to the fsm informing that the order hase been canceled
						MbxMessage message = new MbxMessage(Event.OrderCanceled, stock);
						this.Send(message);
					}
					break;
				case State.ClosingPosition:
					//do nothing - it's okay since we wait for exit with LMT order
					break;
				default:
					HandleMessage_Default(msg, stock.state);
					break;
			}
		}

		private void HandleMessage_PartialFill(MbxMessage msg)
		{
			AlgoStock stock = (AlgoStock)msg.msgData;
			stock.lastEvent = msg.msgEvent;
			//log
			logger.AddEntry(stock);

			int FilledQuantity = (int)msg.msgData1;

			int RemainingQuantity = stock.orderClose.Quantity;

			switch (stock.state)
			{
				case State.OpeningPosition:
					// TODO here we need more logic. 3 possibilities:
					// if there is at least minimal lot:
					//		1. cancel the remainder and go for exit
					// if there is less less than minimal lot:
					//		2. wait for complete fill
					//		3. complete the order to the minimal lot, then go for exit
					//
					// which case to take depends on the PnL in each possibility
					// we will choose the highest expected value
					break;
				case State.ClosingPosition:
					// update the filled quantity, continue waiting for completion
					RemainingQuantity = RemainingQuantity - FilledQuantity;
					// the trade is completed
					// TODO: check with the FMR's helpdesk whether the last portion reports 
					// partial fill or complete fill?
					if (RemainingQuantity <= 0)
					{
						MbxMessage message = new MbxMessage(Event.Fill, stock);
						this.Send(message);
					}

					stock.orderClose.Quantity = RemainingQuantity;

					break;
				default:
					HandleMessage_Default(msg, stock.state);
					break;
			}
		}

		private void HandleMessage_Fill(MbxMessage msg)
		{
			AlgoStock stock = (AlgoStock)msg.msgData;
			stock.lastEvent = msg.msgEvent;
			//log
			logger.AddEntry(stock);

			switch (stock.state)
			{
				case State.PaperOpened:
				case State.OpeningPosition:
					//send request to close the trade
					if (stock.IsRealTrade)
						_orderFSM.Create(stock.orderClose, _base.OrderFSM_EventNotifier, stock, out stock._orderFSM_Cookie);

					stock.state = State.ClosingPosition;

					statistics.openedPositions++;
					break;
				case State.PaperClosed:
				case State.ClosingPosition:
					//roundtrip is complete - perform cleanup
					if (stock.IsRealTrade)
					{
						//in real trade i want to cleanup the order fsm,
						//remove the reference to the complete order from stock
						if (_orderFSM.Remove(ref stock._orderFSM_Cookie))
							stock._orderFSM_Cookie = null;
					}

					//aggregate statistics
					statistics.PnL = statistics.PnL + stock._trigger.PnL_NIS;
					statistics.fees = statistics.fees + stock._trigger.Fees;
					statistics.PnL_fees = statistics.PnL_fees + stock._trigger.PnL_Fees;
					statistics.completePositions++;

					this.TriggerCleanup(stock);
					stock.state = State.WaitingTrigger;

					break;
				default:
					HandleMessage_Default(msg, stock.state);
					break;
			}
		}

		private void HandleMessage_Error(MbxMessage msg)
		{
			AlgoStock stock = (AlgoStock)msg.msgData;
			stock.lastEvent = msg.msgEvent;
			//log
			logger.AddEntry(stock);


			switch (stock.state)
			{
				case State.OpeningPosition:
					//here i need to decide whether to insist on this trade or give up
					break;
				case State.ClosingPosition:
					//here i must get out of the position, so re-try or pass it to the operator 
					//to manually take care of it
					break;
				default:
					break;
			}
		}

		protected void HandleMessage_Stop()
		{
			if (this.Mode == Base.Mode.Semiautomatic)
			{
				if (_triggeredStock != null)
				{
					Console.WriteLine("Active trigger in the system. Can't stop.");
				}
			}

			foreach (AlgoStock stock in this.secList.Values)
			{
				switch (stock.state)
				{
					case State.WaitingTrigger:
						stock.state = State.Idle;
						break;
					default:
						Console.WriteLine("Can't stop stock " + stock.Name + " in state " + stock.state);
						break;
				}
			}
		}

		protected void HandleMessage_Start()
		{
			foreach (AlgoStock stock in secList.Values)
			{
				switch (stock.state)
				{
					case State.Idle:
						stock.state = State.WaitingTrigger;
						break;
					default:
						Console.WriteLine("Failed to start algo for stock " + stock.Name + "because it's in sate " + stock.state);
						break;
				}
			}
		}

		protected void HandleMessage_OrderCanceled(MbxMessage msg)
		{
			AlgoStock stock = (AlgoStock)msg.msgData;
			stock.lastEvent = msg.msgEvent;
			//log
			logger.AddEntry(stock);


			switch (stock.state)
			{
				case State.PaperClosed:
				case State.OpeningPosition:
					//the only place to cancel orders is when attempt to
					//open position fails (probably because the trigger 
					//disappeared too fast. 
					//after order is canceled - do cleanup
					if (stock.IsRealTrade)
					{
						if (_orderFSM.Remove(ref stock._orderFSM_Cookie))
							stock._orderFSM_Cookie = null;
					}

					this.TriggerCleanup(stock);

					statistics.missedTriggers++;

					break;
				default:
					HandleMessage_Default(msg, stock.state);
					break;
			}
		}

		protected void HandleMessageTimerReactionExpired(MbxMessage msg)
		{
			AlgoStock stock = (AlgoStock)msg.msgData;
			MbxMessage message;

			switch (stock.state)
			{
				case State.OpeningPosition:
					if (!IsTriggerStillActive(stock))
					{
						message = new MbxMessage(Event.PlacedLimitBook, stock);
					}
					else
					{
						stock.state = State.PaperOpened;
						message = new MbxMessage(Event.Fill, stock);
					}
					this.Send(message);

					break;
				default:
					HandleMessage_Default(msg, stock.state);
					break;
			}

		}

		protected void HandleMessage_Default(MbxMessage msg, State state)
		{
			Console.WriteLine("Unhandeled event " + msg.msgEvent + " in state " + state);
		}

		#endregion;


		protected void TriggerCleanup(AlgoStock stock)
		{
			stock.TriggerCleanup();
			stock.state = State.WaitingTrigger;

			if (this.Mode == Base.Mode.Semiautomatic)
			{
				_triggeredStock = null;
			}
		}

		#region paper trading methods

		/// <summary>
		/// minimal time of reaction after which I can assume that I got fill at market price.
		/// need it for paper trading logic.
		/// </summary>
		const int MIN_REACTION_TIME = 500;

		/// <summary>
		/// paper complete indication
		/// here a lot of assumptions were made, because in most cases 
		/// there no sure way to know if someone was ready to take our LMT order
		/// 3 complete indications are used, the first is the strongest, the last one is less
		/// </summary>
		/// <param name="stock"></param>
		private bool ClosePaperPositionIndication(AlgoStock stock)
		{
			int complete = 0;

			double enterPrc = stock.orderOpen.Price;
			double exitPrc = stock.orderClose.Price;
			double lmt_sl1 = stock.latestTicks[stock.latestTicksCounter - 1].ask[0].price;
			double lmt_by1 = stock.latestTicks[stock.latestTicksCounter - 1].bid[0].price;

			//latest transaction since the trigger is active, zero if none
			double lst_dl_pr = 0.0;
			if (stock.timeTrigger < stock.latestTicks[0].lastTradeTime)
				lst_dl_pr = stock.latestTicks[stock.latestTicksCounter - 1].lastTradePrice;

			// if the trigger is at the bid side
			if (stock.orderClose.TransactionType == TransactionType.BUY)
			{
				// (1) - the best ask is better than the target exit price, means any outstanding LMT was filled
				if (lmt_sl1 <= exitPrc)
					complete = 1;

				// (2) - there was a transaction at target exit price or better
				// TODO - add volume check if exactly target price
				if (lst_dl_pr > 0 && lst_dl_pr <= exitPrc)
					complete = 2;

				// (3) - the best bid is better than the second bid @ the time of the trigger.
				if (lmt_by1 < stock.trigger_lmt2)
					complete = 3;
			}

			// if the trigger is at the ask side
			else
			{
				// (1) - the best bid is better than target exit price
				if (lmt_by1 >= exitPrc)
					complete = 1;

				// (2) - there was a transaction at target exit price or better
				// todo - add volume check if exactly target price
				if (lst_dl_pr > 0 && lst_dl_pr >= exitPrc)
					complete = 2;

				// (3) - the best bid is better than the second bid @ the time of the trigger
				if (lmt_sl1 > stock.trigger_lmt2)
					complete = 3;
			}


			return (complete > 0);
		}

		#endregion


		/// <summary>
		/// call this method in ClosingPosition state on DataUpdate event - the only situation
		/// we might want to update our position's exit price - in case there are limits better 
		/// than ours, we're ready to trade flat (before commissions). If it's better than our 
		/// position's opening price, we post stop loss immediately - don't wait it to recover.
		/// at the moment we don't correct better - leave it for later
		/// </summary>
		/// <param name="stock"></param>
		private bool StopLossIndication(AlgoStock stock, out double newPrice, out bool loss)
		{
			newPrice = 0.0;
			loss = false;
			bool result = false;

			LimitOrderBook latest = stock.latestTicks[stock.latestTicksCounter - 1];
			LMTOrderParameters orderClose = stock.orderClose;

			double lmt_sl1 = latest.ask[0].price;
			int lmy_sl1_nv = latest.ask[0].size;

			double lmt_by1 = latest.bid[0].price;
			int lmy_by1_nv = latest.bid[0].size;

			double enterPrc = stock.orderOpen.Price;
			double exitPrc = orderClose.Price;

			//Bid-side trigger
			if (orderClose.TransactionType == TransactionType.BUY)
			{
				// stop loss is here - exit immediately
				if (lmt_by1 > enterPrc &&
					lmy_by1_nv >= JQuant.TASEUtils.MinLotSize(lmt_by1, stock.MIN_LOT_NIS))
				{
					result = true;
					newPrice = lmt_sl1;
					loss = true;
				}
				// some orders are better than our original exit target - need to correct
				else if (lmt_by1 < enterPrc &&
					lmt_by1 > exitPrc)
				{
					result = true;
					newPrice = lmt_by1 + JQuant.TASEUtils.TickSize(lmt_by1, true);
				}
			}

			//Ask-side trigger
			else if (orderClose.TransactionType == TransactionType.SELL)
			{
				// stop loss is here - exit immediately
				if (lmt_sl1 <= enterPrc &&
					lmy_sl1_nv >= JQuant.TASEUtils.MinLotSize(lmt_sl1, stock.MIN_LOT_NIS))
				{
					result = true;
					loss = true;
					newPrice = lmt_by1;
				}
				// some orders are better than our original exit target - need to correct
				else if (lmt_sl1 > enterPrc && lmt_sl1 < exitPrc)
				{
					result = true;
					newPrice = lmt_sl1 - JQuant.TASEUtils.TickSize(lmt_sl1, false);
				}
			}
			return result;
		}

		private bool IsTriggerStillActive(AlgoStock stock)
		{
			bool result = true;

			if (stock.timeTrigger != default(DateTime))
			{
				LMTOrderParameters orderOpen = stock.orderOpen;
				LimitOrderBook latest = stock.latestTicks[stock.latestTicksCounter - 1];

				if (orderOpen.TransactionType == TransactionType.BUY)
				{
					if (latest.ask[0].price > stock.orderOpen.Price)
					{
						result = false;
					}
				}
				else if (orderOpen.TransactionType == TransactionType.SELL)
				{
					if (latest.bid[0].price < stock.orderOpen.Price)
					{
						result = false;
					}
				}
			}

			return result;
		}

		private bool TriggerCondition(AlgoStock stock, out bool bidCondition)
		{
			bidCondition = false;
			bool result = false;

			//I need at lest 3 recent records in order to start testing for triggers
			if (stock.latestTicksCounter >= AlgoStock.HISTORY_SIZE)
			{
				result = TriggerCondition_Bid(stock);
				bidCondition = result;

				// if I already have a bid trigger I don't need to check if the other side is triggered too
				// we can have a trigger only on one side of the book at a time
				if (!bidCondition)
					result = result || TriggerCondition_Ask(stock);

			}

			return result;
		}

		//TODO: probably convert all these parameters into xml, which will be read by the application
		protected const double MIN_PROFIT_RATE = 0.002;
		//protected const double MIN_LOT_NIS = 5000.0;
		protected const double MIN_PROFIT = 30.0;
		protected const double MAX_POS_SIZE = 50 * 1000 * 100;

		// I build two 'what-if' scenarios for each side of the book,
		// for the previous market data record: one for bid, one for ask
		// and compare it with the last market data. 
		// If the scenario is realized at the current data, then there is a trigger.

		/// <summary>
		/// Test for ask side trigger
		/// </summary>
		/// <param name="stock"></param>
		/// <returns></returns>
		protected bool TriggerCondition_Ask(AlgoStock stock)
		{
			// for ask I need to compare the current market data and the one before the previous
			// thus - 3 latest records. This is because of the way FMR updates data in the 
			// market data stream - the bid side first, then ask side, duplicating previously updated
			// bid side
			LimitOrderBook previous = stock.latestTicks[stock.latestTicksCounter - 3];
			LimitOrderBook latest = stock.latestTicks[stock.latestTicksCounter - 1];

			// Ask side scenario:
			//     (1) buy @ lower MKT price (1st ask) 
			//     (2) sell back @ higher LMT (better than current 2nd ask)
			//
			// the scenario is a pair of prices:
			//    - open (enter) price
			//    - close (exit) price
			double lmt_sl1 = previous.ask[0].price;

			// Exit - after the 1st best LMT becomes 2nd best, 
			// plus an extra down tick to be sure that I'm the 1st in the line
			double sl_exit_pr = lmt_sl1 - JQuant.TASEUtils.TickSize(lmt_sl1, false);

			// Enter: target price = maximal acceptable enter price
			// depends on the exit price computed above, and target profit rate.
			// TODO check if (1 - MIN_PROFIT_RATE) is practically similar to 1/(1+MIN_PROFIT_RATE)
			double sl_enter_pr = Math.Floor(sl_exit_pr * (1 - MIN_PROFIT_RATE) / 10) * 10;


			// current limit book data - i need it to compute the expected PnL

			lmt_sl1 = latest.ask[0].price;

			double lmt_sl2 = latest.ask[1].price;
			int lmy_sl1_nv = latest.ask[0].size;

			double lmt_by1 = latest.bid[0].price;
			int lmy_by1_nv = latest.bid[0].size;

			double lmt_by2 = latest.bid[1].price;
			int lmy_by2_nv = latest.bid[1].size;

			double lmt_by3 = latest.bid[2].price;
			int lmy_by3_nv = latest.bid[2].size;


			// eventually, the ask-side trigger
			bool sl_trigger = (
						  (lmt_sl1 <= sl_enter_pr)
								&&
						  (lmt_sl2 > sl_exit_pr)
								&&
						  (lmy_sl1_nv >= JQuant.TASEUtils.MinLotSize(lmt_sl1, stock.MIN_LOT_NIS))
								&&
						  (lmt_sl1 * lmy_sl1_nv < MAX_POS_SIZE)
								&&
						  ((-lmt_sl1 + (lmt_sl2 - JQuant.TASEUtils.TickSize(lmt_sl2, false))) * lmy_sl1_nv >= MIN_PROFIT)
							   );


			// stop loss, in NIS
			double stop_nis = 0;

			// compute the stop loss, in NIS
			// in case it can't be computed from the current
			// market data (not deep enough), give up the trigger
			if (lmy_sl1_nv <= lmy_by1_nv)
			{
				stop_nis = lmy_sl1_nv * lmt_by1 / 100;
			}
			else if (lmy_sl1_nv <= lmy_by1_nv + lmy_by2_nv)
			{
				stop_nis = (lmt_by1 * lmy_by1_nv + lmt_by2 * (lmy_sl1_nv - lmy_by1_nv)) / 100;
			}
			else if (lmy_sl1_nv <= lmy_by1_nv + lmy_by2_nv + lmy_by3_nv)
			{
				stop_nis = (lmt_by1 * lmy_by1_nv + lmt_by2 * lmy_by2_nv + lmt_by3 * (lmy_sl1_nv - lmy_by1_nv - lmy_by2_nv)) / 100;
			}
			else
			{
				sl_trigger = false;
			}

			//if there is a trigger @ ask, initialize orders data:
			if (sl_trigger)
			{
				//trigger time
				stock.timeTrigger = latest.timestamp;

				//open
				stock.orderOpen.TransactionType = TransactionType.BUY;
				stock.orderOpen.Price = lmt_sl1;
				stock.orderOpen.Quantity = lmy_sl1_nv;

				//close
				stock.orderClose.TransactionType = TransactionType.SELL;
				stock.orderClose.Price = sl_exit_pr;
				stock.orderClose.Quantity = lmy_sl1_nv;

				//2nd best limit
				stock.trigger_lmt2 = lmt_sl2;

				//compute and keep the trigger data
				stock._trigger = new Trigger();
				stock._trigger.Bid = false;
				stock._trigger.Size = lmy_sl1_nv;
				stock._trigger.EnterPrice = lmt_sl1;
				stock._trigger.ExitPrice = sl_exit_pr;
				stock._trigger.SL = Math.Round(stop_nis, 2);
			}

			return sl_trigger;
		}

		/// <summary>
		/// Test for bid-side trigger
		/// </summary>
		/// <param name="stock"></param>
		/// <returns></returns>
		protected bool TriggerCondition_Bid(AlgoStock stock)
		{
			LimitOrderBook previous = stock.latestTicks[stock.latestTicksCounter - 2];
			LimitOrderBook latest = stock.latestTicks[stock.latestTicksCounter - 1];

			// Bid side scenario:
			//     (1) short sell @ higher MKT price (1st bid) 
			//     (2) buy back @ lower LMT (better than current 2nd bid)
			//     
			// the scenario is a pair of prices:
			//    - open (enter) price
			//    - close (exit) price
			double lmt_by1 = previous.bid[0].price;

			// Exit - after the 1st best LMT becomes 2nd best, 
			// plus an extra up tick to be sure that I'm the 1st in the line
			double by_exit_pr = lmt_by1 + JQuant.TASEUtils.TickSize(lmt_by1, true);

			// Enter: target price = minimal acceptable enter price
			// depends on the exit price computed above, and target profit rate.
			double by_enter_pr = Math.Ceiling(by_exit_pr * (1 + MIN_PROFIT_RATE) / 10) * 10;


			// current limit book data - i need it to compute the expected PnL

			lmt_by1 = latest.bid[0].price;

			double lmt_by2 = latest.bid[1].price;
			int lmy_by1_nv = latest.bid[0].size;

			double lmt_sl1 = latest.ask[0].price;
			int lmy_sl1_nv = latest.ask[0].size;

			double lmt_sl2 = latest.ask[1].price;
			int lmy_sl2_nv = latest.ask[1].size;

			double lmt_sl3 = latest.ask[2].price;
			int lmy_sl3_nv = latest.ask[2].size;


			// eventually, the bid-side trigger
			bool by_trigger = (
						  (lmt_by1 >= by_enter_pr)
								&&
						  (lmt_by2 < by_exit_pr)
								&&
						  (lmy_by1_nv >= JQuant.TASEUtils.MinLotSize(lmt_by1, stock.MIN_LOT_NIS))
								&&
						  (lmt_by1 * lmy_by1_nv < MAX_POS_SIZE)
								&&
						  ((lmt_by1 - (lmt_by2 + JQuant.TASEUtils.TickSize(lmt_by2, true))) * lmy_by1_nv >= MIN_PROFIT)
							   );


			// stop loss, in NIS
			double stop_nis = 0;

			// compute the stop loss, in NIS
			// in case it can't be computed from the current
			// market data (not deep enough), give up the trigger
			if (lmy_by1_nv <= lmy_sl1_nv)
			{
				stop_nis = lmy_by1_nv * lmt_sl1 / 100;
			}
			else if (lmy_by1_nv <= lmy_sl1_nv + lmy_sl2_nv)
			{
				stop_nis = (lmt_sl1 * lmy_sl1_nv + lmt_sl2 * (lmy_by1_nv - lmy_sl1_nv)) / 100;
			}
			else if (lmy_by1_nv <= lmy_sl1_nv + lmy_sl2_nv + lmy_sl3_nv)
			{
				stop_nis = (lmt_sl1 * lmy_sl1_nv + lmt_sl2 * lmy_sl2_nv + lmt_sl3 * (lmy_by1_nv - lmy_sl1_nv - lmy_sl2_nv)) / 100;
			}
			else
			{
				by_trigger = false;
			}

			//if there is a trigger @ bid side, initialize orders data:
			if (by_trigger)
			{
				//trigger time
				stock.timeTrigger = latest.timestamp;

				//open
				stock.orderOpen.TransactionType = TransactionType.SELL;
				stock.orderOpen.Price = lmt_by1;
				stock.orderOpen.Quantity = lmy_by1_nv;

				//close
				stock.orderClose.TransactionType = TransactionType.BUY;
				stock.orderClose.Price = by_exit_pr;
				stock.orderClose.Quantity = lmy_by1_nv;

				//2nd best limit
				stock.trigger_lmt2 = lmt_by2;

				//compute and keep the trigger data
				stock._trigger = new Trigger();
				stock._trigger.Bid = true;
				stock._trigger.Size = lmy_by1_nv;
				stock._trigger.EnterPrice = lmt_by1;
				stock._trigger.ExitPrice = by_exit_pr;
				stock._trigger.SL = Math.Round(stop_nis, 2);

			}

			return by_trigger;
		}

		/// <summary>
		/// New data arrived from the market data stream.
		/// Check if the stock is already on the list of securities,
		/// if not - add it. Update the stock with the data.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		protected AlgoStock UpdateDictionary(LimitOrderBook data)
		{
			int id = data.id;
			AlgoStock stock;

			if (secList.ContainsKey(id))
			{
				stock = secList[id];
			}
			else
			{
				stock = new AlgoStock(data);
				if (TA25List.Contains(stock.IdNum))
					stock.MIN_LOT_NIS = 5000;
				secList.Add(id, stock);
				stock.state = State.WaitingTrigger;
			}

			stock.UpdateMarketData(data);

			// in SA mode i want to print new transactions only for triggered stock
			if (this.Mode == Base.Mode.Semiautomatic &&
				this._triggeredStock != null &&
				stock.state != State.WaitingTrigger &&
				stock.NewTransaction)
			{
				stock.PrintLastTrade();
			}

			return stock;
		}

		protected Statistics statistics;

		/// <summary>
		/// flag informing if the Algo paused
		/// </summary>
		bool _paused;

		/// <summary>
		/// system wide mode.
		/// depending on the mode i can enter or skip WaitForOperator state
		/// </summary>
		public Base.Mode Mode
		{
			get;
			set;
		}

		/// <summary>
		/// a reference to the shell containing the instance of this FSM
		/// </summary>
		public Base _base
		{
			get;
			set;
		}

		/// <summary>
		/// In semiautomatic trading I keep a reference to the stock which 
		/// caused the trigger. 
		/// I wait for the human operator to intervene - skip, place order, etc.
		/// In the fully automatic FSM I would not need this field.
		/// </summary>
		public Stock _triggeredStock
		{
			get;
			set;
		}


		TimerTask timerTask;
		TimerList reactionTimers;


		/// <summary>
		/// Order processor. Algo calls it when it needs to 
		/// send / update / cancel real-cash order. 
		/// Don't forget to call Remove() in order to perform cleanup
		/// after the order is not needed anymore (get fill / cancel).
		/// </summary>
		FMRShell.RezefOrderFSM _orderFSM;

		/// <summary>
		/// Lists all the Securities the system encounters.
		/// I use the dictionary to translate from stock ID to the 
		/// reference to the AlgoStock object instance.
		/// </summary>
		System.Collections.Generic.Dictionary<int, AlgoStock> secList;

		/// <summary>
		/// Logger
		/// </summary>
		AlgoLogger logger;

		/// <summary>
		/// Keeps the list of TA25 stocks. I need to identify them because the minimal lot size 
		/// for 25 is 5000 NIS, while for 75 it's 2000 NIS
		/// </summary>
		List<int> TA25List;
	}

	/// <summary>
	/// Shell around the AlgoFSM object.
	/// Object of this type subscribes to the market data feed and looks for triggers. 
	/// If triggered in the semiautomatic mode, it prints out the the trigger and waits for operator's decision. 
	/// In the fully automatic mode the FSM places the oder according to its internal logic with no operator's intervention.
	/// 
	/// In the semiautomatic mode three possibilities exist:
	///   - Skip, when FSM looks for the next trigger
	///   - Place real trade, when FSM uses TaskBarLib (FMR) to place the trade to the TASE
	///   - Place paper trade, when FSM notes that there is a pending trade and attempts to exercise 
	///     the trade on paper, simulating trading events from the market data.
	///     
	/// Call "Start" to make the gears moving.
	/// </summary>
	public class Base : IConsumer<FMRShell.MarketData>, IResourceStatistics, IDisposable
	{
		/// <summary>
		/// constructor - intializes the algo
		/// </summary>
		/// <param name="orderFsm">references to the order processor server</param>
		public Base(FMRShell.RezefOrderFSM orderFsm)
		{
			fsm = new AlgoFSM(orderFsm);

			//pass to the Algo the reference to itself
			fsm._base = this;
			//i set mode SA hardcoded
			//TODO chose mode from the CLI
			fsm.Mode = Mode.Semiautomatic;
		}

		public void Start()
		{
			//activate the AlgoFSM
			fsm.Start();
			MbxMessage msg = new MbxMessage(Event.Start, null);
			fsm.Send(msg);
		}


		// TODO at the moment Pause / StopGracefull / StopUrgent use the same AlgoFSM.Stop()
		// More logic is needed for closing any opened position in each method
		// For StopUrgent it will probably be liquidating usink MKT orders
		// For StopGracefull it can be some kind of LMT
		// For Pause no need to close existing orders is needed - just wait for existing trades to
		// finish, and ignore any new triggers.
		public void Pause()
		{
			// send Pause to the FSM
			fsm.Stop();
			MbxMessage msg = new MbxMessage(Event.Stop, null);
			fsm.Send(msg);
		}

		public void StopGracefull()
		{
			// send Stop to the FSM
			//fsm.Stop();
			//this.Dispose();
		}

		public void StopUrgent()
		{
			fsm.Stop();
			this.Dispose();
		}

		public void Dispose()
		{
			fsm.Dispose();
		}

		public enum Mode
		{
			Automatic,
			Semiautomatic
		}

		/// <summary>
		/// Toggles automatic/semiautomatic mode
		/// </summary>
		public void SetMode(Mode mode)
		{
			// set Mode in the FSM
			fsm.SetMode(mode);
		}

		/// <summary>
		/// Interface JQuant.IResourceStatistics requires this method to be implemented
		/// Return all debug counters
		/// </summary>
		public void GetEventCounters(out System.Collections.ArrayList names, out System.Collections.ArrayList values)
		{
			// get counters from the FSM
			fsm.GetEventCounters(out names, out values);

			// I can add more counters from the Base if I need 
		}

		/// <summary>
		/// Called by data feed (producer). I will allocate a new message and send to the
		/// Algo FSM for further processing. The method should return fairly fast.
		/// </summary>
		public void Notify(int count, FMRShell.MarketData data)
		{
			// TODO if trigger condition can be discovered quickly do it here before cloning

			// i am going to postpone the processing
			// i have to clone the object first - i convert the data to internal type
			LimitOrderBook lob;

			RawDataToLimitBookData(data, out lob);

			// build and send message to the processing task
			MbxMessage msg = new MbxMessage(Event.DataUpdate, lob);
			fsm.Send(msg);
		}

		/// <summary>
		/// Convert raw data (lot of strings) from data feed to internal format
		/// </summary>
		private static void RawDataToLimitBookData(FMRShell.MarketData rawdata, out LimitOrderBook limitBookData)
		{
			limitBookData = new LimitOrderBook();
			K300RzfType rezef = (K300RzfType)(((FMRShell.MarketDataRezef)rawdata).Data);

			limitBookData.id = JQuant.Convert.StrToInt(rezef.BNO_Num);
			limitBookData.name = rezef.BNO_NAME_E;	//English name
			limitBookData.bid[0].price = JQuant.Convert.StrToDouble(rezef.LMT_BY1, 0.0);
			limitBookData.bid[1].price = JQuant.Convert.StrToDouble(rezef.LMT_BY2, 0.0);
			limitBookData.bid[2].price = JQuant.Convert.StrToDouble(rezef.LMT_BY3, 0.0);
			limitBookData.bid[0].size = JQuant.Convert.StrToInt(rezef.LMY_BY1_NV);
			limitBookData.bid[1].size = JQuant.Convert.StrToInt(rezef.LMY_BY2_NV);
			limitBookData.bid[2].size = JQuant.Convert.StrToInt(rezef.LMY_BY3_NV);
			limitBookData.ask[0].price = JQuant.Convert.StrToDouble(rezef.LMT_SL1, 0.0);
			limitBookData.ask[1].price = JQuant.Convert.StrToDouble(rezef.LMT_SL2, 0.0);
			limitBookData.ask[2].price = JQuant.Convert.StrToDouble(rezef.LMT_SL3, 0.0);
			limitBookData.ask[0].size = JQuant.Convert.StrToInt(rezef.LMY_SL1_NV);
			limitBookData.ask[1].size = JQuant.Convert.StrToInt(rezef.LMY_SL2_NV);
			limitBookData.ask[2].size = JQuant.Convert.StrToInt(rezef.LMY_SL3_NV);
			limitBookData.lastTradePrice = JQuant.Convert.StrToDouble(rezef.LST_DL_PR, 0.0);
			limitBookData.lastTradeSize = JQuant.Convert.StrToInt(rezef.LST_DL_VL);
			limitBookData.dayVolume = JQuant.Convert.StrToInt(rezef.DAY_VL);
			limitBookData.tick = rawdata.Ticks;                                  // system tick
			limitBookData.timestamp = rawdata.TimeStamp;                         // and inernal time stamp 
		}


		/// <summary>
		/// called by the delegate from within orderFSM.
		/// if needed, converts orderFSM events to AlgoFSM events and sends 
		/// message to the AlgoFSM informing about them
		/// </summary>
		/// <param name="cookie"></param>
		/// <param name="_orderFSMevent"></param>
		public void OrderFSM_EventNotifier(ref object cookie, OrderEventNotifierMessage _message)
		{
			MbxMessage msg;

			switch (_message.orderEvent)
			{
				case OrderProcessorEvent.InitOrder:
					//do nothing besides log
					break;
				case OrderProcessorEvent.OrderSent:
					msg = new MbxMessage(Event.PaperTrade, cookie);
					fsm.Send(msg);
					break;
				case OrderProcessorEvent.TradingApproved:
				case OrderProcessorEvent.Updated:
					msg = new MbxMessage(Event.PlacedLimitBook, cookie);
					fsm.Send(msg);
					break;
				case OrderProcessorEvent.PartialFill:
					msg = new MbxMessage(Event.PartialFill, cookie, _message.data);
					fsm.Send(msg);
					break;
				case OrderProcessorEvent.CompleteFill:
					msg = new MbxMessage(Event.Fill, cookie);
					fsm.Send(msg);
					break;
				case OrderProcessorEvent.Error:
					msg = new MbxMessage(Event.OrderError, cookie);
					fsm.Send(msg);
					break;
				case OrderProcessorEvent.CancelSent:
					//log - orderFSM informed about sending Cancel request
					//no cancel approved yet
					break;
				case OrderProcessorEvent.Canceled:
					msg = new MbxMessage(Event.OrderCanceled, cookie);
					fsm.Send(msg);
					break;
				case OrderProcessorEvent.UpdateSent:
					//log - orderFSM informed about sending Update
					//request - no TASE approval yet
					break;
				case OrderProcessorEvent.Cleanup:
					//orderFSM repors aboutr sucessful cleanup
					//after the user asked the orderFSM for cleanup
					break;
				default:
					Console.WriteLine("Unhandled order processor event " + _message);
					break;
			}
		}

		public void PlacePaperTrade()
		{
			if (fsm._triggeredStock != null)
			{
				MbxMessage msg = new MbxMessage(Event.PaperTrade, fsm._triggeredStock);
				fsm.Send(msg);
			}
		}

		public void PlaceRealTrade()
		{
			if (fsm._triggeredStock != null)
			{
				MbxMessage msg = new MbxMessage(Event.RealTrade, fsm._triggeredStock);
				fsm.Send(msg);
			}
		}

		public void SkipTrigger()
		{
			if (fsm._triggeredStock != null)
			{
				MbxMessage msg = new MbxMessage(Event.Skip, fsm._triggeredStock);
				fsm.Send(msg);
			}
			else
				Console.WriteLine("No active triggers in the system. Can't skip.");
		}

		/// <summary>
		/// Algo FSM (mailbox thread) 
		/// </summary>
		private AlgoFSM fsm;
	}

	class AlgoParameters
	{
	}

	class ReadAlgoParameters : XmlTextReader
	{
		public ReadAlgoParameters(string filename)
			: base(filename)
		{
		}

		private enum xmlState
		{
			[Description("BEGIN")]
			BEGIN,
			[Description("STOCKS")]
			STOCKS,
			[Description("STOCKS_STOCK")]
			STOCKS_STOCK,
			[Description("STOCK")]
			STOCK,
			[Description("PARAMS")]
			PARAMS,
			[Description("USERNAME")]
			USERNAME,
			[Description("PASSWORD")]
			PASSWORD,
			[Description("ACCOUNT")]
			ACCOUNT,
		}

		public bool Parse(out AlgoParameters parameters)
		{
			xmlState state = xmlState.BEGIN;
			string username = "";
			string password = "";
			string account = "";

			bool result = true;
			string val;
			parameters = null;

			while (base.Read())
			{
				switch (base.NodeType)
				{
					case XmlNodeType.Element:
						val = base.Name;
						if ((val.Equals("algoparameters")) && (state == xmlState.BEGIN))
						{
							state = xmlState.PARAMS;
						}
						else if ((val.Equals("username")) && (state == xmlState.PARAMS))
						{
							state = xmlState.USERNAME;
						}
						else if ((val.Equals("password")) && (state == xmlState.USERNAME))
						{
							state = xmlState.PASSWORD;
						}
						else if ((val.Equals("account")) && (state == xmlState.PASSWORD))
						{
							state = xmlState.ACCOUNT;
						}
						else
						{
							Console.WriteLine("Failed at element " + val + " in state " + JQuant.EnumUtils.GetDescription(state));
							result = false;
						}
						break;

					case XmlNodeType.Text:
						val = base.Value;
						if (state == xmlState.USERNAME)
						{
							username = val;
						}
						else if (state == xmlState.PASSWORD)
						{
							password = val;
						}
						else if (state == xmlState.ACCOUNT)
						{
							account = val;
						}
						else
						{
							Console.WriteLine("Failed at text " + val + " in state " + JQuant.EnumUtils.GetDescription(state));
							result = false;
						}
						break;

					case XmlNodeType.EndElement:
						// I will not check that end element Name is Ok 
						val = base.Name;
						if ((val.Equals("connectionparameters")) && (state == xmlState.ACCOUNT))
						{
						}
						else if ((val.Equals("username")) && (state == xmlState.USERNAME))
						{
						}
						else if ((val.Equals("password")) && (state == xmlState.PASSWORD))
						{
						}
						else if ((val.Equals("account")) && (state == xmlState.ACCOUNT))
						{
						}
						else
						{
							Console.WriteLine("Failed at EndElement " + val + " in state " + JQuant.EnumUtils.GetDescription(state));
							result = false;
						}
						break;
				}

				// something is broken in the XML file
				if (!result)
				{
					break;
				}
			}

			// if parsing worked - create new object
			if (result)
			{
				parameters = new AlgoParameters();
			}

			return result;
		}
	}
}
