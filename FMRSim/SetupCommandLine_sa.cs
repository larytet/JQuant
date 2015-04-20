
using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;

using FMRShell;
using Algo;

#if USEFMRSIM
using TaskBarLibSim;
#else
using TaskBarLib;
#endif


namespace JQuant
{
	partial class Program
	{
		protected Algo.Base algoMachine = null;
		RezefOrderFSM rezefOrderFSM = null;

		/// <summary>
		/// Create Algo object and connect it to the data feed
		/// Data feed can be a playback or real-time data feed
		/// </summary>
		protected void algoStart(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			if (IsLogged(iWrite))
			{
				if (algoMachine != null)
				{
					iWrite.WriteLine("Stop the algo first");
					return;
				}

				bool argsRes = true;
#if USEFMRSIM
				string backlogfile = null;
				double speedup = 1.0;
#endif
				// parse arguments
				switch (cmdArguments.Length)
				{
#if USEFMRSIM
					case 0:
					case 1:
						iWrite.WriteLine("Usage: <backlogfile> [speedup]");
						argsRes = false;
						break;

					case 2:
						backlogfile = cmdArguments[1].ToString();
						break;

					case 3:
						backlogfile = cmdArguments[1].ToString();
						speedup = JQuant.Convert.StrToDouble(cmdArguments[2].ToString(), 1.0);
						break;
#endif

					default:
						iWrite.WriteLine("Too many arguments");
						argsRes = false;
						break;
				}

				if (!argsRes) return;


				// if i run in simulation mode create playback
#if USEFMRSIM
				// create Rezef data generator
				TaskBarLibSim.EventGenerator<K300RzfType> dataGenerator =
					new TaskBarLibSim.RezefDataGeneratorLogFile(backlogfile, speedup, 500);

				// setup the data generator(s) in the K300Class
				TaskBarLibSim.K300Class.InitStreamSimulation(dataGenerator);
#endif

				// Create data collector if not created already. 
				// Do not set internal time stamps, use time stamps from the recorded data log
				if (DataCollector == null)
					DataCollector = new FMRShell.Collector(this.fmrConection.GetSessionId(), false);

				// create Order FSM
				rezefOrderFSM = new RezefOrderFSM(this.fmrConection);
				rezefOrderFSM.Start();

				// create Algo 
				if (algoMachine == null)
					algoMachine = new Algo.Base(rezefOrderFSM);

				// connect algo to the data feed
				DataCollector.rezefProducer.AddConsumer(algoMachine);

				// start data collector, which will start the stream in K300Class
				DataCollector.Start(FMRShell.DataType.Rezef);

				// start algo - can be also done afterwards, by calling Start
				algoMachine.Start();
			}
		}

		protected void algoStop(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			if (IsLogged(iWrite))
			{
				//stop and dispose algo
				if (algoMachine != default(Algo.Base))
				{
					algoMachine.StopGracefull();
					DataCollector.rezefProducer.RemoveConsumer(algoMachine);
					algoMachine.Dispose();
					algoMachine = default(Algo.Base);
				}
				//stop and dispose OrderFSM
				if (rezefOrderFSM != default(FMRShell.RezefOrderFSM))
				{
					rezefOrderFSM.Dispose();
					rezefOrderFSM = default(FMRShell.RezefOrderFSM);
				}

				//TODO - test if there is no other users, before calling DataCollector.Stop
				if (DataCollector != default(FMRShell.Collector))
					DataCollector.Stop(FMRShell.DataType.Rezef);
			}
		}

		protected void algoStat(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			if (algoMachine == null)
			{
				iWrite.WriteLine("No algo started");
				return;
			}

			IResourceStatistics resStat = algoMachine;

			System.Collections.ArrayList names;
			System.Collections.ArrayList values;
			resStat.GetEventCounters(out names, out values);

			int columnSize = 8;
			CommandLineInterface.printTableHeader(iWrite, names, columnSize);
			CommandLineInterface.printValues(iWrite, values, columnSize);
		}

		protected void algoStopUrgent(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			algoMachine.StopUrgent();
		}

		protected void algoPaper(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			if (algoMachine != null)
			{
				algoMachine.PlacePaperTrade();
			}
			else
			{
				if (IsLogged(iWrite))
				{
					iWrite.WriteLine("Algo is not started.");
				}
			}
		}

		protected void algoReal(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			if (algoMachine != null)
			{
				algoMachine.PlaceRealTrade();
			}
			else
			{
				if (IsLogged(iWrite))
				{
					iWrite.WriteLine("Algo is not started.");
				}
			}
		}

		protected void algoSkip(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			if (algoMachine != null)
			{
				algoMachine.SkipTrigger();
			}
			else
			{
				if (IsLogged(iWrite))
				{
					iWrite.WriteLine("Algo is not started.");
				}
			}
		}

		protected void algoSetMode(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			if (algoMachine != null)
			{
				switch (cmdArguments.Length)
				{
					case 1:
						iWrite.WriteLine("Please deifne the mode - a | s");
						break;
					case 2:
						switch (cmdArguments[1].ToString().ToLower())
						{
							case "a":
								algoMachine.SetMode(Base.Mode.Automatic);
								break;
							case "s":
								algoMachine.SetMode(Base.Mode.Semiautomatic);
								break;
							default:
								iWrite.WriteLine("Invalid mode " + cmdArguments[1].ToString());
								break;
						}
						break;
					default:
						iWrite.WriteLine("Too many arguments");
						break;
				}
			}
		}

		protected void LoadCommandLineInterface_sa()
		{
			Menu menuAlgo = cli.RootMenu.AddMenu("algo", "Algo debug",
								   "Semiautomatic traidng routines");
			menuAlgo.AddCommand("start",
				"Create algo",
				"Create algo, connect to the data feed - DataCollector.rezefProducer. Args: " +
#if USEFMRSIM
				"<logFile> [speedup]",
#else			                  
				"",
#endif
				algoStart);

			menuAlgo.AddCommand("stop",
							  "Graceful stop",
							  "Stop the FSM gracefuly - takes care to close all pending orders",
							   algoStop);
			menuAlgo.AddCommand("stopu",
							  "Immediate stop",
							  "Gets out immediately. Does not clean up the pending orders if any",
							   algoStop);
			menuAlgo.AddCommand("stat",
							  "Show algo statistics counters",
							  "Show traffic related and trades related statistics",
							   algoStat);
			menuAlgo.AddCommand("s",
							  "Skip trigger",
							  "Skip the trigger, go on with looking for triggers",
							algoSkip);
			menuAlgo.AddCommand("p",
							  "Launch paper trade",
							  "Simulated paper trading using either real time data or playback log",
							algoPaper);
			menuAlgo.AddCommand("o",
							  "Launch real trade",
							  "Real cash order using based on real-time data feed",
							algoReal);
			menuAlgo.AddCommand("sm",
							  "Toggle Algo FSM mode. Usage: sm a|s ",
							  "Set mode: semiautomatic or automatic",
							algoSetMode);
		}
	}
}
