
using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;

using System.Reflection;
using System.Text;


namespace JQuant
{
	partial class Program
	{

		// This function is called from Main before entering main loop
		protected void SelfTests()
		{
		}


		protected void debugGcCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			System.GC.Collect();
			System.GC.WaitForPendingFinalizers();
			iWrite.WriteLine("Garbage collection done");
		}

		protected void debugMbxTestCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			Mailbox<bool> mbx = new Mailbox<bool>("TestMbx", 2);

			iWrite.WriteLine("TestMbx created");
			bool message = true;
			bool result = mbx.Send(message);
			if (!result)
			{
				iWrite.WriteLine("Mailbox.Send returned false");
			}
			else
			{
				iWrite.WriteLine("Mailbox.Send message sent");
			}
			result = mbx.Receive(out message);
			if (!result)
			{
				iWrite.WriteLine("Mailbox.Receive returned false");
			}
			else
			{
				iWrite.WriteLine("Mailbox.Send message received");
			}
			if (!message)
			{
				iWrite.WriteLine("I did not get what i sent");
			}
			debugMbxShowCallback(iWrite, cmdName, cmdArguments);

			mbx.Dispose();

			System.GC.Collect();
		}

		protected void debugMbxShowCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			debugPrintResourcesNameAndStats(iWrite, Resources.Mailboxes);
		}

		protected long[] threadpoolTestTicks;

		protected void ThreadPoolJobEnter(ref object argument)
		{
		}

		protected void ThreadPoolJobDone(object argument)
		{
			int c = (int)argument;
			long tick = DateTime.Now.Ticks;
			threadpoolTestTicks[c] = (tick - threadpoolTestTicks[c]);
		}

		protected void debugThreadTestCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			debugThreadShowCallback(iWrite, cmdName, cmdArguments);
			MailboxThread<bool> thr = new MailboxThread<bool>("TestMbx", 2);
			debugThreadShowCallback(iWrite, cmdName, cmdArguments);
			thr.Start();
			debugThreadShowCallback(iWrite, cmdName, cmdArguments);
			bool message = true;
			bool result = thr.Send(message);
			if (!result)
			{
				iWrite.WriteLine("Thread.Send returned false");
			}
			thr.Stop();
			debugThreadShowCallback(iWrite, cmdName, cmdArguments);

			System.GC.Collect();
		}

		protected void debugThreadShowCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			iWrite.WriteLine();
			iWrite.WriteLine(
				OutputUtils.FormatField("Name", 10) +
				OutputUtils.FormatField("State", 14) +
				OutputUtils.FormatField("Ticks", 10)
			);
			iWrite.WriteLine("-------------------------------------------");
			bool isEmpty = true;
			foreach (IThread iThread in Resources.Threads)
			{
				isEmpty = false;
				iWrite.WriteLine(
					OutputUtils.FormatField(iThread.Name, 10) +
					OutputUtils.FormatField(EnumUtils.GetDescription(iThread.GetState()), 14) +
					OutputUtils.FormatField(iThread.GetLongestJob(), 10)
				);

			}
			if (isEmpty)
			{
				iWrite.WriteLine("No threads");
			}
			int workerThreads;
			int completionPortThreads;
			System.Threading.ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
			iWrite.WriteLine("workerThreads=" + workerThreads + ",completionPortThreads=" + completionPortThreads);

		}

		protected void Timer5sHandler(ITimer timer)
		{
			Console.WriteLine("5s timer expired " + DateTime.Now);
		}

		protected void Timer30sHandler(ITimer timer)
		{
			Console.WriteLine("30s timer expired " + DateTime.Now);
		}

		protected void debugTimerTestThread()
		{
			// create set (timer task). initially empty
			TimerTask timerTask = new TimerTask("ShortTimers");

			Console.WriteLine("Start timers " + DateTime.Now);

			// create two types of timers
			TimerList timers_5sec = new TimerList("5sec", 5 * 1000, 100, this.Timer5sHandler, timerTask);
			TimerList timers_30sec = new TimerList("30sec", 30 * 1000, 100, this.Timer30sHandler, timerTask);

			timerTask.Start();

			// start some timers
			timers_5sec.Start();
			timers_5sec.Start();
			timers_5sec.Start();
			Thread.Sleep(1 * 1000);
			timers_5sec.Start();

			ITimer timer;
			long timerId;
			timers_30sec.Start(out timer, out timerId, null, false);
			timers_5sec.Start();

			debugTimerShowCallback(null, null, null);

			// wait for the first timer to expire
			Thread.Sleep(10 * 1000);
			timers_30sec.Stop(timer, timerId);

			Thread.Sleep(30 * 1000);
			debugTimerShowCallback(null, null, null);

			// clean up
			timers_5sec.Dispose();
			timers_30sec.Dispose();
			timerTask.Dispose();
		}

		protected void debugTimerTestCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			// call once - init timers subsystem
			Timers.Init();

			// timer test contains delauys. run the test from a separate thread and release
			// user input context
			System.Threading.Thread thread = new System.Threading.Thread(debugTimerTestThread);
			thread.Start();
		}

		protected void debugTimerShowCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			System.Collections.ArrayList names;
			System.Collections.ArrayList values;
			int entry = 0;
			int columnSize = 12;

			bool isEmpty = true;

			((JQuant.IWrite)this).WriteLine();

			foreach (IResourceTimerList timerList in Resources.TimerLists)
			{
				timerList.GetEventCounters(out names, out values);
				isEmpty = false;

				if (entry == 0)
				{
					names.Insert(0, "TimerListName");
					names.Insert(0, "TimerTaskName");
					CommandLineInterface.printTableHeader((JQuant.IWrite)this, names, columnSize);
				}
				values.Insert(0, OutputUtils.FormatField(timerList.Name, columnSize));
				values.Insert(0, OutputUtils.FormatField(timerList.GetTaskName(), columnSize));
				CommandLineInterface.printValues((JQuant.IWrite)this, values, columnSize);

				entry++;

			}
			if (isEmpty)
			{
				System.Console.WriteLine("No timers");
			}
		}

		protected int debugRTClockSleep(Random random)
		{
			int res = 0;
			int ms = random.Next(0, 2 * 100);

			Thread.Sleep(ms);

			return res;
		}

		protected void debugRTClockCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			Random random = new Random();
			DateTime dtRT0;
			int tests = 0;


			dtRT0 = DateTimePrecise.Now;
			iWrite.WriteLine("tick=" + dtRT0.Ticks + " " + dtRT0 + " " + DateTime.Now);
			Thread.Sleep(30);

			dtRT0 = DateTimePrecise.Now;
			iWrite.WriteLine("tick=" + dtRT0.Ticks + " " + dtRT0 + " " + DateTime.Now);
			Thread.Sleep(70);

			dtRT0 = DateTimePrecise.Now;
			iWrite.WriteLine("tick=" + dtRT0.Ticks + " " + dtRT0 + " " + DateTime.Now);
			Thread.Sleep(100);

			do
			{
				// read time stamps - system and PreciseTime
				DateTime dt, dtNow;
				long maxDrift = 16 * 10000;
				dt = DateTime.Now;
				dtNow = dt;
				DateTime dtB = dt.Subtract(new TimeSpan(maxDrift));

				DateTime dtRT1 = DateTimePrecise.Now;

				dt = DateTime.Now;
				DateTime dtA = dt.Add(new TimeSpan(maxDrift));


				// run checks
				if (dtRT1 < dtRT0)
				{
					iWrite.WriteLine("Time moves backward dtRT1=" + dtRT1 + "." + dtRT1.Millisecond +
									  " dtRT0=" + dtRT0 + "." + dtRT0.Millisecond + " delta=" + (dtRT0.Ticks - dtRT1.Ticks) + "ticks");
				}
				if (dtRT1 < dtB)
				{
					iWrite.WriteLine("Timer slower dtRT1=" + dtRT1 + "." + dtRT1.Millisecond +
									  " dtB=" + dtB + "." + dtB.Millisecond + " delta=" + (dtB.Ticks - dtRT1.Ticks) + "ticks");
				}
				if (dtRT1 > dtA)
				{
					iWrite.WriteLine("Timer faster dtRT1=" + dtRT1 + "." + dtRT1.Millisecond +
									 " dtA=" + dtA + "." + dtA.Millisecond + " delta=" + (dtRT1.Ticks - dtB.Ticks) + "ticks");
				}

				dtRT0 = dtRT1;

				// sleep little bit
				debugRTClockSleep(random);
				tests++;
				if ((tests & 0xFF) == 0xFF)
				{
					iWrite.Write("." + (dtRT0.Ticks - dtNow.Ticks));
				}
			}
			while (true);
		}

		protected void debugRTClock1Callback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			DateTime dtRT0 = DateTimePrecise.Now;
			int tests = 0;
			long maxDelta = 0;

			do
			{
				DateTime dtRT1 = DateTimePrecise.Now;

				// run checks
				if (dtRT1 < dtRT0)
				{
					iWrite.WriteLine("Time moves backward dtRT1=" + dtRT1 + "." + dtRT1.Millisecond +
									  " dtRT0=" + dtRT0 + "." + dtRT0.Millisecond);
					iWrite.WriteLine("dtRT1=" + dtRT1.Ticks +
									 " dtRT0=" + dtRT0.Ticks +
									 " delta=" + (dtRT0.Ticks - dtRT1.Ticks));
				}
				tests++;
				if ((tests & 0x7FFFF) == 0x7FFFF)
				{
					iWrite.Write(".");
					long delta = Math.Abs(dtRT1.Ticks - dtRT0.Ticks);
					if (delta > maxDelta)
					{
						maxDelta = delta;
						iWrite.Write("" + delta);
					}
				}
				dtRT0 = dtRT1;
			}
			while (true);
		}

		protected void debugRTClock2Callback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			Random random = new Random();

			do
			{
				DateTime dtRT0 = DateTimePrecise.Now;
				int delay = random.Next(0, 50);
				Thread.Sleep(delay);
				DateTime dtRT1 = DateTimePrecise.Now;

				iWrite.WriteLine("delay=" + delay + "ms ticks=" + (dtRT1.Ticks - dtRT0.Ticks));
			}
			while (true);
		}

		protected void debugPoolTestCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			Pool<bool> pool = new Pool<bool>("TestPool", 2);

			bool message1 = true;
			bool message2 = false;
			pool.Fill(message1); pool.Fill(message2);

			bool result = pool.Get(out message1);
			if (!result)
			{
				iWrite.WriteLine("Pool.Get returned false");
			}
			if (message1)
			{
				iWrite.WriteLine("I did not get what i stored");
			}
			pool.Free(message1);
			debugPoolShowCallback(iWrite, cmdName, cmdArguments);

			pool.Dispose();

			System.GC.Collect();
		}

		protected void debugLoggerTestCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			OpenStreamAndLog(iWrite, true, FMRShell.DataType.Maof, "simLog.txt", "simLogger");
		}

		protected void debugCyclicBufferTestCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			CyclicBuffer<int> cb = new CyclicBufferSynchronized<int>("cliCbTest", 3);

			iWrite.WriteLine();
			foreach (int i in cb)
			{
				iWrite.WriteLine("No elements " + i);
			}

			iWrite.WriteLine();
			cb.Add(0);
			foreach (int i in cb)
			{
				iWrite.WriteLine("One element " + i);
			}

			iWrite.WriteLine();
			cb.Add(1);
			cb.Add(2);
			cb.Add(3);
			cb.Add(4);
			foreach (int i in cb)
			{
				iWrite.WriteLine("Three elements " + i);
			}
		}

		/// <summary>
		/// I can just implement ILogerData (one method) or inherit LoggerDataStructure
		/// I chose to inherit LoggerDataStructure
		/// </summary>
		protected class FileLoggerTestData : LoggerDataStructure
		{
			public FileLoggerTestData()
				: base(",")
			{
				// set fields and call InitValue();
				field1 = counter++;
				field2 = counter++;
			}

			public int field1;
			public int field2;
			static int counter = 201;
		}

		/// <summary>
		/// I can just implement ILogerData (one method) or inherit LoggerDataStructure
		/// I chose to implement ILogerData
		/// </summary>
		protected class FileLoggerTestDataSimple : ILoggerData
		{
			public FileLoggerTestDataSimple()
			{
				// set fields 
				field1 = counter++;
				field2 = counter++;
			}

			public string ToLogString()
			{
				FieldInfo[] fields = this.GetType().GetFields();
				StringBuilder sbData = new StringBuilder(fields.Length * 10);

				object o = (object)this;

				foreach (FieldInfo field in fields)
				{
					object val = field.GetValue(o);
					sbData.Append(val.ToString());
					sbData.Append(","); // add delimiter
				}

				string values = sbData.ToString();
				// System.Console.WriteLine("type="+this.GetType()+",fields="+fields.Length);
				// System.Console.WriteLine("values="+values);

				return values;
			}

			public int field1;
			public int field2;

			static int counter = 0;
		}

		protected void debugFileLoggerTestCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			// I can use FileLoggerTestDataSimple or FileLoggerTestData structures - both work
			FileLogger<FileLoggerTestData> logger = new FileLogger<FileLoggerTestData>("test", "test.csv", false, "");
			logger.Start();

			// add an entry to the log
			logger.AddEntry(new FileLoggerTestData());

			// wait little bit before I tear the logger down - i want to let the logger to finish writing
			Thread.Sleep(200);

			logger.Stop();
			logger.Dispose();
		}

		protected class TestJsonElement
		{
			public TestJsonElement(int d1, int d2)
			{
				this.d1 = d1;
				this.d2 = d2;
				da = new int[2];
				da[0] = d1;
				da[1] = d2;
			}

			public int d1;
			public int d2;
			public int[] da;
		}

		protected string debugJsonTest()
		{
			TestJsonElement[] els = new TestJsonElement[2];

			els[0] = new TestJsonElement(1, 2);
			els[1] = new TestJsonElement(3, 4);

			string json = OutputUtils.GetJSON(els, "els");
			return json;
		}

		protected void debugJsonTestCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			string json = debugJsonTest();
			iWrite.WriteLine(json);
		}


		protected void LoadCommandLineInterface_test()
		{
			Menu menuTests = cli.RootMenu.AddMenu("tst", "Short tests",
								   " Infrastructure/API tests");

			menuTests.AddCommand("GC", "Run garbage collector",
								  " Forces garnage collection", debugGcCallback);
			menuTests.AddCommand("mbxTest", "Run simple mailbox tests",
								  " Create a mailbox, send a message, receive a message, print debug info", debugMbxTestCallback);
			menuTests.AddCommand("mbxShow", "Show mailboxes",
								  " List of created mailboxes with the current status and statistics", debugMbxShowCallback);
			menuTests.AddCommand("threadTest", "Run simple thread",
								  " Create a mailbox thread, send a message, print debug info", debugThreadTestCallback);
			menuTests.AddCommand("threadShow", "Show threads",
								  " List of created threads and thread states", debugThreadShowCallback);
			menuTests.AddCommand("poolTest", "Run simple pool tests",
								  " Create a pool, add object, allocate object, free object", debugPoolTestCallback);
			menuTests.AddCommand("poolShow", "Show pools",
								  " List of created pools with the current status and statistics", debugPoolShowCallback);

			menuTests.AddCommand("timerTest", "Run simple timer tests",
								  " Create a timer task, two timer lists, start two timers, clean up", debugTimerTestCallback);
			menuTests.AddCommand("timerShow", "Show timers",
								  " List of created timers and timer tasks", debugTimerShowCallback);

			menuTests.AddCommand("threadPoolTest", "Run simple thread pool tests",
								  " Create a thread pool, start a couple of jobs, destroy the pool", debugThreadPoolTestCallback);
			menuTests.AddCommand("threadPoolShow", "Show thread pools",
								  " List of created thread pools", debugThreadPoolShowCallback);

			menuTests.AddCommand("cbtest", "Cyclic buffer class test",
								  " Create a cyclic buffer, check functionality", debugCyclicBufferTestCallback);

			menuTests.AddCommand("rtclock", "RT clock test",
								  " Calls PreciseTime periodically and checks that the returned time is reasonable", debugRTClockCallback);
			menuTests.AddCommand("rtclock_1", "RT clock test",
								  " Calls PreciseTime in tight loop and checks that the returned time is reasonable", debugRTClock1Callback);
			menuTests.AddCommand("rtclock_2", "RT clock test",
								  " Calls PreciseTime and print out ticks", debugRTClock2Callback);


			menuTests.AddCommand("logTest", "File logger test",
								  " Create a logger, add a couple of entries and get out", debugFileLoggerTestCallback);

			menuTests.AddCommand("jsonTest", "Test JSON support",
								  " Create set of objects, call Utils.GetJSON(), print the output", debugJsonTestCallback);
		}

	}
}
