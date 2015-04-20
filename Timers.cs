
using System;
using System.Threading;
using System.Timers;
using System.Collections.Generic;
using System.ComponentModel;

/// <summary>
/// The idea is taken from http://larytet.sourceforge.net/aos.shtml  (AOS Timer)
/// In the CSharp there is System.Threading.Timer. The service comes at cost - a separate thread
/// for every timer. There are more problems assotiated with the general timer system.
/// In some (most) of the applications number of simultaneously running timers can be significant,
/// but there are only limited (and small) number of different timeouts (different types of timers). 
/// Some of the timers are short and require precision (handle high priority events) and some
/// timers are long and do not require high priority threads for handling.
/// In the typical modern timer system there is an array where each entry is a linked list of
/// timers which belong (approximately) to the same group (have similar or same) timeout. Thus start
/// timer in a typical case takes O(1) - adding a new timer to the list. Number of lists is about
/// equal to the number of different types of timers in the system.
/// Such system has difficiencies
///   * No control over prirority of the thread which handles the time expiration
///   * The same pool of timers is shared by all applications
///   * If number of different timers in the system is significant performance of 
///     the StartTimer will degrade
///   * There is no interface which allows to handle timer expiration synchronously -
///     in the context of the same thread which start the timers
/// 
/// This is proprietary implemenation for the application timers
///   Terminology:
///   Timer list - queue of the running timers with the SAME timeout. For example list of 1s timers
///   Set - one or more lists of timers and a task handling the lists.
///       For example set A containing 1s timers, 2s timers and 5s timers
///       and set B containing 100 ms timers, 50 ms timers and 200 ms timers
///   Timer task - task that handles one and only one set of lists of timers
/// 
///                      -----------   Design   ---------------
/// 
///   In the system run one or more timer tasks handling different timer sets. Every timer
///   set contains one or more timer lists.
///   Function start timer allocates free entry from the stack of free timers and places
///   the timer in the end of the timer list (FIFO). Time ~O(1)
///   Timer task waits for the expiration of the nearest timer, calls application handler
///   TimerExpired, find the next timer to be expired using sequential search in the
///   set (always the first entry in a timer list). Time ~ O(size of set)
///   Function stop timer marks a running timer as stopped. Time ~O(1)
/// 
///                      -----------   Reasons  ---------------
/// 
///   1. It is possible that every subsystem will have own timer tasks running in
///      different priorities
///   2. Set of long timers and set of short timers can be created and handled by tasks with
///      different priorities
///   3. "Timer expired"  application handlers can be called from different tasks. For high
///      priority short timers such handler should be short - release semaphore for example,
///      for low priority long timers handler can make long processing like audit in data-base
///   4. In the system can coexist 1 or 2 short timers - 50 ms - used in call process
///      and 10 long timers  - 10 s, 1 min, 1 h, etc. - used in the application
///      sanity checking or management
///   5. In the system can coexist short - 10 ms - timer that always expired and 10 long
///      protocol timers that ususally stopped by the application before expiration
/// 
///                      -----------   Usage examples  ---------------
///
///   Timers.Init();  // initialize the subsystem, call once
/// 
///   // create a set
///   TimerTask timerTask = new TimerTask("ShortTimers");
/// 
///   // create two types of timers - 5s timer and 30s timer
///   TimerList timers_5sec = new TimerList("5sec", 5*1000, 100, this.TimerExpiredHandler, timerTask);
///   TimerList timers_30sec = new TimerList("30sec", 30*1000, 100, this.TimerExpiredHandler, timerTask);
/// 
///   // start the set of timers
///   timerTask.Start();
/// 
///   // start a couple of timers - i am no planning to stop these timers
///   // timerTask will call method TimerExpiredHandler() when the timers expire
///   timers_5sec.Start();
///   timers_30sec.Start();
/// 
///   How to stop previously started timer? This is tricky. Application have to keep two values - 
///   reference to the object of type ITimer and timer identifier returned by Start(). To stop 
///   the running timer application will need both of them. In the typical case state machine
///   in the application will keep the values for all started timers as part of the internal 
///   "state"
///
/// </summary>
namespace JQuant
{

	/// <summary>
	/// application calls Timers.Init() before any other operation
	/// </summary>
	public class Timers
	{
		/// <summary>
		/// this methos should be called once by the application 
		/// before any of the API will be used
		/// </summary>
		public static void Init()
		{
			TimerList.InitTimerId();
		}

		public enum Error
		{
			[Description("None")]
			NONE,

			[Description("Already stoped")]
			ALREADY_STOPED,

			[Description("Wrong timer ID")]
			WRONG_TIMER_ID,


			[Description("No free timers")]
			NO_FREE_TIMERS,

			[Description("Unknown in Stop")]
			STOP_UNKNOWN,

			[Description("Unknown in Start")]
			START_UNKNOWN
		};


	}


	/// <summary>
	/// This class used for all interactions between application
	/// and TimerList. For example, TimerList.Start() returns objects of this
	/// type and TimerList.Stop() requires reference to the object of this
	/// type
	/// 
	/// Internally TimerList uses objects of type Timer
	/// </summary>
	public interface ITimer
	{
		/// <summary>
		/// application is free to set ApplicationHook field. TimerList
		/// will not modify the field.
		/// This field can be used to store reference to the object which keeps
		/// state machine internal state.
		/// </summary>
		object ApplicationHookGet();
		void ApplicationHookSet(object hook);
	}


	public delegate void TimerExpiredCallback(ITimer timer);

	/// <summary>
	/// lits of timers - keep all timers with the same timeout
	/// </summary>
	public class TimerList : IResourceTimerList, IDisposable
	{
		/// <summary>
		/// Create a timer list
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/>
		/// Name of the timer list
		/// </param>
		/// <param name="size">
		/// A <see cref="System.Int32"/>
		/// Maximum number of pending timers
		/// </param>
		/// <param name="timeout">
		/// A <see cref="System.Int32"/>
		/// Timeout for the timers in this list (milliseconds)
		/// </param>
		/// <param name="timerCallback">
		/// A <see cref="TimerExpiredCallback"/>
		/// This method will be called for all expired timers
		/// There is no a callback per timer. Only a callback per timer list
		/// </param>
		public TimerList(string name, int timeout, int size, TimerExpiredCallback timerCallback, TimerTask timerTask)
		{

			Name = name;
			this.timerCallback = timerCallback;
			// this.baseTick = DateTime.Now.Ticks;
			this.timerTask = timerTask;
			this.Timeout = timeout;

			// clear the counters
			countStart = 0;
			countStartAttempt = 0;
			countStop = 0;
			countStopAttempt = 0;
			countExpired = 0;
			countMax = 0;

			// create pool of free timers
			InitTimers(size);

			// register with the TimerTask
			timerTask.AddList(this);

			// add myself to the list of created timer lists
			Resources.TimerLists.Add(this);
		}

		public void Dispose()
		{
			// rmeove myself from the TimerTask
			timerTask.RemoveList(this);

			// remove myself from the list of created timer lists
			Resources.TimerLists.Remove(this);
		}

		/// <summary>
		/// Allocate a free timer from the stack of free timers, set expiration
		/// add the timer to end of the list of running timers
		/// </summary>
		/// <param name="timeout">
		/// A <see cref="System.Int64"/>
		/// </param>
		/// <param name="timer">
		/// A <see cref="Timer"/>
		/// Reference to the started timer. Can be used in call to Stop()
		/// </param>
		/// <param name="timerId">
		/// A <see cref="System.Int64"/>
		/// Timer list reuse refernces (objects) of type Timer. Reference to Timer is
		/// not enough to make sure that you stop the correct timer. Value timerId
		/// is a unique (system level) timer identifier
		/// </param>
		/// <param name="applicationHook">
		/// A <see cref="System.Object"/>
		/// Timer.applicationHook field will be set to this value
		/// Field applicationHook can help to identify the timer in the context of timerExpired
		/// callback
		/// </param>
		/// <param name="autoRestart">
		/// A <see cref="System.Boolean"/>
		/// If true the system will reschedule the expire timer automatically while taking care 
		/// that the next expiration tick is exactly startTick+2*timeout
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// returns true of Ok, false is failed to allocate a new timer - no free
		/// timers are available
		/// </returns>
		public bool Start(out ITimer iTimer, out long timerId, object applicationHook, bool autoRestart)
		{

			// timestamp the call as soon as possible
			DateTime startTime = DateTime.Now;
			long startTick = (startTime.Ticks) / TICKS_IN_MILLISECOND;


			Timer timer = null;
			iTimer = null;
			timerId = 0;
			Timers.Error error = Timers.Error.START_UNKNOWN;


			do
			{
				// get new timer Id
				timerId = TimerId.GetNext();

				// allocate a timer from the stack of free timers
				lock (this)
				{
					countStartAttempt++;

					if (freeTimers.Count > 0)
					{
						timer = freeTimers.Pop();
					}
					else
					{
						error = Timers.Error.NO_FREE_TIMERS;
						break;
					}
				}

				// initialize the timer
				timer.ApplicationHook = applicationHook;
				timer.StartTick = startTick;
				timer.ExpirationTime = startTick + Timeout;
				timer.Running = true;
				timer.TimerId = timerId;
				timer.AutoRestart = autoRestart;
				timer.Restarts = 1; // first start is considered a restart
				iTimer = timer;

				// add the timer to the queue of the pending timers
				lock (this)
				{
					countStart++;
					pendingTimers.Add(timer);
					countMax = Math.Max(countMax, pendingTimers.Count);
				}

				// send wakeup call to the task handling the timers
				timerTask.WakeupCall();

				error = Timers.Error.NONE;
			}
			while (false);

			if (error != Timers.Error.NONE)
			{
				PrintError("Start failed ", error);
			}


			return (error == Timers.Error.NONE);
		}


		/// <summary>
		/// use this method if no need to call Stop() will ever arise for the timer
		/// </summary>
		public bool Start(bool autorestart)
		{
			ITimer timer;
			long timerId;

			bool result = Start(out timer, out timerId, null, autorestart);

			return result;
		}

		/// <summary>
		/// use this method if no need to call Stop() will ever arise for the timer
		/// </summary>
		public bool Start(object applicationHook)
		{
			ITimer timer;
			long timerId;

			bool result = Start(out timer, out timerId, applicationHook, false);

			return result;
		}

		/// <summary>
		/// use this method if there is no need to call Stop()
		/// TimerList will call callback when the timer expires. 
		/// Different timers can not be distinguished
		/// </summary>
		public bool Start()
		{
			ITimer timer;
			long timerId;

			bool result = Start(out timer, out timerId, null, false);

			return result;
		}

		/// <summary>
		/// stop previously started timer
		/// </summary>
		/// <param name="timer">
		/// A <see cref="Timer"/>
		/// </param>
		/// <param name="timerId">
		/// A <see cref="System.Int64"/>
		/// Value returned by StartTimer()
		/// Timer list reuse refernces (objects) of type Timer. Reference to Timer object 
		/// is not enough to make sure that you stop the correct timer. Value timerId
		/// is a unique (system level) timer identifier
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// Returns true if the timer was running and now stopped
		/// Call to this method for already stoped timer will return false
		/// and error message will be printed on the console
		/// </returns>
		public bool Stop(ITimer iTimer, long timerId)
		{
			Timers.Error error = Timers.Error.NONE;

			Timer timer = (Timer)iTimer;

			lock (this)
			{
				countStopAttempt++;

				if ((timer.Running) && (timer.TimerId == timerId))
				{
					countStop++;
					timer.Running = false;
				}
				else if (!timer.Running)
				{
					error = Timers.Error.ALREADY_STOPED;
				}
				else if (timer.TimerId != timerId)
				{
					error = Timers.Error.STOP_UNKNOWN;
				}
			}

			if (error != Timers.Error.NONE)
			{
				PrintError("Stop failed ", error);
			}

			return (error == Timers.Error.NONE);
		}

		/// <summary>
		/// returns the timeout before the nearest timer expires
		/// </summary>
		/// <returns>
		/// A <see cref="System.Int64"/>
		/// Returns true if there is a timer in the pending list
		/// </returns>
		public bool GetDelay(long currentTick, out long delay)
		{
			bool result = false;

			lock (this)
			{
				if (pendingTimers.Count > 0)
				{
					Timer timer = pendingTimers[0];
					delay = timer.ExpirationTime - currentTick;
					result = true;
				}
				else
				{
					delay = long.MaxValue;
				}
			}

			return result;
		}

		/// <summary>
		/// Check timers starting from the ooldest. If expired call the user callback,
		/// remove the timer from the queue of pending timers, release the timer back
		/// to the stack of free timers.
		/// 
		/// If the timer should be rescheduled (autorestart) I keep the start tick
		/// (tick when the timer was started very first time) and counter of
		/// restarts.  
		/// The timer is removed from the head of the queue and added to the tail.
		/// Timer expiration time is set to startTick + restarts * timeout. This way
		/// I solve problem of timeout "drift" for the periodic timers under normal
		/// conditions. The correct approach is to insert the restarted timer at the 
		/// correct position, which is close to the tail, but not neccessary the
		/// last element of the queue
		/// </summary>
		/// <param name="currentTick">
		/// A <see cref="System.Int64"/>
		/// </param>
		/// <param name="delay">
		/// A <see cref="System.Int32"/>
		/// </param>
		public void ProcessExpiredTimers(long currentTick, out int delay)
		{
			Timer timer = default(Timer);
			delay = Int32.MaxValue;

			while (true)
			{
				lock (this)
				{
					// any pending timers in the list ?
					if (pendingTimers.Count > 0)
					{
						// get first (oldest) timer from the list head
						timer = pendingTimers[0];
					}
					else
					{
						// list is empty
						break;
					}
				}

				// no more expired timers
				if (timer.ExpirationTime > currentTick)
				{
					delay = (int)(timer.ExpirationTime - currentTick);
					break;
				}

				if ((timer.ExpirationTime <= currentTick) && (timer.Running))
				{
					lock (this)
					{
						countExpired++;
						if (!timer.AutoRestart)
						{
							timer.Running = false;
						}
					}

					timerCallback(timer);
				}

				// return all not running timers (expired and stoped)
				// to the pool of free timers
				if ((!timer.Running) && (!timer.AutoRestart))
				{
					lock (this)
					{
						pendingTimers.Remove(timer);
						freeTimers.Push(timer);
					}
				}

				// timer requires rescheduling
				if (timer.AutoRestart)
				{
					lock (this)
					{
						int timerRestarts = (timer.Restarts++);
						timer.ExpirationTime = timer.StartTick + timerRestarts * Timeout;

						// remove expired timer from the head, add to the tail
						pendingTimers.Remove(timer);
						pendingTimers.Add(timer);
					}
				}
			}

		}

		protected void PrintError(string prefix, Timers.Error error)
		{
			System.Console.WriteLine(prefix + EnumUtils.GetDescription(error));
		}

		/// <summary>
		/// free timers are stored in the stack
		/// </summary>
		/// <param name="size">
		/// A <see cref="System.Int32"/>
		/// </param>
		protected void InitTimers(int size)
		{
			freeTimers = new Stack<Timer>(size);
			for (int i = 0; i < size; i++)
			{
				Timer timer = new Timer();
				freeTimers.Push(timer);
			}

			// list of pending timers - initially empty
			pendingTimers = new List<Timer>(size);
		}

		~TimerList()
		{
			Console.WriteLine("TimerList " + Name + " from set " + timerTask.Name + " destroyed");
		}

		protected int GetSize()
		{
			int size;

			lock (this)
			{
				size = (pendingTimers.Count + freeTimers.Count);
			}

			return size;
		}

		public string GetTaskName()
		{
			return timerTask.Name;
		}

		public void GetEventCounters(out System.Collections.ArrayList names, out System.Collections.ArrayList values)
		{
			names = new System.Collections.ArrayList(12);
			values = new System.Collections.ArrayList(12);


			names.Add("Size"); values.Add(GetSize());
			names.Add("Start"); values.Add(countStart);
			names.Add("Expired"); values.Add(countExpired);
			names.Add("Stop"); values.Add(countStop);
			names.Add("PendingTimers"); values.Add(pendingTimers.Count);
			names.Add("StartAttempt"); values.Add(countStartAttempt);
			names.Add("StopAttempt"); values.Add(countStopAttempt);
		}


		/// <summary>
		/// stack of free timers
		/// </summary>
		protected Stack<Timer> freeTimers;

		int countStart;
		int countStartAttempt;
		int countStop;
		int countStopAttempt;
		int countExpired;
		int countMax;

		/// <summary>
		/// List of pending timers. TimerList contains timers with the same timeout
		/// The oldest is going to be in the tail of the list
		/// </summary>
		protected List<Timer> pendingTimers;

		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// this method will be called for every expired timer
		/// </summary>
		protected TimerExpiredCallback timerCallback;

		/// <summary>
		/// to save tick wrap arounds all ticks are going to be calculated as 
		/// offsets to this value
		/// </summary>
		// protected long baseTick;

		public const int TICKS_IN_MILLISECOND = 10000;

		protected TimerTask timerTask;

		public int Timeout
		{
			get;
			protected set;
		}


		/// <summary>
		/// Data holder - keeps a timer
		/// </summary>
		protected class Timer : ITimer
		{
			public object ApplicationHookGet()
			{
				return ApplicationHook;
			}

			public void ApplicationHookSet(object hook)
			{
				ApplicationHook = hook;
			}


			// private section - application is not expected to use the fields
			// below this line

			/// <summary>
			/// this is expiration tick (milliseconds)
			/// </summary>
			public long ExpirationTime;

			/// <summary>
			/// system tick when the timer was started  (milliseconds)
			/// </summary>
			public long StartTick;

			/// <summary>
			/// true if not stopped
			/// </summary>
			public bool Running;

			public long TimerId;

			public bool AutoRestart;

			public int Restarts;

			public object ApplicationHook
			{
				get;
				set;
			}
		}

		public static void InitTimerId()
		{
			TimerId.Init();
		}

		/// <summary>
		/// generates a unique timer identifier - simple 64 bits counter
		/// </summary>
		protected class TimerId
		{
			protected TimerId(long initialValue)
			{
				timerId = initialValue;
			}

			public static void Init()
			{
				if (instance == default(TimerId))
				{
					instance = new TimerId(0xB5);
				}
			}

			/// <summary>
			/// Locks access to the timerId
			/// should not be called from inside locked section
			/// </summary>
			/// <returns>
			/// A <see cref="System.Int64"/>
			/// Unique on the system level timer identifier
			/// </returns>
			static public long GetNext()
			{
				long id;

				lock (instance)
				{
					timerId++;
					id = timerId;
				}

				return id;
			}

			static TimerId instance;
			static long timerId;
		}
	}

	/// <summary>
	/// Timer task (timer set) implementation
	/// </summary>
	public class TimerTask : IDisposable
	{

		public TimerTask(string name)
		{
			this.Name = name;
			this.isAlive = false;
			this.timerLists = new List<TimerList>(5);
			sleepTimeout = Int32.MaxValue;
			semaphore = new Semaphore(0, Int32.MaxValue);
		}

		public void Dispose()
		{
			thread.Abort();
			thread = null;
			semaphore.Close();
		}


		/// <summary>
		/// call this method to start the thread - enters loop forever in the 
		/// method Run()
		/// </summary>
		public void Start()
		{
			this.thread = new Thread(this.Run);
			this.thread.Start();
		}

		public void Stop()
		{
			isAlive = false;
			semaphore.Release();
		}

		public void WakeupCall()
		{
			semaphore.Release();
		}

		public void AddList(TimerList timerList)
		{
			lock (this)
			{
				timerLists.Add(timerList);
			}
		}

		public void RemoveList(TimerList timerList)
		{
			lock (this)
			{
				timerLists.Remove(timerList);
			}
		}

		~TimerTask()
		{
			Console.WriteLine("TimerTask " + Name + " destroyed");
		}


		protected void Run()
		{
			isAlive = true;

			// Check all timer lists, find minimum delay (timeout before the nearest timer expires)
			// call Thread.Sleep()
			while (isAlive)
			{
				semaphore.WaitOne(sleepTimeout, true);
				ProcessExpiredTimers();
			}
		}


		/// <summary>
		/// process all lists for expired timers
		/// calculate next sleep delay
		/// </summary>
		protected void ProcessExpiredTimers()
		{
			int delay = Int32.MaxValue;

			// i want to save CPU cycles. calculate current tick only once
			long currentTick = DateTime.Now.Ticks / TimerList.TICKS_IN_MILLISECOND;

			int idx = 0;
			TimerList timerList = default(TimerList);

			while (true)
			{
				lock (this)
				{
					if (idx < timerLists.Count)
					{
						timerList = timerLists[idx];
						idx++;
					}
					else
					{
						break;
					}
				}

				int temp;
				timerList.ProcessExpiredTimers(currentTick, out temp);
				if (temp < delay)
				{
					delay = temp;
				}
			}

			sleepTimeout = delay;
		}

		protected Thread thread;
		protected bool isAlive;
		public string Name
		{
			get;
			protected set;
		}
		protected int sleepTimeout;

		List<TimerList> timerLists;

		protected Semaphore semaphore;

	}

}//namespace JQuant
