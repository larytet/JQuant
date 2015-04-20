
using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;

//support the simulation mode
#if USEFMRSIM
using TaskBarLibSim;
#else
using TaskBarLib;
#endif

//temporary
using System.Windows.Forms;


namespace JQuant
{
	partial class Program
	{
		//many methods require being logged in, this is the check
		protected bool IsLogged(IWrite iWrite)
		{
			bool rc = false;
			if (this.fmrConection == null)
			{
				iWrite.WriteLine(Environment.NewLine
					+ "WARNING! You're not logged in. Please log in first!");
			}

			//then check if connection is OK. TaksBar takes care of keeping it alive
			//in all times. If loginStatus != LoginSessionActive - there probably a big trouble
			else if (this.fmrConection.loginStatus != LoginStatus.LoginSessionActive)
			{
				iWrite.WriteLine(Environment.NewLine
					+ "WARNING! Your login status is "
					+ this.fmrConection.loginStatus.ToString()
					+ "Please check your connection!");
			}
			else rc = true;
			return rc;
		}


		protected void GetSH161Data(IWrite iWrite, string filename)
		{
			SH161DataLogger dl = new SH161DataLogger(filename);
			dl.GetAndLogSH161Data(fmrConection.GetSessionId());
		}

		protected void printIntStatisticsHeader(IWrite iWrite)
		{
			iWrite.WriteLine(OutputUtils.FormatField("Name", 8) +
						 OutputUtils.FormatField("Mean", 8) +
						 OutputUtils.FormatField("Ready", 8) +
						 OutputUtils.FormatField("Size", 8) +
						 OutputUtils.FormatField("Count", 8)
						);
			iWrite.WriteLine("----------------------------------------------------------------");
		}

		protected void printIntStatistics(IWrite iWrite, IntStatistics statistics)
		{
			iWrite.WriteLine(OutputUtils.FormatField(statistics.Name, 8) +
						 OutputUtils.FormatField(statistics.Mean, 8) +
						 OutputUtils.FormatField(statistics.Full().ToString(), 8) +
						 OutputUtils.FormatField(statistics.Size, 8) +
						 OutputUtils.FormatField(statistics.Count, 8)
						);
		}

		protected void printIntMaxMinHeader(IWrite iWrite)
		{
			iWrite.WriteLine(OutputUtils.FormatField("Name", 8) +
						 OutputUtils.FormatField("Max", 8) +
						 OutputUtils.FormatField("Min", 8) +
						 OutputUtils.FormatField("Ready", 8) +
						 OutputUtils.FormatField("Size", 8) +
						 OutputUtils.FormatField("Count", 8)
						);
			iWrite.WriteLine("----------------------------------------------------------------");
		}

		protected void printIntMaxMin(IWrite iWrite, IntMaxMin maxMin)
		{
			iWrite.WriteLine(OutputUtils.FormatField(maxMin.Name, 8) +
						 OutputUtils.FormatField(maxMin.Max, 8) +
						 OutputUtils.FormatField(maxMin.Min, 8) +
						 OutputUtils.FormatField(maxMin.Full().ToString(), 8) +
						 OutputUtils.FormatField(maxMin.Size, 8) +
						 OutputUtils.FormatField(maxMin.Count, 8)
						);
		}

		protected void debugLogSH161DataCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			string filename = Resources.CreateLogFileName("sh161_", LogType.CSV);
			iWrite.WriteLine("SH161 Log File: " + filename);
			GetSH161Data(iWrite, filename);
		}

		protected void debugGetAS400DTCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			int ltncy;
			DateTime dt;

			//ping every 2 seconds, 60 times, write the output to the console
			//TODO - write to a file instead of console, make it a separate low priority thread
			for (int i = 0; i < 60; i++)
			{
				FMRShell.AS400Synch.Ping(out dt, out ltncy);
				Console.WriteLine(FMRShell.AS400Synch.ToShortCSVString(dt, ltncy));
				Thread.Sleep(2000);
			}
		}

		protected void debugFMRPingCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			int argsNum = cmdArguments.Length;
			string[] args = (string[])cmdArguments;
			FMRShell.FMRPing fmrPing = FMRShell.FMRPing.GetInstance();

			switch (argsNum)
			{
				case 1:
					fmrPing.Start();
					iWrite.WriteLine("FMRPing started");
					break;

				default:
					string arg = args[1];
					if (arg.Equals("login"))
					{
						fmrPing.SendLogin();
						iWrite.WriteLine("FMRPing Login");
					}
					if (arg.Equals("logout"))
					{
						fmrPing.SendLogout();
						iWrite.WriteLine("FMRPing Logout");
					}
					if (arg.Equals("stat"))
					{
						iWrite.WriteLine("Failed " + fmrPing.CountPingFailed + " from " + (fmrPing.CountPingOk + fmrPing.CountPingFailed));
						printIntStatisticsHeader(iWrite);
						printIntStatistics(iWrite, fmrPing.Statistics2min);
						printIntStatistics(iWrite, fmrPing.Statistics10min);
						printIntStatistics(iWrite, fmrPing.Statistics1hour);
						iWrite.WriteLine();
						printIntMaxMinHeader(iWrite);
						printIntMaxMin(iWrite, fmrPing.MaxMin2min);
						printIntMaxMin(iWrite, fmrPing.MaxMin10min);
						printIntMaxMin(iWrite, fmrPing.MaxMin1hour);
					}
					if (arg.Equals("kill"))
					{
						fmrPing.Dispose();
					}
					break;
			}
		}

		protected void debugThreadPoolTestCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			int maxJobs = 5;
			JQuant.ThreadPool threadPool = new JQuant.ThreadPool("test", 1, maxJobs, ThreadPriority.Lowest);

			threadpoolTestTicks = new long[maxJobs];
			long tick = DateTime.Now.Ticks;
			for (int i = 0; i < maxJobs; i++)
			{
				threadpoolTestTicks[i] = tick;
			}

			for (int i = 0; i < maxJobs; i++)
			{
				threadPool.PlaceJob(ThreadPoolJobEnter, ThreadPoolJobDone, i);
			}
			Thread.Sleep(500);

			debugThreadPoolShowCallback(iWrite, cmdName, cmdArguments);
			threadPool.Dispose();

			for (int i = 0; i < threadpoolTestTicks.Length; i++)
			{
				iWrite.WriteLine("ThreadPoolJob done  idx =" + i + ", time = " + (double)threadpoolTestTicks[i] / (double)(10 * 1) + " micros");
			}

		}

		protected void debugThreadPoolShowCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			debugPrintResourcesNameAndStats(iWrite, Resources.ThreadPools);
		}

		protected void debugPrintResourcesNameAndStats(IWrite iWrite, System.Collections.ArrayList list)
		{
			int entry = 0;
			int columnSize = 8;

			bool isEmpty = true;

			iWrite.WriteLine();

			foreach (INamedResource resNamed in list)
			{
				isEmpty = false;

				IResourceStatistics resStat = (IResourceStatistics)resNamed;

				System.Collections.ArrayList names;
				System.Collections.ArrayList values;
				resStat.GetEventCounters(out names, out values);

				if (entry == 0)
				{
					names.Insert(0, "Name");
					CommandLineInterface.printTableHeader(iWrite, names, columnSize);
				}
				values.Insert(0, OutputUtils.FormatField(resNamed.Name, columnSize));
				CommandLineInterface.printValues(iWrite, values, columnSize);

				entry++;

			}
			if (isEmpty)
			{
				System.Console.WriteLine("Table is empty - no resources registered");
			}
		}

		protected void debugVerifierShowCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			System.Collections.ArrayList names;
			System.Collections.ArrayList values;
			int entry = 0;
			int columnSize = 12;

			bool isEmpty = true;

			iWrite.WriteLine();

			foreach (IResourceDataVerifier verifier in Resources.Verifiers)
			{
				verifier.GetEventCounters(out names, out values);
				isEmpty = false;

				if (entry == 0)
				{
					names.Insert(0, "Name");
					CommandLineInterface.printTableHeader((JQuant.IWrite)this, names, columnSize);
				}
				values.Insert(0, OutputUtils.FormatField(verifier.Name, columnSize));
				CommandLineInterface.printValues((JQuant.IWrite)this, values, columnSize);

				entry++;

			}
			if (isEmpty)
			{
				iWrite.WriteLine("No verifiers");
			}

		}


		protected void debugPoolShowCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			debugPrintResourcesNameAndStats(iWrite, Resources.Pools);
		}

		protected void debugLoggerShowCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			iWrite.WriteLine(
				OutputUtils.FormatField("Name", 10) +
				OutputUtils.FormatField("Triggered", 10) +
				OutputUtils.FormatField("Logged", 10) +
				OutputUtils.FormatField("Dropped", 10) +
				OutputUtils.FormatField("Log type", 10) +
				OutputUtils.FormatField("Latest", 24) +
				OutputUtils.FormatField("Oldest", 24) +
				OutputUtils.FormatField("Stamped", 10)
			);
			iWrite.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
			bool isEmpty = true;
			foreach (ILogger logger in Resources.Loggers)
			{
				isEmpty = false;
				iWrite.WriteLine(
					OutputUtils.FormatField(logger.GetName(), 10) +
					OutputUtils.FormatField(logger.GetCountTrigger(), 10) +
					OutputUtils.FormatField(logger.GetCountLog(), 10) +
					OutputUtils.FormatField(logger.GetCountDropped(), 10) +
					OutputUtils.FormatField(logger.GetLogType().ToString(), 10) +
					OutputUtils.FormatField(logger.GetLatest().ToString(), 24) +
					OutputUtils.FormatField(logger.GetOldest().ToString(), 24) +
					OutputUtils.FormatField(logger.TimeStamped().ToString(), 10)
				);

			}
			if (isEmpty)
			{
				iWrite.WriteLine("No loggers");
			}
		}


		protected void debugProducerShowCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			System.Collections.ArrayList names;
			System.Collections.ArrayList values;
			int entry = 0;
			int columnSize = 12;

			bool isEmpty = true;

			iWrite.WriteLine();

			foreach (IResourceProducer producer in Resources.Producers)
			{
				producer.GetEventCounters(out names, out values);
				isEmpty = false;

				if (entry == 0)
				{
					names.Insert(0, "Name");
					CommandLineInterface.printTableHeader((JQuant.IWrite)this, names, columnSize);
				}
				values.Insert(0, OutputUtils.FormatField(producer.Name, columnSize));
				CommandLineInterface.printValues((JQuant.IWrite)this, values, columnSize);

				entry++;

			}
			if (isEmpty)
			{
				iWrite.WriteLine("No producers");
			}

		}

		protected class HttpRegGetTimeData
		{
			public HttpRegGetTimeData(double maof)
			{
				System.DateTime dtNow = System.DateTime.Now;
				hour = dtNow.Hour;
				minute = dtNow.Minute;
				second = dtNow.Second;
				year = dtNow.Year;
				month = dtNow.Month;
				day = dtNow.Day;

				this.maof = maof+randomGenerator.Next(0,300);

				// modify optionsChanged - make the client (browser) to call GetOptions()
				optionsChanged = CHANGES;
				CHANGES++;
			}

			public double maof;
			public int hour;
			public int minute;
			public int second;
			public int year;
			public int month;
			public int day;
			public long optionsChanged;
			protected static long CHANGES = 0;
			static System.Random randomGenerator = new System.Random();
		}

		private void httpReqGetTime(string request, System.Net.Sockets.NetworkStream networkStream, out bool stream)
		{
			// this is not a stream - let HTTP server close the connection when I am done
			stream = false;

			// i ignore the request itself - no arguments

			HttpRegGetTimeData timedate = new HttpRegGetTimeData(500.0);

			// i have to create a packet containing JSON formated data 
			string s = JQuant.OutputUtils.GetJSON(timedate, "timedate");
			// System.Console.WriteLine(s);
			System.DateTime dtNow = System.DateTime.Now;
			byte[] data = System.Text.Encoding.ASCII.GetBytes(s);
			JQuantHttp.Http.SendHeader(networkStream, dtNow, false, data.Length, JQuantHttp.Http.GetMimeType(".html"));
			JQuantHttp.Http.SendOctets(networkStream, data);

			return;
		}

		private void httpReqGetHttpStat(string request, System.Net.Sockets.NetworkStream networkStream, out bool stream)
		{
			// this is not a stream - let HTTP server close the connection when I am done
			stream = false;

			// i ignore the request itself - no arguments

			// get traffic countres from the HTTP server (I assume that there is only one server)
			IResourceStatistics resStat = JQuantHttp.Http.GetIResourceStatistics();

			// i have to create a packet containing JSON formated data 
			string s = JQuant.OutputUtils.GetJSON(resStat);
			System.DateTime dtNow = System.DateTime.Now;
			byte[] data = System.Text.Encoding.ASCII.GetBytes(s);
			JQuantHttp.Http.SendHeader(networkStream, dtNow, false, data.Length, JQuantHttp.Http.GetMimeType(".html"));
			JQuantHttp.Http.SendOctets(networkStream, data);

			return;
		}

		protected class HttpRegGetOptionData
		{

			/// <summary>
			/// This constructor is used only for generation of random data 
			/// </summary>
			public HttpRegGetOptionData(double strike, int id, bool isPut)
			{
				this.optionStrike = strike;
				ask = new LimitOrderPair[3];
				bid = new LimitOrderPair[3];
				FillOrderPairs(ask);
				FillOrderPairs(bid);
				// for this specific case I am buying options - thus minus
				this.pendingOrder = GetPendingOrder(ask, bid);
				this.optionId = id;
				this.isPut = 0;
				if (isPut)
					this.isPut = 1;
			}

			/// <summary>
			/// Fill order pairs (size, price) with random data 
			/// </summary>
			private static void FillOrderPairs(LimitOrderPair[] orderPairs)
			{
				int idx;
				int count = orderPairs.Length;
				for (idx = 0;idx < count;idx++)
				{
					int price = randomGenerator.Next(500, 1000);
					int size = randomGenerator.Next(3, 50);
					orderPairs[idx] = new LimitOrderPair(price, size);
				}
				
			}

			/// <summary>
			/// Set random pending order 
			/// </summary>
			private static double GetPendingOrder(LimitOrderPair[] asks, LimitOrderPair[] bids)
			{
				double result = 0;

				int choice = randomGenerator.Next(0, 20);

				if (choice == 1) result = -asks[0].price;
				if (choice == 2) result = bids[0].price;

				return result;
			}

			public double optionStrike;
			public double pendingOrder;
			public int optionId;
			public LimitOrderPair[] ask;
			public LimitOrderPair[] bid;
			public int isPut;  // non zero if put
			static System.Random randomGenerator = new System.Random();

		}

		/// <summary>
		/// Generate random options
		/// </summary>
		/// <param name="count">
		/// A <see cref="System.Int32"/>
		/// Number of options to generate
		/// </param>
		/// <returns>
		/// A <see cref="HttpRegGetOptionData[]"/>
		/// </returns>
		protected static HttpRegGetOptionData[] GenerateRadomOptions(int count)
		{
			int idx;
			HttpRegGetOptionData[] options = new HttpRegGetOptionData[count];

			for (idx = 0;idx < count;idx++)
			{
				options[idx] = new Program.HttpRegGetOptionData((12-idx/2)*100, idx, ((idx & 1) == 0));
			}

			return options;
		}

		private void httpReqGetOptions(string request, System.Net.Sockets.NetworkStream networkStream, out bool stream)
		{
			stream = false;
		}

		protected void debugHttpStart(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			// i want to add a couple of HTTP request handlers in case they are not there already
			JQuantHttp.Http.AddRequestHandler("getTime", httpReqGetTime);
			JQuantHttp.Http.AddRequestHandler("getHttpStat", httpReqGetHttpStat);
		}

		protected void debugHttpStop(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			JQuantHttp.Http httpServer = Program.httpServer;
			if (httpServer != default(JQuantHttp.Http))
			{
				httpServer.Stop();
			}
		}

		protected void debugHttpStat(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			IResourceStatistics resStat = JQuantHttp.Http.GetIResourceStatistics();

			System.Collections.ArrayList names;
			System.Collections.ArrayList values;
			resStat.GetEventCounters(out names, out values);

			int columnSize = 8;
			CommandLineInterface.printTableHeader(iWrite, names, columnSize);
			CommandLineInterface.printValues(iWrite, values, columnSize);
		}

		#region Orders Stream;
		//I test the TaskBar.UserClass orders stream (info on submitted orders and fills) functions

		protected void debugStartOrderStream(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			if (IsLogged(iWrite))
			{
				TradeType trtyp = TradeType.ALLTradeType;
				string streamtype = "ALL";

				bool errParams = false;

				//check what args the user provided. 
				//basically he can decide whether to get info on RZ or MF orders or both.
				if (cmdArguments.Length > 2)
				{
					iWrite.WriteLine("invalid sintax" + Environment.NewLine + "type 'start' + MF|RZ|ALL. typing 'start' = 'start ALL'");
					errParams = true;
				}
				else if (cmdArguments.Length == 2)
				{
					switch (cmdArguments[1].ToString().ToLower())
					{
						case ("mf"):
							trtyp = TradeType.MF;
							streamtype = "MF";
							break;
						case ("rz"):
							trtyp = TradeType.RZ;
							streamtype = "RZ";
							break;
						case ("all"):
							trtyp = TradeType.ALLTradeType;
							break;
						default:
							iWrite.WriteLine("invalid argument supplied. type either 'MF' or 'RZ'. For both - type 'ALL' or nothing");
							errParams = true;
							break;
					}
				}
				else trtyp = TradeType.ALLTradeType;
				if (!errParams)
				{
					int rc = this.fmrConection.userClass.OrdersStreamStart(
						fmrConection.GetSessionId(),
						fmrConection.Parameters.Account,
						fmrConection.Parameters.Branch,
						trtyp
						);
					//check the return code, report tothe operator
					switch (rc)
					{
						case (0):
							iWrite.WriteLine(streamtype + " order stream successflly started, rc = " + rc.ToString());
							break;
						default:
							iWrite.WriteLine(streamtype + " order stream start failed. rc = " + rc.ToString());
							break;
					}
				}
			}
		}

		protected void debugListOrders(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			if (IsLogged(iWrite))
			{
				int rc;
				Array res = null;
				string lstTime = "00000000";

				//parse user's parameters
				if (cmdArguments.Length != 2)
					iWrite.WriteLine("Incorrect command. Type 'o' + 'mf | rz '");
				else if (cmdArguments.Length == 2)
				{
					switch (cmdArguments[1].ToString().ToLower())
					{
						case ("mf"):
							rc = fmrConection.userClass.GetOrdersMF(
								fmrConection.GetSessionId(),
								out res,
								fmrConection.Parameters.Account,
								fmrConection.Parameters.Branch,
								ref lstTime);
							iWrite.WriteLine("rc = " + rc + "last time = " + lstTime);
							break;
						case ("rz"):
							rc = fmrConection.userClass.GetOrdersRZ(
								fmrConection.GetSessionId(),
								out res,
								fmrConection.Parameters.Account,
								fmrConection.Parameters.Branch,
								ref lstTime);
							iWrite.WriteLine("rc = " + rc + " last time = " + lstTime);
							break;
						default:
							iWrite.WriteLine("Incorrect data type. Type 'mf | rz '");
							break;
					}
				}

			}
		}

		protected void debugStopOrderStream(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			if (IsLogged(iWrite))
			{
				//first collect all the needed parameters from the Connection object:
				int sessionid = this.fmrConection.GetSessionId();
				string account = this.fmrConection.Parameters.Account;
				string branch = this.fmrConection.Parameters.Branch;
				TradeType trtyp = TradeType.ALLTradeType;
				string streamtype = "ALL";

				bool errParams = false;

				//check what args the user provided. 
				//basically he can decide whether to get info on RZ or MF orders or both.
				if (cmdArguments.Length > 2)
				{
					iWrite.WriteLine("invalid sintax" + Environment.NewLine + "type 'stop' + MF|RZ|ALL. typing 'start' = 'start ALL'");
					errParams = true;
				}
				else if (cmdArguments.Length == 2)
				{
					switch (cmdArguments[1].ToString().ToLower())
					{
						case ("mf"):
							trtyp = TradeType.MF;
							streamtype = "MF";
							break;
						case ("rz"):
							trtyp = TradeType.RZ;
							streamtype = "RZ";
							break;
						case ("all"):
							trtyp = TradeType.ALLTradeType;
							break;
						default:
							iWrite.WriteLine("invalid argument supplied. type either 'MF' or 'RZ'. For both - type 'ALL' or nothing");
							errParams = true;
							break;
					}
				}
				else trtyp = TradeType.ALLTradeType;
				if (!errParams)
				{
					int rc = this.fmrConection.userClass.OrdersStreamStop(sessionid, account, branch, trtyp);
					//check the return code, report tothe operator
					switch (rc)
					{
						case (0):
							iWrite.WriteLine(streamtype + " order stream successflly stopped, rc = " + rc.ToString());
							break;
						default:
							iWrite.WriteLine(streamtype + " order stream stop failed. rc = " + rc.ToString());
							break;
					}
				}
			}
		}

		public class PermTypeToString : StructToString<UserPasswordType>
		{
			public PermTypeToString(string delimiter)
				: base(delimiter)
			{
			}
		}

		protected void debugUserPermissions(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			System.Array res = null;
			if (IsLogged(iWrite))
			{
				int rc = this.fmrConection.userClass.GetUserPermissions(this.fmrConection.GetSessionId(), out res);
				PermTypeToString print = new PermTypeToString(" ");
				int len = res.Length;
				//int i = 0;

				if (res != null)
				{
					iWrite.WriteLine(print.Legend);

					foreach (UserPasswordType p in res)
					{
						iWrite.Write(p.SUG_RC + " " +
							p.USR_NAME + " " +
							p.USR_PW + " " +
							p.Branch + " " +
							p.Sug + " " +
							p.MFZL_ID + " " +
							p.TIK_N + " " +
							p.ID_N + " " +
							p.APPL_N + " " +
							p.DEVICE_N + " " +
							p.AUTH_LVL + " " +
							p.FIELD + " " +
							p.OPERATOR + " " +
							p.val + " " +
							p.UPD_DAT + " " +
							p.END_DAT + " " +
							p.UPD_USR_N + " " +
							p.FROM_USR_N + " " +
							p.FIL + " " +
							Environment.NewLine);

						/*if (p.FIELD != "")
						{
							print.Init((UserPasswordType)res.GetValue(i));
							iWrite.WriteLine(print.Values);
						}
						i++;*/
					}

					//for some reason this didn't work, so i replaced it with the code above
					//in any case it not well understood what all those values say))
					/*
					 for (int i = 0; i < len; i++)
					 {
						 if (res.GetValue(i) != null)
						 {
							 print.Init((UserPasswordType)res.GetValue(i));
							 iWrite.WriteLine(print.Values);
						 }
					 }*/

				}
			}
		}

		public class AccountTypeToString : StructToString<AccountType>
		{
			public AccountTypeToString(string delimiter)
				: base(delimiter)
			{
			}

		}

		protected void debugUserAcc(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			AccountTypeToString ac = new AccountTypeToString(",");
			if (IsLogged(iWrite))
			{
				System.Array res = default(Array);
				string msg = "";
				string strQuery = "";
				int nRecords = fmrConection.userClass.GetAccounts(
					fmrConection.GetSessionId(),
					strQuery,
					ref res,
					ref msg);

				iWrite.WriteLine("collected " + nRecords + " records");
				iWrite.WriteLine();
				iWrite.WriteLine(ac.Legend);
				for (int i = 0; i < nRecords; i++)
				{
					ac.Init((AccountType)res.GetValue(i));
					iWrite.WriteLine(ac.Values);
				}
				iWrite.WriteLine();
			}
		}


		#endregion;

		#region OFSM callbacks;

		protected void debugOFSM_Create(IWrite iwrite, string cmdName, object[] cmdArguments)
		{
			if (IsLogged(iwrite))
			{
				if (_rzFSM != default(FMRShell.RezefOrderFSM))
				{
					iwrite.WriteLine("Rezef Order FSM already exists");
				}
				else
				{
					_rzFSM = new FMRShell.RezefOrderFSM(this.fmrConection);
					_rzFSM.Start();
				}
			}
		}

		protected void debugOFSM_Stop(IWrite iwrite, string cmdName, object[] cmdArguments)
		{
			if (IsLogged(iwrite))
			{
				if (_rzFSM == default(FMRShell.RezefOrderFSM))
				{
					iwrite.WriteLine("Can't stop Rezef Order FSM that doesn't exist");
				}
				else
				{
					_rzFSM.Dispose();
					_rzFSM = default(FMRShell.RezefOrderFSM);
				}
			}
		}

		protected void debugOFSM_Send(IWrite iwrite, string cmdName, object[] cmdArguments)
		{
			//usage:    send B/S   id   Q    P
			//          send B   629014 10 20000

			//parse the order:
			if (cmdArguments.Length != 5)
			{
				iwrite.WriteLine("Missing or incorrect args to send command");
				iwrite.WriteLine("type: send B|S id Q P");
			}
			else
			{
				LMTOrderParameters p = new LMTOrderParameters();
				bool _continue = false;
				switch (cmdArguments[1].ToString().ToLower())
				{
					case "b":
						p.TransactionType = TransactionType.BUY;
						_continue = true;
						break;
					case "s":
						p.TransactionType = TransactionType.SELL;
						_continue = true;
						break;
					default:
						iwrite.WriteLine("Illegal transaction code '" + cmdArguments[1].ToString() + "' use B for buy, S for sell");
						break;
				}

				_continue = (_orderCookie == null);

				if (_continue)
				{
					//initialize the order's parameters
					p.Security = new Stock();
					p.Security.IdNum = JQuant.Convert.StrToInt(cmdArguments[2].ToString());
					p.Quantity = JQuant.Convert.StrToInt(cmdArguments[3].ToString());
					p.Price = JQuant.Convert.StrToDouble(cmdArguments[4].ToString());
					_orderCookie = new object();

					//and pass the order for processing by the orders FSM
					_rzFSM.Create(p, OrderEventNotifier, null, out _orderCookie);
				}
				else
				{
					Console.WriteLine("no more than a single order is allowed in SIM mode. Cancel existing order first.");
				}
			}
			//wait for all the messages to be printed out, then print the command prompt
			Thread.Sleep(750);
		}

		private void OrderEventNotifier(ref object cookie, FMRShell.OrderEventNotifierMessage msg)
		{
			switch (msg.orderEvent)
			{
				case OrderProcessorEvent.InitOrder:
					Console.WriteLine("Order FSM informed the Algo about Init event.");
					break;
				case OrderProcessorEvent.OrderSent:
					Console.WriteLine("Order FSM informed the Algo about Send event.");
					break;
				case OrderProcessorEvent.TradingApproved:
					Console.WriteLine("Order FSM informed the Algo about ApproveTrading event.");
					break;
				case OrderProcessorEvent.PartialFill:
					Console.WriteLine("Order FSM informed the Algo about PartialFill event.");
					break;
				case OrderProcessorEvent.CompleteFill:
					Console.WriteLine("Order FSM informed the Algo about CompleteFill event.");
					//perform cleanup of the FSM
					_rzFSM.Remove(ref _orderCookie);
					_orderCookie = null;
					break;
				case OrderProcessorEvent.CancelSent:
					Console.WriteLine("Order FSM informed the Algo about SendCancel event");
					break;
				case OrderProcessorEvent.Canceled:
					Console.WriteLine("Order FSM informed the Algo about Canceled event");
					//perform cleanup of the FSM
					_rzFSM.Remove(ref _orderCookie);
					_orderCookie = null;
					break;
				case OrderProcessorEvent.UpdateSent:
					Console.WriteLine("Order FSM informed the Algo about SendUpdate event");
					break;
				case OrderProcessorEvent.Updated:
					Console.WriteLine("Order FSM informed the Algo about Updated event");
					break;
				case OrderProcessorEvent.Cleanup:
					Console.WriteLine("Order FSM informed the Algo about Cleanup event");
					break;
				case OrderProcessorEvent.Error:
					Console.WriteLine("Order FSM informed the Algo about Error event");
					_rzFSM.Remove(ref _orderCookie);
					_orderCookie = null;
					break;
				default:
					break;
			}
		}

		protected void debugOFSM_Cancel(IWrite iwrite, string cmdName, object[] cmdArguments)
		{
			//usage: just type 'cancel' - we work with a single order

			//check if there is pending order
			if (_orderCookie != null)
				_rzFSM.Cancel(ref _orderCookie);
			else
				iwrite.WriteLine("No pending order to cancel.");
			//wait for all the messages to be printed out, then print the command prompt
			Thread.Sleep(750);
		}

		protected void debugOFSM_Update(IWrite iwrite, string cmdName, object[] cmdArguments)
		{
			//usage: update [p newprice] [q newqauntity]
			switch (cmdArguments.Length)
			{
				case 3:
					switch (cmdArguments[1].ToString().ToLower())
					{
						case "p":
							_rzFSM.Update(JQuant.Convert.StrToDouble(cmdArguments[2].ToString()), ref _orderCookie);
							break;
						case "q":
							_rzFSM.Update(JQuant.Convert.StrToInt(cmdArguments[2].ToString()), ref _orderCookie);
							break;
						default:
							iwrite.WriteLine("Some incorrect arguments to Update command.");
							iwrite.WriteLine("usage: update [q newquantity] [p newprice]");
							break;
					}

					break;
				case 5:
					if (cmdArguments[1].ToString().ToLower() != "q" || 
						cmdArguments[3].ToString().ToLower() != "p" || 
						JQuant.Convert.StrToInt(cmdArguments[2].ToString()) <= 0 ||
						JQuant.Convert.StrToDouble(cmdArguments[4].ToString()) <= 0.0 )
					{
						iwrite.WriteLine("Some incorrect arguments to Update command.");
						iwrite.WriteLine("usage: update [q newquantity] [p newprice]");
					}
					else
					{
						_rzFSM.Update(JQuant.Convert.StrToInt(cmdArguments[2].ToString()),
							JQuant.Convert.StrToDouble(cmdArguments[4].ToString()),
							ref _orderCookie);
					}
					break;
				default:
					iwrite.WriteLine("Missing or incorrect args to Update command");
					iwrite.WriteLine("usage: update [q newquantity] [p newprice]");
					break;
			}
			//wait for all the messages to be printed out, then print the command prompt
			Thread.Sleep(750);
		}

		protected void debugOFSM_Remove(IWrite iwrite, string cmdName, object[] cmdArguments)
		{
			_rzFSM.Remove(ref _orderCookie);
		}

		FMRShell.RezefOrderFSM _rzFSM;
		object _orderCookie;

		#endregion;

		protected void LoadCommandLineInterface_dbg()
		{
			Menu menuDebug = cli.RootMenu.AddMenu("dbg", "System debug info",
								   "Created objetcs, access to the system statistics");


			//test Orders FSM
			Menu menuDbgOrdersFSM = menuDebug.AddMenu("OFSM", "Test RZ Orders FSM",
				"Test Rezef Orders FSM API and performance");

			menuDbgOrdersFSM.AddCommand("start", "create Rezef orders FSM",
				"create a new instance of orders FSM", debugOFSM_Create);
			menuDbgOrdersFSM.AddCommand("send", "send a test order, usage: send B/S id Q P",
				"sends a single test order to the sim/real taskbar", debugOFSM_Send);
			menuDbgOrdersFSM.AddCommand("cancel", "cancel sent test order",
				"cancel previously created test order", debugOFSM_Cancel);
			menuDbgOrdersFSM.AddCommand("update", "update sent test order, usage: update [q newQ] [p newP]",
				"update previously sent test order", debugOFSM_Update);
			menuDbgOrdersFSM.AddCommand("remove", "cleanup inactive order",
				"remove canceled/executed order from the FSM", debugOFSM_Remove);
			menuDbgOrdersFSM.AddCommand("stop", "stop Rezef Orders FSM",
				"stop the FSM after trading was completed", debugOFSM_Stop);

			Menu menuDbgFuncs = menuDebug.AddMenu("dbgUF", "Test vatious UserClass functions",
				"Test UserClass functions and get their output");

			menuDbgFuncs.AddCommand("Perms", "check my permissions",
				"Get and print the list of my account permissions", debugUserPermissions);
			menuDbgFuncs.AddCommand("Acc", "check my accounts",
				"Get and print account details", debugUserAcc);


			Menu menuDbgOrderStream = menuDebug.AddMenu("OrderStream", "Test orders Stream",
				"Start/stop orders stream, retrieve orders info from it");

			menuDbgOrderStream.AddCommand("start",
				"Start orders stream - MF|RZ|ALL (by default)",
				"Start streaming orders data from the server", debugStartOrderStream);

			menuDbgOrderStream.AddCommand("stop", "stop orders stream - - MF|RZ|ALL (by default)",
				"Start streaming orders data from the server", debugStopOrderStream);
			menuDbgOrderStream.AddCommand("O", "list MF/RZ/ALL orders",
				"retrieve orders list from the remote server. O MF|RZ|ALL (or nothing)", debugListOrders);



			Menu menuHttp = menuDebug.AddMenu("http", "HTTP server",
								   "Start/stop embedded HTTP server, get traffic statistics");

			menuHttp.AddCommand("start",
									"Start HTTP server",
									"Create instance of HTTP server and start the server", debugHttpStart);

			menuHttp.AddCommand("stop",
									"Stop HTTP server",
									"Stop previously started server", debugHttpStop);

			menuHttp.AddCommand("stat",
									"Show HTTP server traffic counters",
									"Show connections, incoming and outgoing traffic, errors", debugHttpStat);



			menuDebug.AddCommand("sh161",
									"Get TA25 Index weights",
									"Get TA25 Index weights",
									debugLogSH161DataCallback
									);

			menuDebug.AddCommand("AS400TimeTest",
									"ping the server",
									"ping AS400 server in order to get latency and synchronize local amachine time with server's",
									debugGetAS400DTCallback);

			menuDebug.AddCommand("fmrPing",
									"Start FMR ping thread",
									" Ping AS400 server continuosly [login|logout|stat|kill]",
									debugFMRPingCallback);

			menuDebug.AddCommand("threadPoolShow",
									"Show thread pools",
									" List of created thread pools",
									debugThreadPoolShowCallback
									);
			menuDebug.AddCommand("timerShow",
									"Show timers",
									" List of created timers and timer tasks",
									debugTimerShowCallback
									);
			menuDebug.AddCommand("threadShow",
									"Show threads",
									" List of created threads and thread states",
									debugThreadShowCallback);
			menuDebug.AddCommand("mbxShow",
									"Show mailboxes",
									" List of created mailboxes with the current status and statistics",
									debugMbxShowCallback
									);
			menuDebug.AddCommand("poolShow",
									"Show pools",
									" List of created pools with the current status and statistics",
									debugPoolShowCallback
									);
#if USEFMRSIM
			menuDebug.AddCommand("loggerTest",
									"Run simple test of the logger",
									" Create a Collector and start a random data simulator",
									debugLoggerTestCallback
									);
#endif
			menuDebug.AddCommand("loggerShow",
									"Show existing loggers",
									" List of created loggers with the statistics",
									debugLoggerShowCallback
									);
			menuDebug.AddCommand("prodShow",
									"Show producers",
									" List of created producers",
									debugProducerShowCallback
									);
			menuDebug.AddCommand("veriShow",
									"Show data verifiers",
									" List of created data verifiers",
									debugVerifierShowCallback
									);
		}
	}
}
