
using System;
using System.Collections.Generic;
using System.Reflection;

#if USEFMRSIM
using TaskBarLibSim;
#else
using TaskBarLib;
#endif

namespace JQuant
{
	/// <summary>
	/// This is a simulation of the Maof options market. The class collects incoming events - events taken
	/// from the historical data, keeps track of all strikes. MarketSimulation compares pending orders 
	/// with the market state and figures out if fill was possible. To make the whole exercise practical 
	/// for the current phase I assume that incoming orders do not influence the market. 
	/// I assume that if bid (ask) limit is equal or greater (less) to the best ask (bid),
	/// the probability of the fill is 1 (immediate fill).
	/// </summary>
	public class MarketSimulationMaof : MarketSimulation.Core, JQuant.IConsumer<K300MaofType>
	{
		public MarketSimulationMaof()
		{
			// Market depth (size of the order book) is 3 on TASE
			// i am going to reuse this object 
			marketData = new MarketSimulation.MarketData(3);
			securities = new System.Collections.Hashtable(100);


			regexOption = new System.Text.RegularExpressions.Regex(BNO_NAME_PATTERN_OPTION);
			regexFuture = new System.Text.RegularExpressions.Regex(BNO_NAME_PATTERN_FUTURE);
		}

		/// <summary>
		/// Pattern for regular expression - 12 months
		/// </summary>
		protected const string MONTHS_PATTERN = "JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC";

		/// <summary>
		/// Pattern for regular expression - BNO_NAME_E for Maof option
		/// </summary>
		public const string BNO_NAME_PATTERN_OPTION = "T25 (P|C)([0-9]+) (" + MONTHS_PATTERN + ")[0-9]+";

		/// <summary>
		/// Pattern for regular expression - BNO_NAME_E for Maof future
		/// </summary>
		public const string BNO_NAME_PATTERN_FUTURE = "T25 F (" + MONTHS_PATTERN + ")[0-9]+";

		/// <summary>
		/// The meethod belongs to the IConsumer interface and called from
		/// data genrerator, like MaofDataGeneratorLogFile
		/// </summary>
		public void Notify(int count, K300MaofType data)
		{
			// convert TASE format to the internal object
			// same object is reused here
			RawDataToMarketData(data, ref marketData);

			// update local hash table - i keep the latest entry
			// This is a bug potentially - i have to clone the structure here
			// This works only becase data gennerator created a new object 
			securities[marketData.id] = data;

			// forward to the market simulation logic
			base.Notify(count, marketData);
		}


		/// <summary>
		/// Returns false if I want to drop the data instead of fowarding to the
		/// MarketSimulation.Core
		/// </summary>
		protected bool FilterMaofData(K300MaofType dt)
		{
			string BNO_NAME_E = dt.BNO_NAME_E;
			bool res;

			System.Text.RegularExpressions.MatchCollection matches = regexOption.Matches(BNO_NAME_E);
			res = (matches.Count == 1);  // i expect exactly one match

			if (matches.Count < 1)
			{
				matches = regexFuture.Matches(BNO_NAME_E);
				if (matches.Count < 1)  // this is not an option and not a future - report error
				{
					System.Console.WriteLine("Maof MarketSimulation: drop data for " + BNO_NAME_E);
				}
				else
				{
					// System.Console.WriteLine("Maof MarketSimulation: drop data for future "+BNO_NAME_E);
				}
			}
			else if (matches.Count > 1)
			{
				System.Console.WriteLine("Maof MarketSimulation: drop data for " + BNO_NAME_E + " because of multiple match");
			}

			return res;
		}

		/// <summary>
		/// DataType is something like K300MaofType - lot of strings. The method will  convert
		/// this into something convenient to work with.
		/// </summary>
		/// <param name="dt">
		/// A <see cref="System.Object"/>
		/// Object of type DataType
		/// </param>
		/// <returns>
		/// A <see cref="MarketData"/>
		/// New object containing integers like Price, best bid/ask, etc.
		/// </returns>
		protected void RawDataToMarketData(K300MaofType dt, ref MarketSimulation.MarketData md)
		{
			if (FilterMaofData(dt))
			{
				md.id = JQuant.Convert.StrToInt(dt.BNO_Num);
				md.bid[0].price = JQuant.Convert.StrToInt(dt.LMT_BY1);
				md.bid[1].price = JQuant.Convert.StrToInt(dt.LMT_BY2);
				md.bid[2].price = JQuant.Convert.StrToInt(dt.LMT_BY3);
				md.bid[0].size = JQuant.Convert.StrToInt(dt.LMY_BY1_NV);
				md.bid[1].size = JQuant.Convert.StrToInt(dt.LMY_BY2_NV);
				md.bid[2].size = JQuant.Convert.StrToInt(dt.LMY_BY3_NV);
				md.ask[0].price = JQuant.Convert.StrToInt(dt.LMT_SL1);
				md.ask[1].price = JQuant.Convert.StrToInt(dt.LMT_SL2);
				md.ask[2].price = JQuant.Convert.StrToInt(dt.LMT_SL3);
				md.ask[0].size = JQuant.Convert.StrToInt(dt.LMY_SL1_NV);
				md.ask[1].size = JQuant.Convert.StrToInt(dt.LMY_SL2_NV);
				md.ask[2].size = JQuant.Convert.StrToInt(dt.LMY_SL3_NV);
				md.lastTrade = JQuant.Convert.StrToInt(dt.LST_DL_PR);
				md.lastTradeSize = JQuant.Convert.StrToInt(dt.LST_DL_VL);
				md.dayVolume = JQuant.Convert.StrToInt(dt.DAY_VL);
				md.tick = JQuant.Convert.StrToLong(dt.UPD_TIME, 0);
			}
		}

		/// <summary>
		/// Use this class to get access to the all getters for the Maof option 
		/// Method MarketSimulationMaof.GetSecurity() conveniently creates objects 
		/// of this class
		/// </summary>
		public class Option
		{
			public Option(int id, MarketSimulationMaof ms)
			{
				object o = ms.securities[id];
				if (o != null)
				{
					exists = true;
					md = (K300MaofType)o;
				}
				else
				{
					exists = false;
				}

			}

			public bool Exists()
			{
				return exists;
			}

			public string GetName()
			{
				string res = "Unknown";

				if (exists)
				{
					res = md.BNO_NAME_E;
				}

				return res;
			}

			public int GetExpirationPrice()
			{
				int res = 0;

				if (exists)
				{
					res = JQuant.Convert.StrToInt(md.EX_PRC, 0);
				}

				return res;
			}

			public bool IsPut()
			{
				bool res = false;

				if (exists)
				{
					string bno_name = md.BNO_NAME;
					char firstChar = bno_name[0];
					res = (firstChar == 'P');
				}

				return res;
			}

			/// <summary>
			/// Returns best bids or asks
			/// </summary>
			public MarketSimulation.OrderPair[] GetBook(TransactionType transaction)
			{
				MarketSimulation.OrderPair[] res;

				if (transaction == TransactionType.SELL)
				{
					res = GetBookAsk();
				}
				else
				{
					res = GetBookBid();
				}


				return res;
			}

			/// <summary>
			/// Returns order book
			/// </summary>
			public MarketSimulation.OrderPair[] GetBookBid()
			{
				MarketSimulation.OrderPair[] res = new MarketSimulation.OrderPair[0];

				if (exists)
				{
					// i have three best bids (three best buy orders)
					res = new MarketSimulation.OrderPair[3];
					res[0] = new MarketSimulation.OrderPair(
							   JQuant.Convert.StrToInt(md.LMT_BY1, 0), JQuant.Convert.StrToInt(md.LMY_BY1_NV, 0));
					res[1] = new MarketSimulation.OrderPair(
							   JQuant.Convert.StrToInt(md.LMT_BY2, 0), JQuant.Convert.StrToInt(md.LMY_BY2_NV, 0));
					res[2] = new MarketSimulation.OrderPair(
							   JQuant.Convert.StrToInt(md.LMT_BY3, 0), JQuant.Convert.StrToInt(md.LMY_BY3_NV, 0));
				}

				return res;
			}

			/// <summary>
			/// Returns order book
			/// </summary>
			public MarketSimulation.OrderPair[] GetBookAsk()
			{
				MarketSimulation.OrderPair[] res = new MarketSimulation.OrderPair[0];

				if (exists)
				{
					// i have three best bids (three best buy orders)
					res = new MarketSimulation.OrderPair[3];
					res[0] = new MarketSimulation.OrderPair(
							   JQuant.Convert.StrToInt(md.LMT_SL1, 0), JQuant.Convert.StrToInt(md.LMY_SL1_NV, 0));
					res[1] = new MarketSimulation.OrderPair(
							   JQuant.Convert.StrToInt(md.LMT_SL2, 0), JQuant.Convert.StrToInt(md.LMY_SL2_NV, 0));
					res[2] = new MarketSimulation.OrderPair(
							   JQuant.Convert.StrToInt(md.LMT_SL3, 0), JQuant.Convert.StrToInt(md.LMY_SL3_NV, 0));
				}

				return res;
			}

			private K300MaofType md;
			private bool exists;
		}

		/// <summary>
		/// Returns entry from the hashtable keeping the latest market snapshot
		/// </summary>
		public Option GetOption(int id)
		{
			return new Option(id, this);
		}

		protected MarketSimulation.MarketData marketData;

		/// <summary>
		/// Collection of all traded symbols (different BNO_Num for TASE)
		/// I keep the last update data in this hash table
		/// </summary>
		protected new System.Collections.Hashtable securities;

		protected System.Text.RegularExpressions.Regex regexOption;
		protected System.Text.RegularExpressions.Regex regexFuture;

	}


	/// <summary>
	/// This is a simulation of the TASE (Rezef) stock market. The class collects incoming events - events taken
	/// from the historical data, keeps track of all strikes. MarketSimulation compares pending orders 
	/// with the market state and figures out if fill was possible. To make the whole exercise practical 
	/// for the current phase I assume that incoming orders do not influence the market. 
	/// I assume that if bid (ask) limit is equal or greater (less) to the best ask (bid),
	/// the probability of the fill is 1 (immediate fill).
	/// </summary>
	public class MarketSimulationRezef : MarketSimulation.Core, JQuant.IConsumer<K300RzfType>
	{
		public MarketSimulationRezef()
		{
			// Market depth (size of the order book) is 3 on TASE
			// i am going to reuse this object 
			marketData = new MarketSimulation.MarketData(3);
			securities = new System.Collections.Hashtable(100);

		}

		/// <summary>
		/// The meethod belongs to the IConsumer interface and called from
		/// data genrerator, like MaofDataGeneratorLogFile
		/// </summary>
		public void Notify(int count, K300RzfType data)
		{
			// convert TASE format to the internal object
			// same object is reused here
			RawDataToMarketData(data, ref marketData);

			// update local hash table - i keep the latest entry
			// This is a bug potentially - i have to clone the structure here
			// This works only becase data gennerator created a new object 
			securities[marketData.id] = data;

			// forward to the market simulation logic
			base.Notify(count, marketData);
		}


		/// <summary>
		/// Returns false if I want to drop the data instead of fowarding to the
		/// MarketSimulation.Core
		/// </summary>
		protected bool FilterRezefData(K300RzfType dt)
		{
			// let everything in
			return true;
		}

		/// <summary>
		/// DataType is something like K300MaofType - lot of strings. The method will  convert
		/// this into something convenient to work with.
		/// </summary>
		/// <param name="dt">
		/// A <see cref="System.Object"/>
		/// Object of type DataType
		/// </param>
		/// <returns>
		/// A <see cref="MarketData"/>
		/// New object containing integers like Price, best bid/ask, etc.
		/// </returns>
		protected void RawDataToMarketData(K300RzfType dt, ref MarketSimulation.MarketData md)
		{
			if (FilterRezefData(dt))
			{
				md.id = JQuant.Convert.StrToInt(dt.BNO_Num);
				md.bid[0].price = JQuant.Convert.StrToInt(dt.LMT_BY1);
				md.bid[1].price = JQuant.Convert.StrToInt(dt.LMT_BY2);
				md.bid[2].price = JQuant.Convert.StrToInt(dt.LMT_BY3);
				md.bid[0].size = JQuant.Convert.StrToInt(dt.LMY_BY1_NV);
				md.bid[1].size = JQuant.Convert.StrToInt(dt.LMY_BY2_NV);
				md.bid[2].size = JQuant.Convert.StrToInt(dt.LMY_BY3_NV);
				md.ask[0].price = JQuant.Convert.StrToInt(dt.LMT_SL1);
				md.ask[1].price = JQuant.Convert.StrToInt(dt.LMT_SL2);
				md.ask[2].price = JQuant.Convert.StrToInt(dt.LMT_SL3);
				md.ask[0].size = JQuant.Convert.StrToInt(dt.LMY_SL1_NV);
				md.ask[1].size = JQuant.Convert.StrToInt(dt.LMY_SL2_NV);
				md.ask[2].size = JQuant.Convert.StrToInt(dt.LMY_SL3_NV);
				md.lastTrade = JQuant.Convert.StrToInt(dt.LST_DL_PR);
				md.lastTradeSize = JQuant.Convert.StrToInt(dt.LST_DL_VL);
				md.dayVolume = JQuant.Convert.StrToInt(dt.DAY_VL);
				md.tick = JQuant.Convert.StrToLong(dt.UPD_TIME, 0);
			}
		}

		/// <summary>
		/// Use this class to get access to the all getters for the Maof option 
		/// Method MarketSimulationMaof.GetSecurity() conveniently creates objects 
		/// of this class
		/// </summary>
		public class Stock
		{
			public Stock(int id, MarketSimulationRezef ms)
			{
				object o = ms.securities[id];
				if (o != null)
				{
					exists = true;
					md = (K300RzfType)o;
				}
				else
				{
					exists = false;
				}

			}

			public bool Exists()
			{
				return exists;
			}

			public string GetName()
			{
				string res = "Unknown";

				if (exists)
				{
					res = md.BNO_NAME_E;
				}

				return res;
			}

			/// <summary>
			/// Returns best bids or asks
			/// </summary>
			public MarketSimulation.OrderPair[] GetBook(TransactionType transaction)
			{
				MarketSimulation.OrderPair[] res;

				if (transaction == TransactionType.SELL)
				{
					res = GetBookAsk();
				}
				else
				{
					res = GetBookBid();
				}


				return res;
			}

			/// <summary>
			/// Returns order book
			/// </summary>
			public MarketSimulation.OrderPair[] GetBookBid()
			{
				MarketSimulation.OrderPair[] res = new MarketSimulation.OrderPair[0];

				if (exists)
				{
					// i have three best bids (three best buy orders)
					res = new MarketSimulation.OrderPair[3];
					res[0] = new MarketSimulation.OrderPair(
							   JQuant.Convert.StrToInt(md.LMT_BY1, 0), JQuant.Convert.StrToInt(md.LMY_BY1_NV, 0));
					res[1] = new MarketSimulation.OrderPair(
							   JQuant.Convert.StrToInt(md.LMT_BY2, 0), JQuant.Convert.StrToInt(md.LMY_BY2_NV, 0));
					res[2] = new MarketSimulation.OrderPair(
							   JQuant.Convert.StrToInt(md.LMT_BY3, 0), JQuant.Convert.StrToInt(md.LMY_BY3_NV, 0));
				}

				return res;
			}

			/// <summary>
			/// Returns order book
			/// </summary>
			public MarketSimulation.OrderPair[] GetBookAsk()
			{
				MarketSimulation.OrderPair[] res = new MarketSimulation.OrderPair[0];

				if (exists)
				{
					// i have three best bids (three best buy orders)
					res = new MarketSimulation.OrderPair[3];
					res[0] = new MarketSimulation.OrderPair(
							   JQuant.Convert.StrToInt(md.LMT_SL1, 0), JQuant.Convert.StrToInt(md.LMY_SL1_NV, 0));
					res[1] = new MarketSimulation.OrderPair(
							   JQuant.Convert.StrToInt(md.LMT_SL2, 0), JQuant.Convert.StrToInt(md.LMY_SL2_NV, 0));
					res[2] = new MarketSimulation.OrderPair(
							   JQuant.Convert.StrToInt(md.LMT_SL3, 0), JQuant.Convert.StrToInt(md.LMY_SL3_NV, 0));
				}

				return res;
			}

			private K300RzfType md;
			private bool exists;
		}

		/// <summary>
		/// Returns entry from the hashtable keeping the latest market snapshot
		/// </summary>
		public Stock GetStock(int id)
		{
			return new Stock(id, this);
		}

		protected MarketSimulation.MarketData marketData;

		/// <summary>
		/// Collection of all traded symbols (different BNO_Num for TASE)
		/// I keep the last update data in this hash table
		/// </summary>
		protected new System.Collections.Hashtable securities;

	}
}
