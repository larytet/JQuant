
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using System.Reflection;
using System.ComponentModel;
using System.Diagnostics;

namespace JQuant
{
	/// <summary>
	/// Can be used to stamp order ID for debug purposes. Order ids are convenient 
	/// to follow in the logs
	/// </summary>
	public class UniqueId
	{
		public static long Get()
		{
			lock (o)
			{
				return (++id);
			}
		}

		private static object o = new Int32();
		private static long id = 0;
	}

	public class ArrayUtils
	{
		public static void SetData(int[] array, int data)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = data;
			}
		}

		public static int[] CreateInitializedArray(int data, int size)
		{
			int[] array = new int[size];
			SetData(array, data);

			return array;
		}

	}

	/// <summary>
	/// C# enum does not allow to reload ToString method
	/// I need patch here 
	/// </summary>
	public class EnumUtils
	{
		/// <summary>
		/// resolves enum - looks for the description
		/// </summary>
		public static string GetDescription(System.Enum value)
		{
			FieldInfo fi = value.GetType().GetField(value.ToString());
			DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (attributes.Length > 0)
				return attributes[0].Description;
			else
				return value.ToString();
		}
	}


	/// <summary>
	/// format output - add blanks, remove trailing blanks, etc.
	/// </summary>
	public class OutputUtils
	{

		public static void PrintCallStack()
		{
			System.Diagnostics.StackTrace stackTrace = new StackTrace();           // get call stack
			System.Diagnostics.StackFrame[] stackFrames = stackTrace.GetFrames();  // get method calls (frames)

			// write call stack method names
			foreach (StackFrame stackFrame in stackFrames)
			{
				System.Console.WriteLine(stackFrame.GetMethod().Name);   // write method name
			}
		}

		/// <summary>
		/// add trailing blanks to the integer if neccessary 
		/// </summary>
		static public string FormatField(long val, int fieldSize)
		{
			string s = FormatField("" + val, fieldSize);

			return s;
		}

		/// <summary>
		/// add trailing blanks to the string if neccessary 
		/// </summary>
		static public string FormatField(string val, int fieldSize)
		{
			string s = FormatField(val, fieldSize, ' ');

			return s;
		}

		static public string FormatField(string val, int fieldSize, char filler)
		{
			StringBuilder s = new StringBuilder("" + val, fieldSize);

			int count = s.Length;
			for (int i = count; i < fieldSize; i++)
			{
				s.Insert(0, filler);
			}

			return s.ToString();
		}

		/// <summary>
		/// add trailing blanks to the integer if neccessary 
		/// </summary>
		static public string FormatField(double val, int fieldSize)
		{
			string s = FormatField("" + val, fieldSize);

			return s;
		}

		static public string RemoveLeadingBlanks(string s)
		{
			int blank_idx = s.IndexOf(' ');
			while (blank_idx == 0)
			{
				s.Remove(0, 1);
				blank_idx = s.IndexOf(' ');
			}
			return s;
		}

		/// <summary>
		/// Removes all the leading and trailing blanks and
		/// replaces multiple blanks inside the string 
		/// with a single blank
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string CleanBlanks(string s)
		{
			return Regex.Replace(s.Trim(), @"\s+", " ");
		}

		/// <summary>
		/// Create string like this
		/// "{
		///    "Weight": 40.0,
		///    "Expiry": "2010, 1, 28",
		///    "Price": 81.09,
		/// }"
		/// </summary>
		public static string GetJSON(IResourceStatistics resStat)
		{
			System.Collections.ArrayList names;
			System.Collections.ArrayList values;
			resStat.GetEventCounters(out names, out values);

			StringBuilder sb = new StringBuilder(20 * names.Count);

			sb.Append("{");

			int count = values.Count;
			for (int i = 0; i < count; i++)
			{
				JSONAddField(sb, names[i], values[i]);
			}

			sb.Append("}");

			return sb.ToString();
		}

		/// <summary>
		/// Build JSON formated string for an arbitrary object
		/// no arrays are processes in the phase
		/// 
		/// In the .NET3.5 there is support for JSON serialization
		/// </summary>
		public static string GetJSON(object o, string name)
		{
			FieldInfo[] fields = o.GetType().GetFields();

			System.Text.StringBuilder sb = new System.Text.StringBuilder(fields.Length * 20);


			GetJSON(sb, name, o);


			string json = sb.ToString();
			return json;
		}

		protected static void GetJSON(System.Text.StringBuilder sb, string name, object o)
		{
			if (o.GetType().IsArray)
			{
				JSONAddArray(sb, name, o);
			}
			else 
			{
				JSONAddFields(sb, name, o);
			}

			sb.Remove(sb.Length-1, 1);  // remove last comma
		}

		protected static void JSONAddFields(StringBuilder sb, object name, object data)
		{
			if (data.GetType().IsPrimitive)
			{
				JSONAddField(sb, data);
			}
			else
			{
				FieldInfo[] fields = data.GetType().GetFields();
	
				sb.Append("{");
				foreach (FieldInfo field in fields)
				{
					string fieldName = field.Name;
					object val = field.GetValue(data);
	
					if (val.GetType().IsArray)
					{
						sb.Append(fieldName);
						sb.Append(":");
						JSONAddArray(sb, fieldName, val);
					}
					else 
					{
						JSONAddField(sb, fieldName, val);
					}
				}
				sb.Remove(sb.Length-1, 1);  // remove last comma
				sb.Append("},");
			}
		}

		/// <summary>
		///Array in JSON looks like this
		///   { identifier: 'name',
 		///       items: [
		///             { name: 'Adobo', aisle: 'Mexican', price: 3.01 },
		///             { name: 'Balsamic vinegar', aisle: 'Condiments', price: 4.01 }
		///             ]
		///   }
		/// More examles
		/// {identifier:'els',label:'els',items: [{d1:'1',d2:'2'},{d1:'3',d2:'4'}]}
		/// {identifier:'els',label:'els',items: [{d1:'1',d2:'2',items:[1,2,3]},{d1:'3',d2:'4',items:[1,2,3]}]}
		/// {identifier:'els',label:'els',items: [{d1:'1',d2:'2',bid:{items:[1,2,3]}},{d1:'3',d2:'4',bid:{items:[1,2,3]}}]}
		/// {identifier:'options',label:'options',items:[{strike:'100',id:'0',options:{identifier:'ask',label:'ask',items:[{price:'727',size:'21'},{price:'893',size:'41'},{price:'675',size:'46'}]},options:{identifier:'bid',label:'bid',items:[{price:'856',size:'44'},{price:'649',size:'37'},{price:'891',size:'17'}]},isPut:'1'},{strike:'200',id:'1',options:{identifier:'ask',label:'ask',items:[{price:'631',size:'24'},{price:'558',size:'20'},{price:'841',size:'6'}]},options:{identifier:'bid',label:'bid',items:[{price:'945',size:'17'},{price:'708',size:'43'},{price:'713',size:'23'}]},isPut:'0'}]}
		/// </summary>
		protected static void JSONAddArray(StringBuilder sb, object name, object data)
		{
			sb.Append("{identifier:'");
			sb.Append(name);
			sb.Append("',label:'");
			sb.Append(name);
			sb.Append("',items:[");
			foreach (object el in (System.Collections.IEnumerable)data)
			{
				JSONAddFields(sb, name, el);
			}
			sb.Remove(sb.Length-1, 1);  // remove last comma
			sb.Append("]},");
		}

		protected static void JSONAddField(StringBuilder sb, object name, object data)
		{
			//sb.Append("'");
			sb.Append(name);
			//sb.Append("'");
			sb.Append(":");

			// i can ask here if (JSONIsString(data)), but i will save CPU cycles
			// i am going to add '"' around the value in all cases
			sb.Append("'");
			sb.Append(data);
			sb.Append("',");
		}

		protected static void JSONAddField(StringBuilder sb, object data)
		{
			sb.Append(data);
			sb.Append(",");
		}

		protected static bool JSONIsString(object data)
		{
			bool res = false;

			System.Type t = data.GetType();

			res = res || t.Equals(typeof(System.Int32));
			res = res || t.Equals(typeof(System.Int16));
			res = res || t.Equals(typeof(System.Boolean));
			res = res || t.Equals(typeof(System.Int64));
			res = res || t.Equals(typeof(int));
			res = res || t.Equals(typeof(bool));

			return !res;
		}



	}

	public class Convert
	{
		public static int BoolToInt(bool b)
		{
			if (b) return 1;
			else return 0;
		}

		public static double StrToDouble(string s, double defaultValue)
		{
			double result = defaultValue;
			string t = s.Trim();
			try
			{
				if (t != "") result = Double.Parse(t);
			}
			catch (FormatException)
			{
				Console.WriteLine("Failed to parse string '" + t + "'");
			}
			return result;
		}

		public static long StrToLong(string s, long defaultValue)
		{
			long result = defaultValue;
			string t = s.Trim();
			try
			{
				if (t != "") result = Int64.Parse(t);
			}
			catch (FormatException)
			{
				Console.WriteLine("Failed to parse string '" + t + "'");
			}
			return result;
		}

		public static int StrToInt(string s, int defaultValue)
		{
			int result = defaultValue;
			string t = s.Trim();
			try
			{
				if (t != "") result = Int32.Parse(t);
			}
			catch (FormatException)
			{
				Console.WriteLine("Failed to parse string '" + t + "'");
			}
			return result;
		}

		public static int StrToInt(string s)
		{
			int result = StrToInt(s, 0);
			return result;
		}

		public static long StrToLong(string s)
		{
			long result = StrToLong(s, 0);
			return result;
		}

		public static double StrToDouble(string s)
		{
			double result = StrToDouble(s, 0.0);
			return result;
		}

		public static DateTime StrExDateToDateTime(string s, DateTime defaultValue)
		{
			int year = defaultValue.Year;
			int month = defaultValue.Month;
			int day = defaultValue.Day;

			string t = s.Trim();

			year = StrToInt(t.Substring(0, 4));
			month = StrToInt(t.Substring(4, 2));
			day = StrToInt(t.Substring(6, 2));

			DateTime result = new DateTime(year, month, day);

			return result;
		}

		public static DateTime StrExDateToDateTime(string s)
		{
			return StrExDateToDateTime(s, new DateTime(1900, 1, 1));
		}
	}

	/// <summary>
	/// converts a struct to a string with specified delimiters
	/// </summary>
	public class StructToString<StructType>
	{
		public StructToString(string delimiter)
		{
			this.delimiter = delimiter;
			Type t = typeof(StructType);
			fields = t.GetFields();
			InitLegend();
		}

		/// <value>
		/// keeps list of the fields (field names) separated by the Delimiter 
		/// </value>
		public string Legend
		{
			get;
			protected set;
		}

		/// <value>
		/// keeps a string with values separated by the delimiter
		/// </value>
		public string Values
		{
			get;
			protected set;
		}

		/// <value>
		/// delimiter used to separate the fields
		/// If set the object will regenerate strings
		/// </value>
		public string Delimiter
		{
			get
			{
				return this.delimiter;
			}

			set
			{
				// i can check if the delimiter changed indeed
				delimiter = value;
				Init(data);
			}
		}

		/// <value>
		/// This field is true if Init() was called
		/// </value>
		public bool IsInitialized
		{
			get;
			protected set;
		}

		protected void InitLegend()
		{
			StringBuilder sbLegend = new StringBuilder(fields.Length * 10);

			foreach (FieldInfo field in fields)
			{
				string name = field.Name;
				sbLegend.Append(name);
				sbLegend.Append(delimiter);
			}

			// remove last delimiter
			sbLegend.Remove(sbLegend.Length - delimiter.Length, delimiter.Length);

			Legend = sbLegend.ToString();
		}

		public void Init(StructType data)
		{
			this.data = data;
			StringBuilder sbData = new StringBuilder(fields.Length * 10);

			// i do boxing only once
			object o = data;

			foreach (FieldInfo field in fields)
			{
				object val = field.GetValue(o);
				if (val != null)
					sbData.Append(val.ToString());
				else
					sbData.Append("");
				sbData.Append(delimiter);
			}

			// remove last delimiter
			sbData.Remove(sbData.Length - delimiter.Length, delimiter.Length);

			Values = sbData.ToString();

			IsInitialized = true;
		}

		protected FieldInfo[] fields;
		protected StructType data;
		protected string delimiter;
	}


	public interface IRandomString
	{
		string Next();
	}

	public class RandomNumericalString : IRandomString
	{
		public RandomNumericalString(int min, int max)
		{
			rand = new Random();
			this.min = min;
			this.max = max;
		}

		public string Next()
		{
			int length = rand.Next(min, max);

			return length.ToString();
		}

		Random rand;
		int min;
		int max;
	}

	public class RandomString : IRandomString
	{
		/// <summary>
		/// create one object and call Next() to get a random string
		/// will generate random strings in the specified length range
		/// </summary>
		/// <param name="minLength">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <param name="maxLength">
		/// A <see cref="System.Int32"/>
		/// </param>
		public RandomString(int minLength, int maxLength)
		{
			rand = new Random();
			this.minLength = minLength;
			this.maxLength = maxLength;
		}

		/// <summary>
		/// Build a random string (for id, login, password...)
		/// </summary>
		public string Next()
		{
			int length = rand.Next(minLength, maxLength);
			StringBuilder tempString = new StringBuilder(Guid.NewGuid().ToString());

			tempString = tempString.Replace("-", "");

			while (tempString.Length < length)
			{
				tempString.Append(tempString);
			}

			if (length < tempString.Length)
			{
				tempString = tempString.Remove(0, tempString.Length - length);
			}

			return tempString.ToString();
		}

		Random rand;
		int minLength;
		int maxLength;
	}

	interface ITimeStamp
	{
		DateTime Date
		{
			get;
			set;
		}

		//// <value>
		/// always in microseconds 
		/// </value>
		long Ticks
		{
			get;
			set;
		}
	}

	class TimeUtils
	{
		public static void TimeStamp(ref ITimeStamp o)
		{
			o.Date = System.DateTime.Now;
#if WINDOWS
            o.Ticks = DateTime.Now.Ticks;
#else
			// in Linux 10 ticks is a micro
			o.Ticks = (long)((double)DateTime.Now.Ticks / (double)(10 * 1));
#endif
		}

	}

	/// <summary>
	/// Returns fixed time. In Windows DateTime.Now returns tikcs rounded to 15ms
	/// System tick (Stopwatch) is not real time and drifts by approximately 20s in a day
	/// Property DateTimePrecise.Now returns fixed value of DateTime.Now
	/// I add elapsed ticks as returned by Stopwatch to the base time. Base time is the time
	/// constructor is called.
	/// This is a sealed class to help code optimization
	/// 
	/// In the 1s timer this.UtcNow is called and rift between stopwatch and "real-time" 
	/// returned by DateTime.Now is calculated.
	/// In the  UtcNow the current date is calculated as Base + StopwatchElapsedTicks
	/// Every time UtcNow is called it will shift Base by small amount of ticks until
	/// drift is not less than 5ms. At this point the stopwatch is assumed precise.
	/// If drift growth above 5ms UtcNow will start to fix the Base again
	///
	/// There is a trick. If stopwatch is slower than the real time clock I can 
	/// always add ticks as neccessary. But if the stopwatch is faster I have to
	/// take care of situation where the time returned by UtcNow runs backward. I decrease
	/// the number of ticks by no more than number of elapsed ticks from the most
	/// recent fix. In other words if the stopwatch is faster than real time clock
	/// i will stop it (return the same value) for some short period until drift
	/// does not drop back to under 5ms
	///
	/// Most of the time the drift remains under 5ms and no computation is neccessary.
	/// The whole business of drift can be moved to the context of 1s timer. The timer
	/// can be made shorter. This approach can potentially save some CPU cycles.
	/// 
	/// </summary>
	public sealed class DateTimePrecise
	{
		private DateTimePrecise()
		{
			// for some reason very first call to DateTime.UtcNow takes lot of time
			// do dummy call first 
			DummyCall();

			drift = 0;
			STOPWATCH_FREQ = Stopwatch.Frequency;


			stopwatch = Stopwatch.StartNew();
			stopwatch.Start();

			// 1s timer makes sure that UtcNow is called from time to time
			// and UtcNow is the metod which slowly fixes the drift if such occurs
			// recalculate the drift - compare this.UtcNow with real-time DateTime.UtcNow
			timer1s = new System.Timers.Timer();
			timer1s.AutoReset = true;
			timer1s.Interval = 1000;
			timer1s.Elapsed += new ElapsedEventHandler(PollUtc);
			pollUtcCount = 0;
			deltas = new List<long>(STAT_SIZE);


			swBase = swLastObserved = stopwatch.ElapsedTicks;
			dtBase = DateTime.UtcNow;

			// Start timers
			timer1s.Start();
			// timer10s.Start();
		}

		public static void Init()
		{
			dateTimePrecise = new DateTimePrecise();
		}

		public static DateTimePrecise GetInstance()
		{
			return dateTimePrecise;
		}

		/// <summary>
		/// Make sure that UtcNow is called at least once every second
		/// Calculate the clock drift - average of 8 samples
		/// </summary>
		private static void PollUtc(object source, ElapsedEventArgs e)
		{
			// UtcNow property is faster than Now property
			long delta = DateTime.UtcNow.Ticks - DateTimePrecise.UtcNow.Ticks;

			deltas.Add(delta);


			// keep only STAT_SIZE latest reads in the list
			if (deltas.Count > STAT_SIZE)
			{
				deltas.RemoveAt(0);
			}

			// every 8 calls calculate average drift of 8 samples
			// and set drift
			// In the context of UtcNow the base time will be modified
			// to fix the drift
			if ((pollUtcCount & STAT_SIZE_MASK) == STAT_SIZE_MASK)
			{
				long drift = (long)Math.Round(deltas.Average());

				lock (dateTimePrecise)
				{
					DateTimePrecise.drift = drift;
				}
				//                System.Console.Write("."+drift/10 +".");              
			}

			pollUtcCount++;
		}

		/// Returns the current date and time, just like DateTime.UtcNow.
		public static DateTime UtcNow
		{
			get
			{
				// get current value from the stopwatch
				long swObserved = stopwatch.ElapsedTicks - swBase;

				lock (dateTimePrecise)
				{
					// now I have to "fix" base time

					// drift less than 10ms - nothing to fix. this outcome is most likely
					// and lock time is going to be very short
					if (Math.Abs(drift) < 5 * TICKS_IN_MS)
					{
					}
					// i can increase time - there is no problem here
					else if (drift > 0)
					{
						long delta = StopwatchToTick(swObserved - swLastObserved);
						delta = MaxShift(delta); delta = Math.Min(delta, drift); delta = Math.Min(delta, MAX_SHIFT);
						dtBase = dtBase.AddTicks(delta);
						drift -= delta;
					}
					// i can decrease time by no more than (swObserved - swLastObserved)
					else if (drift < 0)
					{
						long delta = StopwatchToTick(swObserved - swLastObserved);
						delta = MaxShift(delta); delta = Math.Min(delta, Math.Abs(drift)); delta = Math.Min(delta, MAX_SHIFT);
						dtBase = dtBase.AddTicks(-delta);
						drift += delta;
					}
					swLastObserved = swObserved;
				}  // lock

				DateTime dt = dtBase.AddTicks(StopwatchToTick(swObserved));


				return dt;
			}
		}

		/// <summary>
		/// Depending on the number of elapsed ticks returns 0.5%-10% of the value
		/// </summary>
		private static long MaxShift(long ticks)
		{
			if (ticks > 5000)  // caluclate 0.5% of the delta
			{                  // 5000 ticks is 0.5ms from the last call to UtcNow
				ticks = (ticks * 5) / 1000;
			}
			else if (ticks > 50)
			{
				ticks = ticks / 10; // 10% of the delta
			}
			else  // very unlikely - less than 5micro between two calls
			{     // use the lowest between delta and 5ticks
				ticks = Math.Min(ticks, 5);
			}

			return ticks;
		}


		private static long StopwatchToTick(long value)
		{
			value = (value * TICKS_FREQ) / STOPWATCH_FREQ;
			return value;
		}

		/// Returns the current date and time, just like DateTime.Now.
		public static DateTime Now
		{
			get
			{
				return UtcNow.ToLocalTime();
			}
		}

		/// <summary>
		/// for some reason very first call to DateTime.UtcNow takes lot of time
		/// i call this method in the constructor
		/// </summary>
		private DateTime DummyCall()
		{
			return DateTime.UtcNow;
		}

		private static Stopwatch stopwatch;

		private static long swBase;
		private static long swLastObserved;
		private static DateTime dtBase;
		private static long drift;


		private const long TICKS_FREQ = 10000000;
		public const long TICKS_IN_MS = TICKS_FREQ / 1000;

		// i am not going to jump too fast - 5ms shift is max
		private const long MAX_SHIFT = (TICKS_FREQ / 200);
		private static long STOPWATCH_FREQ;

		private static DateTimePrecise dateTimePrecise;

		private static int pollUtcCount;
		// i collect samples of drifts
		private static List<long> deltas;

		// user power of two
		private static int STAT_SIZE = 8;
		private static int STAT_SIZE_MASK = STAT_SIZE - 1;

		private System.Timers.Timer timer1s;
	}


	#region StatUtils;
	/// <summary>
	/// Making different statistical computations. 
	/// For the moment implemented for an integer list, need to convert it to any numerical type.
	/// </summary>
	public class StatUtils
	{

		#region List Statistics;

		/// <summary>
		/// Length of a List
		/// </summary>
		/// <param name="lst">List of <see cref="System.Double"/></param>
		/// <returns><see cref="System.Int32"/></returns>
		public static int Length(List<double> lst)
		{
			return lst.Count;
		}

		/// <summary>
		/// Average (mean) of a list of doubles
		/// </summary>
		/// <param name="lst">List of <see cref="System.Double"/></param>
		/// <returns><see cref="System.Double"/></returns>
		public static double Mean(List<double> lst)
		{
			return lst.Average();
		}

		/// <summary>
		/// Standard Deviation of a List of Doubles
		/// </summary>
		/// <param name="lst">List of <see cref="System.Double"/></param>
		/// <returns>see cref="System.Double"/></returns>
		public static double StdDev(List<double> lst)
		{
			double m = Mean(lst);
			double sd = 0.0;
			foreach (int l in lst)
			{
				sd += (l - m) * (l - m);
			}
			return Math.Sqrt(sd) / (Length(lst) - 1);
		}

		/// <summary>
		/// Maximum of List of doubles
		/// </summary>
		/// <param name="lst">List of <see cref="System.Double"/></param>
		/// <returns>see cref="System.Double"/></returns>
		public static double Max(List<double> lst)
		{
			return lst.Max();
		}

		/// <summary>
		/// Minimum of List of doubles
		/// </summary>
		/// <param name="lst">List of <see cref="System.Double"/></param>
		/// <returns>see cref="System.Double"/></returns>
		public static double Min(List<double> lst)
		{
			return lst.Min();
		}

		#endregion;


		#region Math & Stat formulae;
		//////////////////////////////////////////////////////////////////////////////
		//                  Statistical and mathematical tools
		//----------------------------------------------------------------------------

		/// <summary>
		/// Normal Distribution density function.
		/// 1st derivative of the cumilative function
		/// </summary>
		/// <param name="x">Critical Value, <see cref="System.Double"/></param>
		/// <returns>Cumulative Normal Dist., <see cref="System.Double"/></returns>
		public static double NormalCalcPrime(double x)
		{
			const double c = 0.39894228;
			return c * Math.Exp(-x * x / 2.0);
		}//NormalCalcPrime

		/// <summary>
		/// Calculates cumulative Normal distribution (CDF).
		/// </summary>
		/// <param name="x">Critical value, <see cref="System.Double"/></param>
		/// <returns>Normal CDF (probability), <see cref="System.Double"/></returns>
		public static double NormalCalc(double x)
		{
			const double b1 = 0.319381530;
			const double b2 = -0.356563782;
			const double b3 = 1.781477937;
			const double b4 = -1.821255978;
			const double b5 = 1.330274429;
			const double p = 0.2316419;
			const double c = 0.39894228;
			if (x >= 0.0)
			{
				double t = 1.0 / (1.0 + p * x);
				return (1.0 - c * Math.Exp(-x * x / 2.0) *
					t * (t * (t * (t * (t * b5 + b4) + b3) + b2) + b1));
			}
			else
			{
				double t = 1.0 / (1.0 - p * x);
				return (c * Math.Exp(-x * x / 2.0) *
					t * (t * (t * (t * (t * b5 + b4) + b3) + b2) + b1));
			}
		}// NormalCalc

		#endregion;

		#region Black-Scholes Merton;
		/// <summary>
		/// BSM calculator for vanilla European options
		/// </summary>
		/// <param name="type">Either CALL or PUT, <see cref="JQuant.OptionType"/></param>
		/// <param name="vol"><see cref="System.Double"/> annualized underlying volatility</param>
		/// <param name="rf"><see cref="System.Double"/> annualized (risk-free) inerest rate</param>
		/// <param name="strike"><see cref="System.Double"/> Strike price</param>
		/// <param name="uPrice"><see cref="System.Double"/> Underlying asset price</param>
		/// <param name="T"><see cref="System.Double"/> Time to expiration (in years)</param>
		/// <returns>Options' BSM price, in case of error returns -1</returns>
		public static double CalcBSPrice(
			OptionType type,
			double vol,
			double rf,
			double strike,
			double uPrice,
			double T
			)
		{
			double d1 = (Math.Log(uPrice / strike) + (rf + vol * vol / 2) * T) / (vol * Math.Sqrt(T));
			double d2 = d1 - vol * Math.Sqrt(T);

			if (type == OptionType.CALL)
				return (uPrice * NormalCalc(d1) - strike * Math.Exp(-rf * T) * NormalCalc(d2));
			else
				return (strike * Math.Exp(-rf * T) * NormalCalc(-d2) - uPrice * NormalCalc(-d1));

		}//CalcBSPrice

		#endregion;

		#region BSM analytical Greeks;

		/// <summary>
		/// Analytical BSM Delta, 1st partial derivative 
		/// with respect to underlying price
		/// </summary>
		/// <param name="type">Either CALL or PUT, <see cref="JQuant.OptionType"/></param>
		/// <param name="vol"><see cref="System.Double"/> annualized underlying volatility</param>
		/// <param name="rf"><see cref="System.Double"/> annualized (risk-free) inerest rate</param>
		/// <param name="strike"><see cref="System.Double"/> Strike</param>
		/// <param name="uPrice"><see cref="System.Double"/> Underlying price</param>
		/// <param name="T"><see cref="System.Double"/> Time to expiration (in years)</param>
		/// <returns><see cref="System.Double"/> Delta, in case of error returns -99999</returns>
		public static double CalcDelta(
			OptionType type,
			double vol,
			double rf,
			double strike,
			double uPrice,
			double T
			)
		{
			double d1 = (Math.Log(uPrice / strike) + (rf + vol * vol / 2.0) * T) / (vol * Math.Sqrt(T));

			if (type == OptionType.CALL) return StatUtils.NormalCalc(d1);
			else return -StatUtils.NormalCalc(-d1);

		}//CalcDelta

		/// <summary>
		/// BSM analytical Gamma, the second partial derivative with respect
		/// to the underlying price, Delta sensitivity to the changes in the 
		/// underlying. Note that it's the same for both Call and Put.
		/// </summary>
		/// <param name="vol"><see cref="System.Double"/> annualized underlying volatility</param>
		/// <param name="rf"><see cref="System.Double"/> annualized (risk-free) inerest rate</param>
		/// <param name="strike"><see cref="System.Double"/>Strike</param>
		/// <param name="uPrice"><see cref="System.Double"/>Underlying price</param>
		/// <param name="T"><see cref="System.Double"/> Time to expiration (in years)</param>
		/// <returns><see cref="System.Double"/>Gamma</returns>
		public static double CalcGamma(
			double vol,
			double rf,
			double strike,
			double uPrice,
			double T
			)
		{
			double d1 = (Math.Log(uPrice / strike) + (rf + vol * vol / 2.0) * T) / (vol * Math.Sqrt(T));
			return StatUtils.NormalCalcPrime(d1) / (uPrice * vol * Math.Sqrt(T));
		}//CalcGamma

		/// <summary>
		/// Theta, the 1st partial derivative with respect to T (time to expiration).
		/// Option's sensitivity to the pass of time.
		/// </summary>
		/// <param name="type">Either CALL or PUT, <see cref="JQuant.OptionType"/></param>
		/// <param name="vol"><see cref="System.Double"/>underlying volatility</param>
		/// <param name="rf"><see cref="System.Double"/> annualized (risk-free) inerest rate</param>
		/// <param name="strike"><see cref="System.Double"/>Strike</param>
		/// <param name="uPrice"><see cref="System.Double"/>Underlying price</param>
		/// <param name="T"><see cref="System.Double"/> Time to expiration (in years)</param>
		/// <returns><see cref="System.Double"/>Theta</returns>
		public static double CalcTheta(
			OptionType type,
			double vol,
			double rf,
			double strike,
			double uPrice,
			double T
			)
		{
			double d1 = (Math.Log(uPrice / strike) + (rf + vol * vol / 2.0) * T) / (vol * Math.Sqrt(T));
			double d2 = d1 - vol * Math.Sqrt(T);

			if (type == OptionType.CALL)
				return (-uPrice * NormalCalcPrime(d1) * vol) / (2.0 * Math.Sqrt(T)) -
					rf * strike * Math.Exp(-rf * T) * NormalCalc(d2);
			else
				return (-uPrice * NormalCalcPrime(d1) * vol) / (2.0 * Math.Sqrt(T)) +
					rf * strike * Math.Exp(-rf * T) * NormalCalc(-d2);

		}//CalcTheta

		/// <summary>
		/// Vega, 1st partial derivative with respect to the volatility.
		/// Note that it's the same for both Call and Put
		/// </summary>
		/// <param name="vol"><see cref="System.Double"/>underlying volatility</param>
		/// <param name="rf"><see cref="System.Double"/> annualized (risk-free) inerest rate</param>
		/// <param name="strike"><see cref="System.Double"/>Strike</param>
		/// <param name="uPrice"><see cref="System.Double"/> underlying price</param>
		/// <param name="T"><see cref="System.Double"/> Time to expiration (in years)</param>
		/// <returns><see cref="System.Double"/>Vega</returns>
		public static double CalcVega(
			double vol,
			double rf,
			double strike,
			double uPrice,
			double T
			)
		{
			double d1 = (Math.Log(uPrice / strike) + (rf + vol * vol / 2.0) * T) / (vol * Math.Sqrt(T));
			return uPrice * NormalCalcPrime(d1) * Math.Sqrt(T);
		}//CalcVega

		/// <summary>
		/// implied volatility using Newton-Raphson method
		/// </summary>
		/// <param name="type">Either CALL or PUT, <see cref="JQuant.OptionType"/></param>
		/// <param name="premium"><see cref="System.Double"/> Option's premium</param>
		/// <param name="rf"><see cref="System.Double"/> annualized (risk-free) inerest rate</param>
		/// <param name="strike"><see cref="System.Double"/> Strike</param>
		/// <param name="uPrice"><see cref="System.Double"/> Underlying price</param>
		/// <param name="T"><see cref="System.Double"/> Time to expiration (in years)</param>
		/// <returns></returns>
		public static double CalcIV(
			OptionType type,
			double premium,
			double rf,
			double strike,
			double uPrice,
			double T
			)
		{
			const double ACCURACY = 1.0e-6;
			double tryIV =             // initial value of volatility
				Math.Pow(Math.Abs(Math.Log(uPrice / strike) + rf * T) * 2 / T, 0.5);
			double tryPremium = CalcBSPrice(type, tryIV, rf, strike, uPrice, T);
			double Vega = CalcVega(tryIV, rf, strike, uPrice, T);

			while (Math.Abs(premium - tryPremium) > ACCURACY)
			{
				tryIV = tryIV - ((tryPremium - premium) / Vega);
				tryPremium = CalcBSPrice(type, tryIV, rf, strike, uPrice, T);
				Vega = CalcVega(tryIV, rf, strike, uPrice, T);
			}

			return tryIV;
		}//CalcIV

		#endregion;

		#region Numerical Greeks;

		//////////////////////////////////////////////////////////////////////////////
		//            Numerical Greeks, using finite-difference method
		//----------------------------------------------------------------------------

		/// <summary>
		/// Numerical Delta, by finite-difference
		/// </summary>
		/// <param name="type">Either CALL or PUT, <see cref="JQuant.OptionType"/></param>
		/// <param name="vol"><see cref="System.Double"/> volatility, assumes constant over small changes</param>
		/// <param name="rf"><see cref="System.Double"/> annualized (risk-free) inerest rate</param>
		/// <param name="strike"><see cref="System.Double"/> Strike</param>
		/// <param name="S"><see cref="System.Double"/> underlying price</param>
		/// <param name="T"><see cref="System.Double"/> Time to expiration (in years)</param>
		/// <param name="dS"><see cref="System.Double"/> d(underlying price)</param>
		/// <returns>Numerical Delta</returns>
		public static double CalcNDelta(
			OptionType type,
			double vol,
			double rf,
			double strike,
			double S,
			double T,
			double dS
			)
		{
			return (CalcBSPrice(type, vol, rf, strike, S + dS, T) -
				CalcBSPrice(type, vol, rf, strike, S - dS, T)) / (2 * dS);
		}

		/// <summary>
		/// Overloaded numerical Delta, to address
		/// volatility skew impact
		/// </summary>
		/// <param name="type">Either CALL or PUT, <see cref="JQuant.OptionType"/></param>
		/// <param name="rf"><see cref="System.Double"/> annualized (risk-free) inerest rate</param>
		/// <param name="strike"><see cref="System.Double"/> Strike price</param>
		/// <param name="S"><see cref="System.Double"/> underlying asset price</param>
		/// <param name="T"><see cref="System.Double"/> time to expiration (in years)</param>
		/// <param name="dS"><see cref="System.Double"/> d(underlying price)</param>
		/// <param name="vol_up"><see cref="System.Double"/> underlying-up-volatility</param>
		/// <param name="vol_down"><see cref="System.Double"/> underlying-down-volatility</param>
		/// <returns><see cref="System.Double"/> Numerical Delta</returns>
		public static double CalcNDelta(
			OptionType type,
			double rf,
			double strike,
			double S,
			double T,
			double dS,
			double vol_up,
			double vol_down
			)
		{
			return (CalcBSPrice(type, vol_up, rf, strike, S + dS, T) -
				CalcBSPrice(type, vol_down, rf, strike, S - dS, T)) / (2 * dS);
		}

		/// <summary>
		/// Numerical Gamma using finite-difference
		/// </summary>
		/// <param name="type">Either CALL or PUT, <see cref="JQuant.OptionType"/></param>
		/// <param name="vol"><see cref="System.Double"/> volatility, assumes constant over small changes</param>
		/// <param name="rf"><see cref="System.Double"/> annualized (risk-free) inerest rate</param>
		/// <param name="strike"><see cref="System.Double"/> Strike price</param>
		/// <param name="S"><see cref="System.Double"/> underlying asset price</param>
		/// <param name="T"><see cref="System.Double"/> time to expiration (in years)</param>
		/// <param name="dS"><see cref="System.Double"/> d(underlying price)</param>
		/// <returns><see cref="System.Double"/> Numerical Gamma</returns>
		public static double CalcNGamma(
			OptionType type,
			double vol,
			double rf,
			double strike,
			double S,
			double T,
			double dS
			)
		{
			return (CalcBSPrice(type, vol, rf, strike, S + dS, T) -
				2.0 * CalcBSPrice(type, vol, rf, strike, S, T) +
				CalcBSPrice(type, vol, rf, strike, S - dS, T)) / (dS * dS);
		}

		/// <summary>
		/// Numerical Gamma - overloaded mathod
		/// addressing volatility skew.
		/// Computed by finite-difference
		/// </summary>
		/// <param name="type">Either CALL or PUT, <see cref="JQuant.OptionType"/></param>
		/// <param name="vol"><see cref="System.Double"/> volatility, assumes constant over small changes</param>
		/// <param name="rf"><see cref="System.Double"/> annualized (risk-free) inerest rate</param>
		/// <param name="strike"><see cref="System.Double"/> Strike price</param>
		/// <param name="S"><see cref="System.Double"/> underlying asset price</param>
		/// <param name="T"><see cref="System.Double"/> time to expiration (in years)</param>
		/// <param name="dS"><see cref="System.Double"/> d(underlying price)</param>
		/// <param name="vol_up"><see cref="System.Double"/> underlying-up-volatility</param>
		/// <param name="vol_down"><see cref="System.Double"/> underlying-down-volatility</param>
		/// <returns><see cref="System.Double"/> Numerical Gamma</returns>
		public static double CalcNGamma(
			OptionType type,
			double rf,
			double vol,
			double strike,
			double S,
			double T,
			double dS,
			double vol_up,
			double vol_down)
		{
			return (CalcBSPrice(type, vol_up, rf, strike, S + dS, T) -
				2.0 * CalcBSPrice(type, vol, rf, strike, S, T) +
				CalcBSPrice(type, vol_down, rf, strike, S - dS, T)) / (dS * dS);
		}


		/// <summary>
		/// Numerical Theta, by finite-difference
		/// </summary>
		/// <param name="type">Either CALL or PUT, <see cref="JQuant.OptionType"/></param>
		/// <param name="vol"><see cref="System.Double"/> volatility, assumes constant over small changes</param>
		/// <param name="rf"><see cref="System.Double"/> annualized (risk-free) inerest rate</param>
		/// <param name="strike"><see cref="System.Double"/> Strike price</param>
		/// <param name="S"><see cref="System.Double"/> underlying asset price</param>
		/// <param name="T"><see cref="System.Double"/> time to expiration (in years)</param>
		/// <param name="dT"><see cref="System.Double"/> d(Time to expiration) --in years. Note here that time
		/// always goes forward</param>
		/// <returns><see cref="System.Double"/> Numerical Theta</returns>
		public static double CalcNTheta(
			OptionType type,
			double vol,
			double rf,
			double strike,
			double S,
			double T,
			double dT
			)
		{
			return (CalcBSPrice(type, vol, rf, strike, S, T) -
				CalcBSPrice(type, vol, rf, strike, S, T - dT)) / (dT);
		}



		/// <summary>
		/// Numerical Vega, by finite-difference
		/// </summary>
		/// <param name="type">Either CALL or PUT, <see cref="JQuant.OptionType"/></param>
		/// <param name="vol"><see cref="System.Double"/> volatility, assumes constant over small changes</param>
		/// <param name="rf"><see cref="System.Double"/> annualized (risk-free) inerest rate</param>
		/// <param name="strike"><see cref="System.Double"/> Strike</param>
		/// <param name="S"><see cref="System.Double"/> underlying price</param>
		/// <param name="T"><see cref="System.Double"/> Time to expiration (in years)</param>
		/// <param name="dV"><see cref="System.Double"/> d(volatility)</param>
		/// <returns>Numerical Vega</returns>
		public static double CalcNVega(
			OptionType type,
			double vol,
			double rf,
			double strike,
			double S,
			double T,
			double dV
			)
		{
			return (CalcBSPrice(type, vol + dV, rf, strike, S, T) -
				CalcBSPrice(type, vol - dV, rf, strike, S, T)) / (2 * dV);
		}

		#endregion;


	}
	#endregion;

	#region TASEUtils;
	/// <summary>
	/// this is where i put all the functions implementing TASE rules logic
	/// </summary>
	public class TASEUtils
	{
		#region Maof Specs
		//////////////////////////////////////////////////////////////////////////////
		//              Maof Options Specialities

		/// <summary>
		/// This one addresses the fact that Maof options are priced in constatnt increments.
		/// </summary>
		/// <param name="premium"><see cref="System.Double"/> the original premium</param>
		/// <returns><see cref="System.Int32"/> whole number of NIS</returns>
		public static int RoundPremia(double premium)
		{
			if (premium >= 2000.0) return ((int)Math.Round(premium / 20.0)) * 20;
			else if (premium >= 200.0) return ((int)Math.Round(premium / 10.0)) * 10;
			else if (premium >= 20.0) return ((int)Math.Round(premium / 5.0)) * 5;
			else return (int)Math.Round(premium);
		}

		/// <summary>
		/// Computes the nearest increment (up-tick) resulting from
		/// Maof options pricing rules.
		/// </summary>
		/// <param name="premium"><see cref="System.Double"/> the original premium</param>
		/// <returns><see cref="System.Int32"/> whole number of NIS</returns>
		public static double PremiaIncrement(double premium)
		{
			if (RoundPremia(premium) >= 2000.0) return 20.0;
			else if (RoundPremia(premium) >= 200.0) return 10.0;
			else if (RoundPremia(premium) >= 20.0) return 5.0;
			else return 1.0;
		}

		/// <summary>
		/// Computes the nearest decrement (down-tick) resulting from
		/// Maof options pricing rules.
		/// </summary>
		/// <param name="premium"><see cref="System.Double"/> the original premium</param>
		/// <returns><see cref="System.Int32"/> whole number of NIS</returns>
		public static double PremiaDecrement(double premium)
		{
			if (RoundPremia(premium) >= 2020.0) return 20.0;
			else if (RoundPremia(premium) >= 210.0) return 10.0;
			else if (RoundPremia(premium) >= 25.0) return 5.0;
			else return 1.0;
		}

		#endregion;

		public static double TickSize(double price, bool direction)
		{
			double t;
			if (price == 1000.0 || price == 10000.0 || price == 100000.0)
			{

				if (direction == true)
				{
					// True = up tick
					switch ((int)price)
					{
						case (1000):
							t = 1;
							break;
						case (10000):
							t = 10;
							break;
						default:
							t = 100;
							break;
					}
				}
				else
				{
					// False = down tick
					switch ((int)price)
					{
						case (1000):
							t = 0.1;
							break;
						case (10000):
							t = 1;
							break;
						default:
							t = 10;
							break;
					}
				}

			}
			else
			{
				if (price < 1000) t = 0.1;
				else if (price < 10000) t = 1;
				else if (price < 100000) t = 10;
				else t = 100;
			}
			return (t);
		}

		/// <summary>
		/// Minimal lot size on TASE, in units. For TA25 stocks it stands on 5000 NIS,
		/// for TA75 it's 2000 NIS. Computed as a minimal number of shares which value is equal 
		/// or more than minimal lot size in NIS allowed by TASE.
		/// </summary>
		/// <param name="price">stock's price for which I want to compute the lot size</param>
		/// <param name="nis">Minimal lot in NIS, according to the TASE</param>
		/// <returns># of shares consiting the minimal lot</returns>
		public static int MinLotSize(double price, double nis)
		{
			double q = nis * 100 / price;       // non-rounded # of shares in the minimal lot
			if (q <= 100) q = Math.Ceiling(q);                      //round it up, according to the TASE rules - 
			else if (q <= 1000) q = Math.Ceiling(q / 10) * 10;      //the minimal increment in the traded quantity 
			else if (q <= 10000) q = Math.Ceiling(q / 100) * 100;   //is subject to the total amount of shares
			else if (q <= 100000) q = Math.Ceiling(q / 1000) * 1000;//to be traded
			else q = Math.Ceiling(q / 10000) * 10000;

			return (int)q;
		}






	}//class TASEUtils
	#endregion;

	#region Cyclic Buffers
	/// <summary>
	/// Cyclic buffers of integers
	/// Can be used to calculate average over last X minutes
	/// </summary>
	public class IntStatistics : JQuant.CyclicBuffer<int>
	{
		public IntStatistics(string name, int size)
			: base(size)
		{
			summ = 0;
			Name = name;
		}

		public new void Add(int val)
		{
			if (Count < Size)
			{
				Count++;
			}
			else
			{
				// moving summ
				summ -= (buffer[head]);

			}
			summ += val;
			buffer[head] = val;

			head = IncIndex(head, Size);

		}

		protected new int Remove()
		{
			int o = base.Remove();
			return o;
		}

		public double Mean
		{
			get
			{
				double mean = 0;

				// moving average
				if (Count != 0)
				{
					mean = summ / Count;
				}
				return mean;
			}
			protected set
			{
			}
		}

		public int Summ
		{
			get
			{
				return summ;
			}
			protected set
			{
			}
		}

		public string Name
		{
			get;
			protected set;
		}

		int summ;
	}

	/// <summary>
	/// Cyclic buffers of integers
	/// Can be used to calculate maximum and minimum value over last X
	/// entries
	/// </summary>
	public class IntMaxMin : JQuant.CyclicBuffer<int>
	{
		public IntMaxMin(string name, int size)
			: base(size)
		{
			sortedList = new System.Collections.SortedList(size);
			Name = name;
		}

		public new void Add(int val)
		{
			if (Count < Size)
			{
				Count++;
			}
			if (sortedList.Count >= Size)
			{
				// sorted list is full - remove one entry
				sortedList.Remove(buffer[head]);
			}

			buffer[head] = val;

			// add only new value
			if (!sortedList.ContainsKey(val))
			{
				sortedList.Add(val, val);
			}

			head = IncIndex(head, Size);
		}

		protected new int Remove()
		{
			int o = base.Remove();
			return o;
		}

		public int Max
		{
			get
			{
				int val = 0;
				if (sortedList.Count > 0)
				{
					val = (int)sortedList.GetByIndex(sortedList.Count - 1);
				}
				return val;
			}

			protected set
			{
			}
		}

		public int Min
		{
			get
			{
				int val = 0;
				if (sortedList.Count > 0)
				{
					val = (int)sortedList.GetByIndex(0);
				}
				return val;
			}

			protected set
			{
			}
		}

		public string Name
		{
			get;
			protected set;
		}

		System.Collections.SortedList sortedList;
	}
	#endregion
}//namespace JQuant
