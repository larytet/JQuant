
using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using System.IO;

namespace JQuant
{

	public interface IResourceStatistics
	{
		/// <summary>
		/// returns array of debug counters
		/// </summary>
		void GetEventCounters(out System.Collections.ArrayList names, out System.Collections.ArrayList values);
	}

	public interface INamedResource
	{
		/// <summary>
		/// returns name of the resource
		/// interface does not allow to define protected set operator
		/// in the future i will replace this by method GetName() returning string. 
		/// </summary>
		string Name
		{
			get;
			set;
		}
	}


	/// <summary>
	/// objects implementing Mailbox
	/// </summary>
	public interface IResourceMailbox : INamedResource, IResourceStatistics
	{
	}


	public enum ThreadState
	{
		[Description("Initialized")]
		Initialized,
		[Description("Started")]
		Started,
		[Description("Stoped")]
		Stoped,
		[Description("Destroyed")]
		Destroyed

	};

	/// <summary>
	/// objects implementing MailboxThread
	/// </summary>
	public interface IThread : INamedResource
	{
		ThreadState GetState();
		long GetLongestJob();
	}

	/// <summary>
	/// objects implementing Pool interface
	/// </summary>
	public interface IResourcePool : INamedResource, IResourceStatistics
	{
	}

	public interface IResourceProducer : INamedResource, IResourceStatistics
	{
	}

	public interface IResourceJobQueue : INamedResource
	{
	}


	public enum LogType
	{
		[Description("Dynamic memory")]
		RAM,

		[Description("Serialization")]
		BinarySerialization,

		[Description("Binary")]
		Binary,

		[Description("ASCII")]
		Ascii,

		[Description("CSV")]
		CSV,

		[Description("HTML")]
		HTML,

		[Description("XML")]
		XML,

		[Description("SQL")]
		SQL
	};


	/// <summary>
	/// System logs will register in the central data base
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		/// returns name of the logger 
		/// </summary>
		string GetName();

		/// <summary>
		///returns number of records 
		/// </summary>
		/// <returns>
		/// A <see cref="System.Int32"/>
		/// number of records in the log
		/// </returns>
		int GetCountLog();

		/// <summary>
		/// number of times Logger was invoke
		/// normallu equal or close to what GetCountLog() returns
		/// difference between two is number of pending entries (wait in queue
		/// for processing)
		/// </summary>
		/// <returns>
		/// A <see cref="System.Int32"/>
		/// </returns>
		int GetCountTrigger();

		int GetCountDropped();

		LogType GetLogType();

		/// <summary>
		/// returns true if the log entries are time stamped by the producer. 
		/// Logs can and will time stamp the entries. So in some cases there are going to be two
		/// time stamps - by the producer and by the logger.
		/// </summary>
		bool TimeStamped();

		/// <summary>
		/// returns time of the most recent log entry
		/// </summary>
		System.DateTime GetLatest();

		/// <summary>
		/// returns time of the oldest log entry
		/// </summary>
		System.DateTime GetOldest();
	}


	public interface IDataGenerator
	{
		int GetCount();
		string GetName();
	}

	public interface IResourceTimerList : INamedResource, IResourceStatistics
	{
		string GetTaskName();
	}

	public interface IResourceThreadPool : IResourceStatistics, INamedResource
	{
	}

	public interface IResourceDataVerifier : IResourceStatistics, INamedResource
	{
	}

	/// <summary>
	/// Just lot of counters used by the management to figure out what changes 
	/// happen in the system from the previous sample 
	/// </summary>
	[Serializable]
	public class SystemEventCounters
	{
		public SystemEventCounters()
		{
			this.fields = this.GetType().GetFields();
		}

		public long httpChanged;  // any HTTP server related events will be ocunted here
		public long saTrading;  // any semi-automatic FSM events


		/// Create string like this
		/// "{
		///    "httpChanged": "40",
		/// }"
		/// Pay attention to comma in the end - looks like this is Ok and 
		/// does not cause any problems
		public string GetJSON()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder(fields.Length * 20);

			foreach (FieldInfo field in fields)
			{
				string name = field.Name;
				object val = field.GetValue(this);

				sb.Append("\"");
				sb.Append(name);
				sb.Append("\":");

				sb.Append("\"");
				sb.Append(val);
				sb.Append("\",");
			}
			System.DateTime dtNow = System.DateTime.Now;
			string strNow = dtNow.ToString();
			sb.Append("\"DateTime\":");
			sb.Append("\"");
			sb.Append(strNow);
			sb.Append("\"");

			string json = sb.ToString();
			return json;
		}

		protected FieldInfo[] fields;
	}


	/// <summary>
	/// a storage of all created objects
	/// an object central
	/// this guy is singleton - application calls Init() only once to initialize the class
	/// </summary>
	public class Resources
	{
		/// <summary>
		/// call Init() to create a single instance of this class 
		/// </summary>
		protected Resources()
		{
			systemEventCounters = new SystemEventCounters();

			Mailboxes = new System.Collections.ArrayList(10);
			Threads = new List<IThread>(10);
			Pools = new System.Collections.ArrayList(10);
			Loggers = new List<ILogger>(10);
			DataGenerators = new List<IDataGenerator>(10);
			TimerLists = new List<IResourceTimerList>(5);
			ThreadPools = new System.Collections.ArrayList(2);
			Producers = new List<IResourceProducer>(5);
			Verifiers = new List<IResourceDataVerifier>(10);
		}

		static public void Init()
		{
			if (r == null)
			{
				r = new Resources();
			}
			else
			{
				Console.WriteLine("Class Resources is a singletone. Resources.Init() can be called only once");
			}
		}

		/// <summary>
		/// created in the system mailboxes
		/// </summary>
		public static System.Collections.ArrayList Mailboxes;

		/// <summary>
		/// i expect that creation of threads is not an often operation
		/// if this is not the case one can construct threads which do not register at all
		/// or a thread pool where all threads are registered after boot.  
		/// Adding a thread will not take too much time, but thread deletion will 
		/// consume some CPU cycles.
		/// </summary>
		public static List<IThread> Threads;

		public static System.Collections.ArrayList Pools;

		public static List<ILogger> Loggers;

		public static List<IDataGenerator> DataGenerators;

		public static List<IResourceTimerList> TimerLists;

		public static List<IResourceProducer> Producers;

		public static System.Collections.ArrayList ThreadPools;

		public static List<IResourceDataVerifier> Verifiers;

		public static SystemEventCounters systemEventCounters;

		static protected Resources r;


		protected static string DateNowToFilename()
		{
			string s = DateTime.Now.ToString();
			s = s.Replace('/', '_');
			s = s.Replace(':', '_');
			s = s.Replace(' ', '_');

			return s;
		}


		public static string CreateLogFileName(string name, LogType type)
		{
			string filename = GetLogsFolder();
			if (filename != null)
			{
				filename += name;
				filename += DateNowToFilename();
			}

			switch (type)
			{
				case LogType.Ascii:
					filename += ".txt";
					break;
				case LogType.CSV:
					filename += ".csv";
					break;
				default:
					break;
			}

			return filename;
		}

		public static string GetWwwFolder()
		{
			if (RootDirectoryDefined())
			{
				string path = Environment.GetEnvironmentVariable("JQUANT_ROOT") + "www" + Path.DirectorySeparatorChar;
				if (Directory.Exists(path))
					return path;
				else
				{
					Console.WriteLine(Environment.NewLine
						+ "WARNING! Directory {0} doesn't exist. Define it first."
						+ Environment.NewLine, path);
					return null;
				}
			}
			else
			{
				Console.WriteLine(Environment.NewLine
					+ "WARNING! Environment variable JQUANT_ROOT is not set" + Environment.NewLine);

				return null;
			}
		}

		public static string GetLogsFolder()
		{
			if (RootDirectoryDefined())
			{
				string path = Environment.GetEnvironmentVariable("JQUANT_ROOT") + "DataLogs" + Path.DirectorySeparatorChar;
				if (Directory.Exists(path))
					return path;
				else
				{
					Console.WriteLine(Environment.NewLine
						+ "WARNING! Directory {0} doesn't exist. Define it first."
						+ Environment.NewLine, path);
					return null;
				}
			}
			else
			{
				Console.WriteLine(Environment.NewLine
					+ "WARNING! Environment variable JQUANT_ROOT is not set" + Environment.NewLine);

				return null;
			}
		}


		/// <summary>
		/// Need to define an environment variable called JQUANT_ROOT
		/// which defines the directory where different configuration
		/// and data files are located (like xml with connection params)
		/// 
		/// Howoto:
		/// === Win XP: right-click 'My Computer'->Properties->Advanced->
		/// Environment variables -> New. Reboot. 
		/// To check use 'set' w/o parameters from cmd.
		/// note: set from cmd doesn't work (it's for temporary EVs only)
		/// === *NIX:
		/// on Ubuntu add it to /etc/environment file, logout and login
		/// to check use 'env' commnad w/o parameters
		/// </summary>
		public static bool RootDirectoryDefined()
		{
			string jquantRoot = Environment.GetEnvironmentVariable("JQUANT_ROOT");
			return (jquantRoot != null);
		}

		/// <summary>
		/// Tells if the app runs under *NIX or not (that is, WIN)
		/// </summary>
		public static bool isUnix
		{
			get;
			set;
		}

	}
}//namespace JQuant
