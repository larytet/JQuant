
using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;

#if USEFMRSIM
using TaskBarLibSim;
#else
using TaskBarLib;
#endif

namespace JQuant
{
#if USEFMRSIM

	partial class Program
	{

		static MarketSimulationMaof marketSimulationMaof;
		MaofDataGeneratorLogFile dataMaofGenerator;

		protected static void GetMatchGroups(string pattern, string text, out System.Text.RegularExpressions.GroupCollection groups, out int matchesCount)
		{
			System.Text.RegularExpressions.Regex regex;
			groups = null;
			matchesCount = 0;
			text = text.ToUpper();

			regex = new System.Text.RegularExpressions.Regex(pattern);
			System.Text.RegularExpressions.MatchCollection matches = regex.Matches(text);

			do
			{
				matchesCount = matches.Count;
				if (matchesCount < 1)
				{
					break;
				}

				// get groups
				System.Text.RegularExpressions.Match match = matches[0];
				groups = match.Groups;
			}
			while (false);
		}

		protected static bool FindSecurity(System.Collections.Generic.Dictionary<string, int> names, string putcall, string strike, string month, out int id)
		{
			id = 0;
			bool res = false;

			// generate MAOF style name - something like "P01080DEC"
			strike = JQuant.OutputUtils.FormatField(strike, 5, '0');
			month = month.ToUpper();
			putcall = putcall.Substring(0, 1);
			putcall = putcall.ToUpper();
			string name = putcall + strike + month;

			if (names.ContainsKey(name))
			{
				id = names[name];
				res = true;
			}

			return res;
		}

		/// <summary>
		/// This is what I get: 'T25 P01080 DEC9'
		/// This is what I return: 'P01080DEC' or null
		/// </summary>
		protected static string convertBnoName(string BNO_NAME_E)
		{
			const string patternOption = MarketSimulationMaof.BNO_NAME_PATTERN_OPTION;

			System.Text.RegularExpressions.GroupCollection groups;
			int matchesCount;
			GetMatchGroups(patternOption, BNO_NAME_E, out groups, out matchesCount);
			string res = null;

			if (matchesCount == 1)
			{
				string putcall = groups[1].Captures[0].ToString();
				string strike = groups[2].Captures[0].ToString();
				string month = groups[3].Captures[0].ToString();

				res = putcall + strike + month;
				// System.Console.WriteLine("convertBnoName "+BNO_NAME_E+" to "+res);
			}
			else
			{
				// System.Console.WriteLine("Failed to parse BNO_NAME_E '"+BNO_NAME_E+"'");
			}

			return res;
		}

		/// <summary>
		/// This is a magic (aka Trust the force) method which goes through the 
		/// CLI command arguments and look for a ticker IDs. The emthod recognize 
		/// different formats of tickers. For example
		/// - the unique integer 80613003
		/// - partial integer 13003 (if unique)
		/// - description 'Call 1800 Nov'
		/// - description 'C1800Nov'
		/// - description 'Put1800 Nov'
		/// - full name 'TA9Z00960C'
		/// </summary>
		protected bool FindSecurity(string text, out int id)
		{
			id = 0;
			bool res = false;

			// get the list of securities
			int[] ids = marketSimulationMaof.GetSecurities();
			// my key is name of the option and my value is unique option Id (integer)
			// On TASE ID is an integer
			System.Collections.Generic.Dictionary<string, int> names = new System.Collections.Generic.Dictionary<string, int>(ids.Length);
			// i need an array (string) of IDs to look for patial integer IDs
			System.Text.StringBuilder idNames = new System.Text.StringBuilder(ids.Length * 10);
			// fill the dictionary and string of all IDs
			foreach (int i in ids)
			{
				MarketSimulationMaof.Option option = marketSimulationMaof.GetOption(i);
				string name = convertBnoName(option.GetName());
				if (name != null)
				{
					names.Add(name, i);
				}
				idNames.Append(i); idNames.Append(" ");
			}
			string idNamesStr = idNames.ToString();

			// look in the command for regexp jan|feb)($| +)' first
			// Other possibilities are: ' +([0-9]+) *([c,p]) *(jan|feb)($| +)'
			// the final case is any set of digits ' +([0-9]+)($| +)'
			const string monthPattern = "JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC";
			const string putcallPattern = "c|p|C|P|call|put|CALL|PUT|Call|Put]";
			const string pattern1 = " +(" + putcallPattern + ") *([0-9]+) *(" + monthPattern + ")($| +)";
			const string pattern2 = " +([0-9]+) *(" + putcallPattern + ") *(" + monthPattern + ")($| +)";
			const string pattern3 = " +([0-9]+)($| +)";
			System.Text.RegularExpressions.GroupCollection groups;
			int matchesCount;

			do
			{
				GetMatchGroups(pattern1, text, out groups, out matchesCount);
				if (matchesCount > 1)
				{
					System.Console.WriteLine("I expect one and only one match for '" + pattern1 + "' instead of " + matchesCount);
					break;
				}

				if (matchesCount == 1)
				{
					string putcall = groups[1].Captures[0].ToString(); // group[0] is reserved for the whole match
					string strike = groups[2].Captures[0].ToString();
					string month = groups[3].Captures[0].ToString();
					res = FindSecurity(names, putcall, strike, month, out id);
					break;
				}

				GetMatchGroups(pattern2, text, out groups, out matchesCount);
				if (matchesCount > 1)
				{
					System.Console.WriteLine("I expect one and only one match for '" + pattern2 + "' instead of " + matchesCount);
					break;
				}
				if (matchesCount == 1)
				{
					string strike = groups[2].Captures[0].ToString();  // group[0] is reserved for the whole match
					string putcall = groups[1].Captures[0].ToString();
					string month = groups[3].Captures[0].ToString();
					res = FindSecurity(names, putcall, strike, month, out id);
					break;
				}

				// finally i look just for any sequence of digits
				GetMatchGroups(pattern3, text, out groups, out matchesCount);
				if (matchesCount > 0)
				{
					string digits = groups[0].Captures[0].ToString(); // group[0] is reserved for the whole match
					int idxFirst = idNamesStr.IndexOf(digits);        // idNamesStr is a string containing all existing Ids followed by blank
					int idxSecond = idNamesStr.LastIndexOf(digits);
					string firstMatch = idNamesStr.Substring(idxFirst, idNamesStr.IndexOf(" ", idxFirst + 1) - idxFirst);
					string secondMatch = idNamesStr.Substring(idxSecond, idNamesStr.IndexOf(" ", idxSecond + 1) - idxSecond);
					if (idxFirst != idxSecond)
					{
						System.Console.WriteLine("I have at least two matches '" + firstMatch + "' and '" + secondMatch + "'");
						break;
					}
					// System.Console.WriteLine("firstMatch={0},secondMatch={1},digits={2}",firstMatch,secondMatch,digits);
					// i got a single match - convert to ID
					id = Int32.Parse(firstMatch);
					res = true;
					break;
				}

				res = false;
			}
			while (false);


			return res;
		}

		protected void debugMarketSimulationMaofCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			string cmd = "", arg1 = "", arg2 = "";
			switch (cmdArguments.Length)
			{
				case 0:
				case 1:
					iWrite.WriteLine("Usage: maof create <backlogfile> [speedup] | stop | start");
					break;

				case 2:
					cmd = cmdArguments[1].ToString().ToLower();
					break;

				case 3:
					cmd = cmdArguments[1].ToString().ToLower();
					arg1 = cmdArguments[2].ToString();
					break;

				case 4:
					cmd = cmdArguments[1].ToString().ToLower();
					arg1 = cmdArguments[2].ToString();
					arg2 = cmdArguments[3].ToString();
					break;
			}

			if (cmd == "stop")
			{
				if (dataMaofGenerator != default(MaofDataGeneratorLogFile))
				{
					dataMaofGenerator.Stop();
					// dataMaofGenerator.RemoveConsumer(marketSimulationMaof);
					marketSimulationMaof.Dispose();
					marketSimulationMaof = default(MarketSimulationMaof);
					dataMaofGenerator = default(MaofDataGeneratorLogFile);

					iWrite.WriteLine("maof stop called");
				}
				else
				{
					iWrite.WriteLine("No active simulation to stop.");
				}
			}
			else if (cmd == "start") // log file name
			{
				if (this.dataMaofGenerator != default(MaofDataGeneratorLogFile))
				{
					// call EventGenerator.Start() - start the data stream
					dataMaofGenerator.Start();
				}
				else
				{
					iWrite.WriteLine("Use 'create' first to create the market simulation");
				}
			}
			else if (cmd == "create") // log file name
			{
				string logfile = arg1;
				double speedup = JQuant.Convert.StrToDouble(arg2, 1.0);

				//if K300Class instance is not already initilazed, do it now
				if (this.dataMaofGenerator == default(MaofDataGeneratorLogFile))
				{
					this.dataMaofGenerator =
						new MaofDataGeneratorLogFile(logfile, speedup, 0);

					//I need a cast here, because MarketSimulationMaof expects parameter of type IProducer
					marketSimulationMaof = new MarketSimulationMaof();
					//                    dataMaofGenerator.AddConsumer(marketSimulationMaof);

					//                  marketSimulationMaof.EnableTrace(80608128, true);
					//                  marketSimulationMaof.EnableTrace(80616808, true);
					iWrite.WriteLine("Use 'start' to start the market simulation");
				}
				else    //for the moment I don't want the mess of running multiple simulations simultaneously.
				{
					iWrite.WriteLine("Maof simulation " + dataMaofGenerator.Name + "is already running.");
					iWrite.WriteLine("Only a single simulation at a time is possible.");
				}
			}
		}

		protected void debugMarketSimulationMaofStatMaof(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
		}

		protected void debugMarketSimulationMaofStatCore(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			int columnSize = 8;
			System.Collections.ArrayList names;
			System.Collections.ArrayList values;
			marketSimulationMaof.GetEventCounters(out names, out values);

			CommandLineInterface.printTableHeader(iWrite, names, columnSize);
			CommandLineInterface.printValues(iWrite, values, columnSize);
		}

		protected void debugMarketSimulationMaofStatBook(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			int[] ids = marketSimulationMaof.GetSecurities();   //get the list of securities
			int[] columns = new int[0];
			bool firstLoop = true;

			System.Collections.ArrayList names = new System.Collections.ArrayList();
			names.Add("Name");

			System.Array.Sort(ids);

			foreach (int id in ids)
			{
				MarketSimulationMaof.Option option = marketSimulationMaof.GetOption(id);
				System.Collections.ArrayList values = new System.Collections.ArrayList();


				JQuant.IResourceStatistics bids = marketSimulationMaof.GetOrderBook(id, JQuant.TransactionType.BUY);
				System.Collections.ArrayList bidValues;
				System.Collections.ArrayList bidNames;
				bids.GetEventCounters(out bidNames, out bidValues);

				JQuant.IResourceStatistics asks = marketSimulationMaof.GetOrderBook(id, JQuant.TransactionType.SELL);
				System.Collections.ArrayList askValues;
				System.Collections.ArrayList askNames;
				asks.GetEventCounters(out askNames, out askValues);

				// print table header if this is first loop
				if (firstLoop)
				{
					firstLoop = false;
					for (int i = 0; i < bidNames.Count; i++)
					{
						names.Add(bidNames[i]);
					}
					for (int i = 0; i < askNames.Count; i++)
					{
						names.Add(askNames[i]);
					}
					columns = JQuant.ArrayUtils.CreateInitializedArray(6, names.Count);
					columns[0] = 16;
					columns[1] = 10;
					columns[2] = 6;
					columns[3] = 6;
					columns[4] = 6;
					columns[5] = 10;
					columns[6] = 6;
					columns[7] = 6;
					columns[8] = 6;
					CommandLineInterface.printTableHeader(iWrite, names, columns);
				}

				values.Add(option.GetName());
				for (int i = 0; i < bidValues.Count; i++)
				{
					values.Add(bidValues[i]);
				}
				for (int i = 0; i < askValues.Count; i++)
				{
					values.Add(askValues[i]);
				}

				CommandLineInterface.printValues(iWrite, values, columns);
			}
		}

		protected void debugMarketSimulationMaofStatQueue(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			iWrite.WriteLine("Not supported");
		}

		protected void debugMarketSimulationMaofStatCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			string cmd = "core";

			if (dataMaofGenerator == default(MaofDataGeneratorLogFile)) // check if there active simulation to get data from 
			{                                                           // to prevent System.NullReferenceException
				iWrite.WriteLine("No active market simulations.");
				return;
			}

			if (cmdArguments.Length > 1)
			{
				cmd = (string)cmdArguments[1];
			}

			if (cmd == "maof")
			{
				debugMarketSimulationMaofStatMaof(iWrite, cmdName, cmdArguments);
			}
			else if (cmd == "core")
			{
				debugMarketSimulationMaofStatCore(iWrite, cmdName, cmdArguments);
			}
			else if (cmd == "book")
			{
				debugMarketSimulationMaofStatBook(iWrite, cmdName, cmdArguments);
			}
			else if (cmd == "queue")
			{
				debugMarketSimulationMaofStatQueue(iWrite, cmdName, cmdArguments);
			}
			else
			{
				iWrite.WriteLine("Only arguments maof, core, book, queue are supported");
			}
		}

		protected void debugMarketSimulationMaofSecsBook(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			int[] ids = marketSimulationMaof.GetSecurities();   //get the list of securities

			System.Collections.ArrayList names = new System.Collections.ArrayList();
			names.Add("Id");
			names.Add("Name");
			names.Add("Bid:PriceVolume");
			names.Add("Ask:PriceVolume");

			int[] columns = JQuant.ArrayUtils.CreateInitializedArray(6, names.Count);
			columns[0] = 10;
			columns[1] = 16;
			columns[2] = 30;
			columns[3] = 30;

			CommandLineInterface.printTableHeader(iWrite, names, columns);

			System.Array.Sort(ids);

			foreach (int id in ids)
			{
				MarketSimulationMaof.Option option = marketSimulationMaof.GetOption(id);
				System.Collections.ArrayList values = new System.Collections.ArrayList();

				MarketSimulation.OrderPair[] bids = marketSimulationMaof.GetOrderQueue(id, JQuant.TransactionType.BUY);
				MarketSimulation.OrderPair[] asks = marketSimulationMaof.GetOrderQueue(id, JQuant.TransactionType.SELL);

				values.Add(id);
				values.Add(option.GetName());
				values.Add(OrderBook2String(bids, 9));
				values.Add(OrderBook2String(asks, 9));

				CommandLineInterface.printValues(iWrite, values, columns);
			}
		}

		protected static string OrderPair2String(MarketSimulation.OrderPair op, int columnSize)
		{
			string res = "" + op.price + ":" + op.size + " ";
			res = OutputUtils.FormatField(res, columnSize);
			return res;
		}

		protected static string OrderBook2String(MarketSimulation.OrderPair[] book, int columnSize)
		{
			string res = "";

			for (int i = 0; i < book.Length; i++)
			{
				MarketSimulation.OrderPair op = book[i];
				res = res + OrderPair2String(op, columnSize);
			}

			return res;
		}


		protected void debugMarketSimulationMaofSecsMaof(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			int[] ids = marketSimulationMaof.GetSecurities();   //get the list of securities

			System.Collections.ArrayList names = new System.Collections.ArrayList();
			names.Add("Id");
			names.Add("Name");
			names.Add("Bid:PriceVolume");
			names.Add("Ask:PriceVolume");

			int[] columns = JQuant.ArrayUtils.CreateInitializedArray(6, names.Count);
			columns[0] = 10;
			columns[1] = 16;
			columns[2] = 30;
			columns[3] = 30;

			CommandLineInterface.printTableHeader(iWrite, names, columns);

			System.Array.Sort(ids);

			foreach (int id in ids)
			{
				MarketSimulationMaof.Option option = marketSimulationMaof.GetOption(id);
				System.Collections.ArrayList values = new System.Collections.ArrayList();

				values.Add(id);
				values.Add(option.GetName());
				values.Add(OrderBook2String(option.GetBookBid(), 9));
				values.Add(OrderBook2String(option.GetBookAsk(), 9));

				CommandLineInterface.printValues(iWrite, values, columns);
			}
		}

		/// <summary>
		/// This method do two things 
		/// - get list of securities from the MarketSimulationMaof
		/// For every security ask MarketSimulation.Core what the Core thinks about it. 
		/// </summary>
		protected void debugMarketSimulationMaofSecsCore(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			int[] ids = marketSimulationMaof.GetSecurities();   //get the list of securities

			System.Collections.ArrayList names = new System.Collections.ArrayList();
			names.Add("Id");
			names.Add("Name");
			names.Add("CoreId");
			names.Add("Bid:PriceVolume");
			names.Add("Ask:PriceVolume");
			names.Add("LastTrade");
			names.Add("LastTradeSize");
			names.Add("DayVolume");

			int[] columns = JQuant.ArrayUtils.CreateInitializedArray(6, names.Count);
			columns[0] = 9;
			columns[1] = 12;
			columns[2] = 9;
			columns[3] = 30;
			columns[4] = 30;

			CommandLineInterface.printTableHeader(iWrite, names, columns);

			System.Array.Sort(ids);

			foreach (int id in ids)
			{
				// i need MarketSimulationMaof.Option to show the name of the option
				// currently I take care only of options
				MarketSimulationMaof.Option option = marketSimulationMaof.GetOption(id);
				// get information kept in the MrketSimulation.Core
				MarketSimulation.MarketData md = marketSimulationMaof.GetSecurity(id);
				System.Collections.ArrayList values = new System.Collections.ArrayList();

				values.Add(id);
				values.Add(option.GetName());
				values.Add(md.id);
				values.Add(OrderBook2String(md.bid, 9));
				values.Add(OrderBook2String(md.ask, 9));
				values.Add(md.lastTrade);
				values.Add(md.lastTradeSize);
				values.Add(md.dayVolume);

				CommandLineInterface.printValues(iWrite, values, columns);
			}
		}

		protected void debugMarketSimulationMaofSecsQueue(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			if (marketSimulationMaof == default(MarketSimulationMaof)) // check if there active simulation to get data from 
			{
				iWrite.WriteLine("No active market simulations.");
				return;
			}

			iWrite.WriteLine("Not supported");
		}

		protected void debugMarketSimulationMaofSecsCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			string cmd = "maof";

			if (dataMaofGenerator == default(MaofDataGeneratorLogFile)) // check if there active simulation to get data from 
			{                                                           // to prevent System.NullReferenceException
				iWrite.WriteLine("No active market simulations.");
				return;
			}

			if (cmdArguments.Length > 1)
			{
				cmd = (string)cmdArguments[1];
			}

			if (cmd == "maof")
			{
				debugMarketSimulationMaofSecsMaof(iWrite, cmdName, cmdArguments);
			}
			else if (cmd == "core")
			{
				debugMarketSimulationMaofSecsCore(iWrite, cmdName, cmdArguments);
			}
			else if (cmd == "book")
			{
				debugMarketSimulationMaofSecsBook(iWrite, cmdName, cmdArguments);
			}
			else if (cmd == "queue")
			{
				debugMarketSimulationMaofSecsQueue(iWrite, cmdName, cmdArguments);
			}
			else
			{
				iWrite.WriteLine("Only arguments maof, core, book, queue are supported");
			}
		}

		protected void debugMarketSimulationMaofTraceCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			if (cmdArguments.Length < 2)
			{
				iWrite.WriteLine("Security ID is required");
				return;
			}
			if (marketSimulationMaof == default(MarketSimulationMaof))
			{
				iWrite.WriteLine("Create market simulation first");
				return;
			}

			int securityId = Convert.StrToInt((string)(cmdArguments[1]), 0);
			bool enable = false;

			if (cmdArguments.Length > 2)
			{
				enable = Boolean.Parse((string)(cmdArguments[2]));
			}
			else
			{
				enable = !(marketSimulationMaof.GetEnableTrace(securityId));
			}
			marketSimulationMaof.EnableTrace(securityId, enable);
		}


		public class WatchlistCallback
		{
			public WatchlistCallback(IWrite iWrite)
			{
				this.iWrite = iWrite;
			}

			/// <summary>
			/// called by MarketSimulation when a change is in the status of the security 
			/// </summary>
			public void callback(MarketSimulation.MarketData md)
			{
				int id = md.id;
				MarketSimulationMaof.Option option = marketSimulationMaof.GetOption(id);
				string optionName = option.GetName();

				// print everything out - name, bids, asks
				MarketSimulation.OrderPair[] bids = md.bid;
				MarketSimulation.OrderPair[] asks = md.ask;

				System.Collections.ArrayList values = new System.Collections.ArrayList();

				values.Add(md.tick);
				values.Add(id);
				values.Add(optionName);
				values.Add(md.lastTrade);
				values.Add(md.dayVolume);
				values.Add(OrderBook2String(bids, 9));
				values.Add(OrderBook2String(asks, 9));

				CommandLineInterface.printValues(iWrite, values, this.columns);
			}

			public void printLegend()
			{
				System.Collections.ArrayList values = new System.Collections.ArrayList();

				values.Add("Tick");
				values.Add("Id");
				values.Add("Name");
				values.Add("LastTradeSize");
				values.Add("DayVolume");
				values.Add("Bids");
				values.Add("Asks");

				CommandLineInterface.printValues(iWrite, values, this.columns);
			}

			public void printList()
			{
				int[] list = marketSimulationMaof.WatchList();
				for (int i = 0; i < list.Length; i++)
				{
					int id = list[i];
					MarketSimulationMaof.Option option = marketSimulationMaof.GetOption(id);
					string optionName = option.GetName();
					System.Collections.ArrayList values = new System.Collections.ArrayList();
					values.Add(id);
					values.Add(optionName);
					CommandLineInterface.printValues(iWrite, values, this.columns);
				}
			}
			protected IWrite iWrite;
			protected int[] columns = { 8, 10, 12, 6, 6, 30, 30 };
		}

		protected WatchlistCallback watchlistCallback;
		protected void debugMarketSimulationMaofWatchCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			if (marketSimulationMaof == default(MarketSimulationMaof)) // check if there active simulation to get data from 
			{
				iWrite.WriteLine("No active market simulations.");
				return;
			}
			if (watchlistCallback == null) // create a watchlist callback in the first call
			{
				watchlistCallback = new WatchlistCallback(iWrite);
			}
			if (cmdArguments.Length == 2)
			{
				// argument is legend or list
				string legendList = cmdArguments[1].ToString();
				legendList = legendList.ToUpper();
				if (legendList.CompareTo("LEGEND") == 0)
				{
					watchlistCallback.printLegend();
				}
				else if (legendList.CompareTo("LIST") == 0)
				{
					watchlistCallback.printList();
				}
				else
				{
					iWrite.WriteLine("Use commands legend or list");
				}
				return;
			}
			if (cmdArguments.Length < 3)
			{
				iWrite.WriteLine("At least two arguments are required");
				return;
			}
			int id;
			bool res = FindSecurity(cmdName, out id);
			if (!res)
			{
				iWrite.WriteLine("Unknown security in the command " + cmdName);
				return;
			}

			// first argument is add or rmv
			string addRmv = cmdArguments[1].ToString();
			addRmv = addRmv.ToUpper();
			if (addRmv.CompareTo("ADD") == 0)
			{
				marketSimulationMaof.AddWatch(id, watchlistCallback.callback);
			}
			else
			{
				marketSimulationMaof.RemoveWatch(id);
			}
		}

		protected void placeOrderCallback(MarketSimulation.ReturnCode errorCode, MarketSimulation.ISystemLimitOrder lo, int quantity)
		{
			MarketSimulationMaof.Option option = marketSimulationMaof.GetOption(lo.SecurityId);
			string optionName = option.GetName();
			if (errorCode == MarketSimulation.ReturnCode.Fill)
			{
				System.Console.WriteLine("Tick {6} Order {0} {5} id {1} quantity {3} price {4} got fill at price {2}",
										 optionName, lo.Id, lo.FillPrice, quantity, lo.Price, lo.SecurityId, lo.FillTick);
			}
			else
			{
				System.Console.WriteLine("Tick {5} Order {0} id {1} price {2} quantity {3} failed on {4}",
										 optionName, lo.Id, lo.Price, quantity, errorCode.ToString(), lo.FillTick);
			}
		}

		protected void debugMarketSimulationMaofPlaceOrderCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			if (marketSimulationMaof == default(MarketSimulationMaof)) // check if there active simulation to get data from 
			{
				iWrite.WriteLine("No active market simulations.");
				return;
			}

			int id;
			bool res = FindSecurity(cmdName, out id);
			if (!res)
			{
				iWrite.WriteLine("Unknown security in the command " + cmdName);
				return;
			}

			// i got security ID
			bool buyOrder = (cmdArguments[1].ToString().ToUpper().CompareTo("BUY") == 0);
			bool sellOrder = (cmdArguments[1].ToString().ToUpper().CompareTo("SELL") == 0);
			if (!buyOrder && !sellOrder)
			{
				iWrite.WriteLine("Use words buy or sell to specify the order type");
				return;
			}
			if (buyOrder && sellOrder)
			{
				iWrite.WriteLine("Internal error: both buy and sell in " + cmdArguments[1]);
				return;
			}


			// are there three and only three numbers in the command line ?
			const string patternNumbers = ".+[0-9]+.+[0-9]+ +[0-9]+";
			System.Text.RegularExpressions.Regex regexNumbers = new System.Text.RegularExpressions.Regex(patternNumbers);
			System.Text.RegularExpressions.MatchCollection matches = regexNumbers.Matches(cmdName);
			if (matches.Count != 1)
			{
				iWrite.WriteLine("Three and only three numbers - security ID, limit price and quantiy are allowed. I got '" + cmdName + "'");
				return;
			}

			// last arguments are price and quantity
			string limitPriceStr = cmdArguments[cmdArguments.Length - 2].ToString();
			string quantintyStr = cmdArguments[cmdArguments.Length - 1].ToString();
			int limitPrice = Int32.Parse(limitPriceStr);
			int quantity = Int32.Parse(quantintyStr);
			if (limitPrice == 0)
			{
				iWrite.WriteLine("Failed to parse limit price " + limitPriceStr);
				return;
			}
			if (quantity == 0)
			{
				iWrite.WriteLine("Failed to parse quantinty " + quantintyStr);
				return;
			}

			TransactionType transaction;
			if (buyOrder)
			{
				transaction = TransactionType.BUY;
			}
			else
			{
				transaction = TransactionType.SELL;
			}
			MarketSimulation.ReturnCode errorCode;
			MarketSimulation.ISystemLimitOrder order = marketSimulationMaof.CreateOrder(id, limitPrice, quantity, transaction, placeOrderCallback);
			res = marketSimulationMaof.PlaceOrder(order, out errorCode);
			if (!res)
			{
				iWrite.WriteLine("Failed to place order error=" + errorCode);
			}
		}

		protected void debugMarketSimulationMaofCancelOrderCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			if (marketSimulationMaof == default(MarketSimulationMaof)) // check if there active simulation to get data from 
			{
				iWrite.WriteLine("No active market simulations.");
				return;
			}

			iWrite.WriteLine("Not supported");
		}


		protected void LoadCommandLineInterface_ms()
		{
			Menu menuMarketSim = cli.RootMenu.AddMenu("ms", "Market simulation",
								   " Run market simulation");

			menuMarketSim.AddCommand("maof",
									"Run MarketSimulationMaof. Usage: maof create <backlogfile> [speedup] | start | stop",
									"Create Maof Event Generator, connect to the Maof Market simulation",
									debugMarketSimulationMaofCallback
									);

			menuMarketSim.AddCommand("stat",
									"Show statistics for the running market simulation. Usage: stat core|book|queue",
									"Display number of events, number of placed orders at different layers of the market simulaiton",
									debugMarketSimulationMaofStatCallback
									);

			menuMarketSim.AddCommand("secs",
									"Show list of securities. Usage secs maof|core|book|queue",
									"Display list of securities including number of orders",
									debugMarketSimulationMaofSecsCallback
									);


			menuMarketSim.AddCommand("trace",
									"Enable trace for specific security. Usage: trace <securityId> <enable|disable>",
									"Enable/disable debug trace for specific security. " + "Security identifier can be a unique number, \n" +
									 "or things like 'C1800Oct', 'call 1800 Oct', 'call1800Oct', etc.  ",
									debugMarketSimulationMaofTraceCallback
									);


			menuMarketSim.AddCommand("w",
									"Add security to the watch list. Usage: w [add|rmv|legend|list] <securityId> ",
									"Add (remove) specific security to (from) watch list, pint list of watched securities.\n"
									 + "Security identifier can be a unique number, \n" +
									 "or things like 'C1800Oct', 'call 1800 Oct', 'call1800Oct', etc.  ",
									debugMarketSimulationMaofWatchCallback
									);

			menuMarketSim.AddCommand("p",
									"Place order. Usage: p [buy|sell] <securityId>  [limit] [quantity]",
									"Place order for specific security. " + "Security identifier can be a unique number, " +
									 "or things like 'C1800Oct', 'call 1800 Oct', 'call1800Oct', etc.  ",
									debugMarketSimulationMaofPlaceOrderCallback
									);

			menuMarketSim.AddCommand("c",
									"Cancel order. Usage: c <securityId>",
									"Cancels previously placed order. " + "Security identifier can be a unique number, \n" +
									 "or things like 'C1800Oct', 'call 1800 Oct', 'call1800Oct', etc.  ",
									debugMarketSimulationMaofCancelOrderCallback
									);
		}

	}
#endif

}
