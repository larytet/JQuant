
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
	partial class Program
	{
		/// <summary>
		/// i can support multiple data collectors and loggers. Because this is CLI i am going to assume
		/// that there is exactly one logger for one data collector. 
		/// </summary>
		protected FMRShell.Collector DataCollector;


		/// <summary>
		/// i can support multiple data collectors and loggers. Because this is CLI i am going to assume
		/// that there is exactly one logger for one data collector. 
		/// </summary>
		protected FMRShell.TradingDataLogger[] DataLogger = new FMRShell.TradingDataLogger[(int)FMRShell.DataType.Last];


		protected void operLoginCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			//check if there is connection already open:
			if (this.fmrConection != null)
			{
				iWrite.WriteLine(
					Environment.NewLine
					+ "WARNING !!! You're already logged in with SessionId="
					+ this.fmrConection.GetSessionId()
					+ Environment.NewLine
					+ "Login Status: "
					+ this.fmrConection.loginStatus
					+ Environment.NewLine
					);
			}
			//if no open connection - perform login process:
			else
			{
				// Define where the xml with connection params is.
				// To define JQUANT_ROOT - see howto in Main(...) in Main.cs
				string ConnFile = Environment.GetEnvironmentVariable("JQUANT_ROOT");
				ConnFile += "ConnectionParameters.xml";

				this.fmrConection = new FMRShell.Connection(ConnFile);

				bool openResult;
				int errResult;
				openResult = this.fmrConection.Open(iWrite, out errResult, true);

				iWrite.WriteLine("");
				if (openResult)
				{
					iWrite.WriteLine("Connection opened for " + this.fmrConection.GetUserName());
					iWrite.WriteLine("sessionId=" + errResult);
				}
				else
				{
					iWrite.WriteLine("Connection failed errResult=" + errResult);
					iWrite.WriteLine("Error description: " + this.fmrConection.LoginErrorDesc());
				}

				iWrite.WriteLine("Login status is " + this.fmrConection.loginStatus.ToString());
			}
		}

		protected void debugOperationsLogMaofCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			// generate filename
			string filename = Resources.CreateLogFileName("MaofLog_", LogType.CSV);
			iWrite.WriteLine("Log file " + filename);
			OpenStreamAndLog(iWrite, false, FMRShell.DataType.Maof, filename, "MaofLogger");
		}


		protected void operLogoutCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			if (fmrConection != null)
			{
				int s = this.fmrConection.GetSessionId();
				this.fmrConection.Dispose();
				this.fmrConection = null;  //set connection to null
				Console.WriteLine("Session with id " + s + " was terminated.");
			}
			else
			{
				Console.WriteLine("There is no active connection - you're not logged in.");
			}
		}


		protected void operLogCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			//first check if logged in:
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

			// check the entered arguments:
			else if (cmdArguments.Length > 3)
			{
				iWrite.WriteLine(Environment.NewLine + "Too many arguments. Try again");
			}

			//Check whether log directory exists
			else if (Resources.GetLogsFolder() == null)
			{
				iWrite.WriteLine("Can't perform the commmand. Define the logs directory.");
			}

			//this one with no args initializes all the three logs
			else if (cmdArguments.Length == 1)
			{
				LogMaof(iWrite);
				LogRezef(iWrite);
				LogMadad(iWrite);
			}
			else if (cmdArguments.Length == 2)   // start one specified log
			{
				switch (cmdArguments[1].ToString().ToLower())
				{
					case "mf":
						LogMaof(iWrite);
						break;
					case "rz":
						LogRezef(iWrite);
						break;
					case "mdd":
						LogMadad(iWrite);
						break;
					default:
						iWrite.WriteLine(Environment.NewLine
							+ "Invalid data type argument '"
							+ cmdArguments[1].ToString()
							+ "'. Type 'startlog + (optional) MF|RZ|MDD'");
						break;
				}
			}
			else if (cmdArguments.Length == 3)   // start a specified log
			{                                     // using playback data generator in simulation mode
				switch (cmdArguments[1].ToString().ToLower())
				{
					case "mf":
						LogMaof(iWrite, cmdArguments[2].ToString());
						break;
					default:
						iWrite.WriteLine(Environment.NewLine
							+ "Invalid data type argument '"
							+ cmdArguments[1].ToString()
							+ "'. Type 'startlog + (optional) MF' + playbackLogName");
						break;
				}
			}
		}

		protected void operStopLogCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			bool _stopStream = false;
			if (cmdArguments.Length == 1) iWrite.WriteLine("Please supply data type: MF | RZ | MDD");
			else if (cmdArguments.Length > 3) iWrite.WriteLine("Too many arguments");
			else
			{
				if (cmdArguments.Length == 3)
				{
					_stopStream = (cmdArguments[2].ToString().ToLower() == "y");
					Console.WriteLine(_stopStream);
				}
				switch (cmdArguments[1].ToString().ToLower())
				{
					case "mf":
						CloseLog(iWrite, FMRShell.DataType.Maof, _stopStream);
						break;
					case "rz":
						CloseLog(iWrite, FMRShell.DataType.Rezef, _stopStream);
						break;
					case "mdd":
						CloseLog(iWrite, FMRShell.DataType.Madad, _stopStream);
						break;
					default:
						iWrite.WriteLine("Invalid data type parameter: " + cmdArguments[1].ToString());
						break;
				}
			}
		}

		protected void StopStreamCallBack(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			if (cmdArguments.Length == 1) iWrite.WriteLine("Please supply data type: MF | RZ | MDD");
			else if (cmdArguments.Length > 2) iWrite.WriteLine("Too many arguments");
			else
			{
				switch (cmdArguments[1].ToString().ToLower())
				{
					case "mf":
						StopStream(iWrite, FMRShell.DataType.Maof);
						break;
					case "rz":
						StopStream(iWrite, FMRShell.DataType.Rezef);
						break;
					case "mdd":
						StopStream(iWrite, FMRShell.DataType.Madad);
						break;
					default:
						iWrite.WriteLine("Invalid data type parameter: " + cmdArguments[1].ToString());
						break;
				}
			}

		}

		protected void debugOperatonsStopLogCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			//CloseDataStreamAndLog(iWrite, (FMRShell.DataType)cmdArguments[0]);    //an error here - invalid cast exception
			CloseLog(iWrite, FMRShell.DataType.Maof, false);
		}

		protected void CloseLog(IWrite iWrite, FMRShell.DataType dt, bool stopStream)
		{
			FMRShell.TradingDataLogger dataLogger = DataLogger[(int)dt];
			dataLogger.Stop();
			dataLogger.Dispose();
			dataLogger = null;
			if (stopStream)
			{
				StopStream(iWrite, dt);
			}
		}

		protected void StopStream(IWrite iWrite, FMRShell.DataType dt)
		{
			DataCollector.Stop(dt);
		}

		// Operations are intended to run in the real TaskBar environment - no simulated API

		protected void LogMaof(IWrite iWrite)
		{
			LogMaof(iWrite, null);
		}

		protected void LogMaof(IWrite iWrite, string backlog)
		{
			// generate filename and display it
			string filename = Resources.CreateLogFileName("MaofLog_", LogType.CSV);
			iWrite.WriteLine("Maof log file " + filename);

			OpenStreamAndLog(iWrite, false, FMRShell.DataType.Maof, filename, "MaofLogger", backlog);
		}

		protected void LogMadad(IWrite iWrite)
		{
			string filename = Resources.CreateLogFileName("MadadLog_", LogType.CSV);
			iWrite.WriteLine("Madad log file " + filename);

			OpenStreamAndLog(iWrite, false, FMRShell.DataType.Madad, filename, "MadadLogger");
		}

		protected void LogRezef(IWrite iWrite)
		{
			string filename = Resources.CreateLogFileName("RezefLog_", LogType.CSV);
			iWrite.WriteLine("Rezef log file " + filename);

			OpenStreamAndLog(iWrite, false, FMRShell.DataType.Rezef, filename, "RezefLogger");
		}


		protected void OpenStreamAndLog(IWrite iWrite, bool test, FMRShell.DataType dt, string filename, string loggerName)
		{
			// use NULL for backlogfile - in simulation mode data generator is random
			// 
			OpenStreamAndLog(iWrite, test, dt, filename, loggerName, null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="iWrite">
		/// A <see cref="IWrite"/>
		/// </param>
		/// <param name="test">
		/// A <see cref="System.Boolean"/>
		/// </param>
		/// <param name="dt">
		/// A <see cref="FMRShell.DataType"/>
		/// </param>
		/// <param name="filename">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="loggerName">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="backlogfile">
		/// A <see cref="System.String"/>
		/// Applicable only in the simulation mode
		/// If the file is specified (not null) - play back the specified pre-recorded log file
		/// If null use randomly generated data
		/// </param>
		protected void OpenStreamAndLog(IWrite iWrite, bool test, FMRShell.DataType dt, string filename, string loggerName, string backlogfile)
		{
#if USEFMRSIM
			if (dt == FMRShell.DataType.Maof)
			{
				TaskBarLibSim.EventGenerator<K300MaofType> dataMaofGenerator;
				if (backlogfile == null)
				{
					// create Maof data generator
					dataMaofGenerator = new TaskBarLibSim.MaofDataGeneratorRandom();
				}
				else
				{
					// create Maof data generator
					dataMaofGenerator = new TaskBarLibSim.MaofDataGeneratorLogFile(backlogfile, 1, 0);
				}
				// setup the data generator(s) in the K300Class
				TaskBarLibSim.K300Class.InitStreamSimulation(dataMaofGenerator);
			}
			else if (dt == FMRShell.DataType.Rezef)
			{
				//create Rezef data generator
				TaskBarLibSim.RezefDataGeneratorRandom dataRzfGenerator = new TaskBarLibSim.RezefDataGeneratorRandom();
				TaskBarLibSim.K300Class.InitStreamSimulation(dataRzfGenerator);
			}
			else if (dt == FMRShell.DataType.Madad)
			{
				//create Madad data generator
				TaskBarLibSim.MadadDataGeneratorRandom dataMddGenerator = new TaskBarLibSim.MadadDataGeneratorRandom();
				TaskBarLibSim.K300Class.InitStreamSimulation(dataMddGenerator);
			}

			else
			{
				iWrite.WriteLine(Environment.NewLine + "Warning data type not supported in FMR simulation: " + dt.ToString() + Environment.NewLine);
			}
#endif

			// Check that there is no data collector created already
			Console.WriteLine("DT= " + dt.ToString() + " (" + (int)dt + ")");
			if (DataCollector == null)
			{
				// create Collector (producer) - will do it only once
				DataCollector = new FMRShell.Collector(this.fmrConection.GetSessionId());
			}

			// create logger which will register itself (AddSink) in the collector
			FMRShell.TradingDataLogger dataLogger = default(FMRShell.TradingDataLogger);
			if (dt == FMRShell.DataType.Maof)
			{
				dataLogger = new FMRShell.TradingDataLogger(loggerName, filename, false,
				DataCollector.maofProducer, new FMRShell.MarketDataMaof().Legend);
			}
			else if (dt == FMRShell.DataType.Rezef)
			{
				dataLogger = new FMRShell.TradingDataLogger(loggerName, filename, false,
				DataCollector.rezefProducer, new FMRShell.MarketDataRezef().Legend);
			}
			else if (dt == FMRShell.DataType.Madad)
			{
				dataLogger = new FMRShell.TradingDataLogger(loggerName, filename, false,
				DataCollector.madadProducer, new FMRShell.MarketDataMadad().Legend);
			}
			else System.Console.WriteLine("No handling for data type " + dt + "(" + (int)dt + ")");

			DataLogger[(int)dt] = dataLogger;

			// start logger
			dataLogger.Start();

			// start collector, which will start the stream in K300Class
			DataCollector.Start(dt);

			debugLoggerShowCallback(iWrite, "", null);

			if (test)
			{
				Thread.Sleep(1000);
				CloseLog(iWrite, dt, true);
			}
		}


		protected void LoadCommandLineInterface_oper()
		{
			Menu menuOperations = cli.RootMenu.AddMenu("oper", "Operations",
								   " Login, start stream&log");

			menuOperations.AddCommand("Login",
										"Login to the remote server",
										"The call will block until login succeeds",
										operLoginCallback
										);
			menuOperations.AddCommand("Logout",
										"Perform the logout process",
										"The call will block until logout succeeds",
										operLogoutCallback
										);
			menuOperations.AddCommand("StartLog",
										"Log data stream - choose MF|RZ|MDD.",
										"Start market data stream and run logger. In sim mode playback file can be specified",
										operLogCallback
										);
			menuOperations.AddCommand("StopLog",
										"Stop log - MF|MDD|RZ, to stop stream type Y",
										"Stop logger - Maof(MF) | Madad (MDD)| Rezef(RZ) and, optionally, the stream [Y]",
										operStopLogCallback);
			menuOperations.AddCommand("StopStream",
										"Stop the data stream - MF | MDD | RZ",
										"Stop data stream - Maof(MF) | Madad (MDD) | Rezef (RZ)",
										StopStreamCallBack
										);
			menuOperations.AddCommand("ShowLog",
										"Show existing loggers",
										"List of created loggers with the statistics",
										debugLoggerShowCallback
										);
			menuOperations.AddCommand("AS400TimeTest",
										"Ping the server",
										"Ping AS400 server in order to get latency",
										debugGetAS400DTCallback);
		}

	}
}
