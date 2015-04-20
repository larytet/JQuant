
using System;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

using System.Reflection;
using System.Text;


#if USEFMRSIM
using TaskBarLibSim;
#else
using TaskBarLib;
#endif

namespace JQuant
{
	/// <summary>
	/// base class for all system loggers
	/// </summary>
	public abstract class Logger : IDisposable, ILogger
	{
		public Logger(string name)
		{
			this.name = name;
			countTrigger = 0;
			countLog = 0;
			countDropped = 0;

			// add myself to the list of created loggers
			Resources.Loggers.Add(this);
		}

		/// <summary>
		/// application will call this method for proper cleanup
		/// </summary>
		public virtual void Dispose()
		{
			// remove myself from the list of created loggers
			Resources.Loggers.Remove(this);
		}

		public abstract void AddEntry(object o);

		~Logger()
		{
			Console.WriteLine("Logger " + GetName() + " destroyed");
		}

		public string GetName()
		{
			return name;
		}

		public int GetCountLog()
		{
			return countLog;
		}

		public int GetCountTrigger()
		{
			return countTrigger;
		}

		public int GetCountDropped()
		{
			return countDropped;
		}

		public LogType GetLogType()
		{
			return Type;
		}

		public bool TimeStamped()
		{
			return timeStamped;
		}

		public System.DateTime GetLatest()
		{
			return stampLatest;
		}

		public System.DateTime GetOldest()
		{
			return stampOldest;
		}

		public LogType Type
		{
			get;
			set;
		}

		protected string name;
		protected int countTrigger;
		protected int countLog;
		protected int countDropped;
		protected bool timeStamped;
		protected System.DateTime stampLatest;
		protected System.DateTime stampOldest;
	}

	/// <summary>
	/// write log to file
	/// base class for all loggers working with output streams
	/// the class implments asynchronous (postponned) writing and will be
	/// used when there large amounts of data to write or when the data should be
	/// processed before writing and the thread generating the data is 
	/// sensitive to the performance
	///    
	/// Use property Priority to set the task priority. Default priority is 
	/// ThreadPriority.Lowest
	/// 
	/// </summary>
	public abstract class AsyncLogger : Logger
	{
		public AsyncLogger(string name)
			: base(name)
		{
			notStoped = false;
			writer = new Thread(this.Writer);
			writer.Priority = ThreadPriority.Lowest;
			incomingData = new Queue(100);
		}

		/// <summary>
		/// start write file
		/// </summary>
		public virtual bool Start()
		{
			notStoped = true;

			// start Writer (writing thread)
			writer.Start();

			return true;
		}

		/// <summary>
		/// application will call this method for clean up
		/// remove registration from the producer
		/// close the file, remove registration of the data sync from the producer
		/// </summary>
		public virtual void Stop()
		{
			notStoped = false;
			stopped = false;

			lock (this)
			{
				// let the writer thread know that it is time to get out
				Monitor.Pulse(this);

				// help the garbage collector to clean up
				incomingData.Clear();
			}

			// wait for the write thread exit
			while (!stopped)
			{
				lock (this)
				{
					Monitor.Pulse(this);
				}
				Thread.Sleep(100);
			}
		}

		/// <summary>
		/// Application calls this method to add entry to the log
		/// This method is non-blocking - add entry to the queue for 
		/// further processing and get out
		/// </summary>
		public override void AddEntry(object data)
		{
			lock (this)  // protect access to FIFO
			{
				countTrigger++;
				if (incomingData.Count < QueueSize)
				{
					// push the data to the FIFO
					incomingData.Enqueue(data);
				}
				else
				{
					countDropped++;
				}
				Monitor.Pulse(this);
			}
		}

		public ThreadPriority Priority
		{
			set
			{
				writer.Priority = value;
			}

			get
			{
				return writer.Priority;
			}
		}

		/// <value>
		/// Setting this property will limit size of the queue of events
		/// waiting for processing
		/// there is some reasonable default (0.5MB of entries)
		/// </value>
		public int QueueSize
		{
			get;
			set;
		}



		/// <summary>
		/// low priority thread writing the data to the file
		/// </summary>
		protected void Writer()
		{
			object data = null;

			while (notStoped)
			{
				bool dataSet = false;
				lock (this)
				{
					if (incomingData.Count != 0)
					{
						data = incomingData.Dequeue();
						dataSet = true;
					}
					else
					{
						// i want to check the notStoped flag from time to time
						// and make sure that incomingData is empty
						Monitor.Wait(this, 1 * 1000);
					}
				}
				if (dataSet)
				{
					WriteData(data);
				}
			}

			stopped = true;
			Console.WriteLine("Logger " + GetName() + " thread stopped");
		}

		protected abstract void WriteData(object data);

		protected bool notStoped;
		protected bool stopped;

		/// <summary>
		/// Notify() pushes the incoming data here
		/// Thread FileWriter() pulls objects from the list and writes them to the file
		/// Under normal conditions the list is going to be empty most of the time
		/// </summary>
		protected Queue incomingData;
		protected Thread writer;
	}

	/// <summary>
	/// This guy is called once a day in order to keep weights of securities in 
	/// a csv file. 
	/// </summary>
	public class SH161DataLogger
	{
		public SH161DataLogger(string fileName)
		{
			//No append - run it once a day
			_fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read);
			_streamWriter = new StreamWriter(_fileStream);
			_sh161DataToString = new FMRShell.SH161TypeToString(",");
		}

		public void GetAndLogSH161Data(int _sessionId)
		{
			Array x = null;
			K300Class k300Class = new K300Class();
			k300Class.K300SessionId = _sessionId;

			int rc = k300Class.GetSH161(ref x, MadadTypes.TLV25);
			if (rc > 0)
			{
				_streamWriter.WriteLine(_sh161DataToString.Legend);
				for (int i = 0; i < rc; i++)
				{
					_sh161DataToString.Init((SH161Type)x.GetValue(i));
					_streamWriter.WriteLine(_sh161DataToString.Values);
				}
				Console.WriteLine(rc + " SH161 records collected");
			}

			_streamWriter.Close();
			_fileStream.Close();
			_streamWriter.Dispose();
			_fileStream.Dispose();
		}

		FileStream _fileStream;
		StreamWriter _streamWriter;
		FMRShell.SH161TypeToString _sh161DataToString;
	}//SH161DataLogger

	/// <summary>
	///  Objects implemented this interface can be used in the FileLogger
	/// </summary>
	public interface ILoggerData
	{
		string ToLogString();
	}


	/// <summary>
	/// Generic implementation of ILogerData suitable for simple (aka structure) data
	/// ToLogString() will return string containing values of public fields
	/// </summary>
	public class LoggerDataStructure : ILoggerData
	{
		protected string delimiter = ","; //as we use csv (comma-separated values) files as output medium

		/// <summary>
		/// </summary>
		public LoggerDataStructure(string delimiter)
		{
			this.delimiter = delimiter;
		}

		public string ToLogString()
		{
			InitValues();
			return values;
		}

		/// <summary>
		/// Initialize record's Legend - list of all the fields in the data record.
		/// </summary>
		public string GetLegend()
		{
			if (legend != null)
			{
				return legend;
			}

			FieldInfo[] fields = this.GetType().GetFields();
			StringBuilder sbLegend = new StringBuilder(fields.Length * 10);

			foreach (FieldInfo field in fields)
			{
				string name = field.Name;
				sbLegend.Append(name);
				sbLegend.Append(delimiter);
			}

			legend = sbLegend.ToString();

			return legend;
		}

		/// <summary>
		/// Sets property 'Values' - a delimited string containing 
		/// all the fields' values, for the logging purposes.
		/// </summary>
		/// <param name="data">
		/// A <see cref="DataType"/>
		/// </param>
		protected void InitValues()
		{
			FieldInfo[] fields = this.GetType().GetFields();
			StringBuilder sbData = new StringBuilder(fields.Length * 10);

			object o = (object)this;

			foreach (FieldInfo field in fields)
			{
				object val = field.GetValue(o);
				sbData.Append(val.ToString());
				sbData.Append(delimiter);
			}

			this.values = sbData.ToString();
		}



		protected string values;

		protected string legend;
	}


	/// <summary>
	/// this is a simple write to file logger. Just call method Add()
	/// See exampel of usage in the debugFileLoggerTestCallback()
	/// </summary>
	public class FileLogger<DatatType> : AsyncLogger
			  where DatatType : ILoggerData
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="filename">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="append">
		/// A <see cref="System.Boolean"/>
		/// </param>
		/// <param name="legend">
		/// The very first line of the file, typically comma separated names of the fields
		/// can contain version of the data structure. Version of the structure helps to process log files
		/// generated by previous versions of the software
		/// A <see cref="System.String"/>
		/// </param>
		public FileLogger(string name, string filename, bool append, string legend)
			: base(name)
		{
			FileName = filename;
			fileStream = default(FileStream);
			streamWriter = default(StreamWriter);
			this.append = append;
			timeStamped = false;
			this.legend = legend;
			Type = LogType.CSV;
			notStoped = false;

			// I estimate size of FMRShell.MarketData struct 50 bytes
			// AsyncLogger will drop the events after approx 500K of data in the queue
			QueueSize = (500 * 1024) / 50;
		}

		public string FileName
		{
			get;
			protected set;
		}

		public Exception LastException
		{
			get;
			protected set;
		}


		/// <summary>
		/// register notifier in the producer, start write file
		/// returns True if Ok
		/// application will check LastException if the method
		/// returns False
		/// </summary>
		public override bool Start()
		{
			bool result = false;

			// i want a loop here to break from  - i avoid multiple
			// returns this way
			do
			{
				// open file for writing
				try
				{
					if (append) fileStream = new FileStream(FileName, FileMode.Append, FileAccess.Write, FileShare.Read);
					else fileStream = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Read);
					streamWriter = new StreamWriter(fileStream);
				}
				catch (IOException e)
				{
					// store the exception
					LastException = e;
					if (fileStream != default(FileStream))
					{
						fileStream.Close();
						// help Garbage collector to clean up the system resources 
						streamWriter = default(StreamWriter);
						fileStream = default(FileStream);
					}
					// and get out
					break;
				}

				// write legend at the top of the file
				try
				{
					if (legend != null)
					{
						streamWriter.WriteLine(legend);
					}
				}
				catch (IOException e)
				{
					// store the exception
					LastException = e;
					// close the file
					fileStream.Close();
					// help Garbage collector
					streamWriter = default(StreamWriter);
					fileStream = default(FileStream);
					Console.WriteLine(e.ToString());
					// and get out
					break;
				}

				//strat write file
				base.Start();

				first = true;
				result = true;
			}
			while (false);

			return result;
		}

		/// <summary>
		/// application will call this method for clean up
		/// remove registration from the producer
		/// close the file, remove registration of the data sync from the producer
		/// </summary>
		public override void Stop()
		{
			base.Stop();

			if (fileStream != default(FileStream))
			{
				streamWriter.Flush();
				fileStream.Flush();
				// help Garbage collector
				streamWriter = default(StreamWriter);
				fileStream = default(FileStream);
				Console.WriteLine("Logger " + GetName() + " file " + FileName + " closed");
			}
		}

		public override void Dispose()
		{
			if (fileStream != default(FileStream))
			{
				streamWriter.Flush();
				fileStream.Flush();
				fileStream.Close();
			}
			base.Dispose();
		}




		/// <summary>
		/// this method is abstract in the parent class
		/// write data to the file. this method is called from a separate
		/// thread
		/// </summary>
		/// <param name="data">
		/// A <see cref="System.Object"/>
		/// </param>
		protected override void WriteData(object data)
		{
			// I have to decide on format of the log - ASCII or binary 
			// should I write any system info like version the data/software ?
			// at this point only ASCII is supported, no system info
			// write all fields of K300MaofType (data.k3Maof) in one line
			// followed by EOL
			DatatType logData = (DatatType)data;

			string s = logData.ToLogString();
			// write the string to the file
			try
			{
				streamWriter.WriteLine(s);
				// i want to make Flush from time to time
				// the question is when ? or let the OS to manage the things ?
				// _streamWriter.Flush();
				lock (this)
				{
					countLog++;
				}
			}
			catch (ObjectDisposedException e)
			{
				// store the exception
				LastException = e;
				Console.WriteLine(e.ToString());
			}
			catch (IOException e)
			{
				Console.WriteLine(e.ToString());
				// store the exception
				LastException = e;
				// and get out
				Stop();
			}
		}

		bool append;
		FileStream fileStream;
		string legend;
		StreamWriter streamWriter;
		bool first;
	}
}//namespace JQuant
