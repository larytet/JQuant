
using System;
using System.Threading;

namespace JQuant
{

	/// <summary>
	/// Thread waiting forever for a message
	/// </summary>
	public class MailboxThread<Message> : IThread, IDisposable
	{

		public MailboxThread(string name, int mailboxCapacity)
		{
			Init(name, mailboxCapacity, TIMEOUT_INFINITE);
		}

		public MailboxThread(string name, int mailboxCapacity, int timeout)
		{
			Init(name, mailboxCapacity, timeout);
		}

		protected void Init(string name, int mailboxCapacity, int timeout)
		{
			_mailbox = new Mailbox<Message>(name, mailboxCapacity);
			_isAlive = false;
			_name = name;

			// add myself to the list of created mailboxes
			Resources.Threads.Add(this);

			_state = ThreadState.Initialized;
			longestJobTime = 0;
			this.Timeout = timeout;
		}

		public virtual void Dispose()
		{
			if (_isAlive)
			{
				Console.WriteLine("MailboxThread " + Name + " disposed but not stopped");
			}

			_mailbox.Dispose();
			// _mailbox = null;

			_state = ThreadState.Destroyed;

			// remove myself from the list of created mailboxes
			Resources.Threads.Remove(this);

			_thread.Abort();
			_thread = null;
		}

		~MailboxThread()
		{
			Console.WriteLine("MailboxThread " + _name + " destroyed");
		}

		/// <summary>
		/// loop forever (or until _isAlive is true)
		/// get messages from the mailbox, call HandleMessage
		/// </summary>
		protected virtual void Run()
		{
			_state = ThreadState.Started;
			_isAlive = true;
			while (_isAlive)
			{
				Message msg;
				bool result = _mailbox.Receive(out msg);
				if (result)
				{
					long tick = DateTime.Now.Ticks;
					HandleMessage(msg);
					tick = DateTime.Now.Ticks - tick;
					longestJobTime = Math.Max(longestJobTime, tick);
				}
			}

			_state = ThreadState.Stoped;
			Dispose();
		}

		/// <summary>
		/// loop forever (or until _isAlive is true)
		/// get messages from the mailbox, call HandleMessage
		/// </summary>
		protected virtual void RunTimeout()
		{
			_state = ThreadState.Started;
			_isAlive = true;
			while (_isAlive)
			{
				Message msg;
				bool result = _mailbox.Receive(out msg, Timeout);

				if (!_isAlive) break;

				long tick = DateTime.Now.Ticks;
				HandleMessage(msg);
				tick = DateTime.Now.Ticks - tick;
				longestJobTime = Math.Max(longestJobTime, tick);

			}

			_state = ThreadState.Stoped;
			Dispose();
		}

		/// <summary>
		/// application will override this method
		/// </summary>
		/// <param name="message">
		/// A <see cref="Message"/>
		/// </param>
		protected virtual void HandleMessage(Message message)
		{
			Console.WriteLine("I can't handle message " + message);
		}

		public bool Send(Message message)
		{
			bool result = _mailbox.Send(message);
			return result;
		}

		public System.Threading.ThreadPriority Priority
		{
			get
			{
				return threadPriority;
			}

			set
			{
				threadPriority = value;
				if (_thread != null)
				{
					_thread.Priority = value;
				}
			}
		}

		public void Stop()
		{
			_isAlive = false;
			_mailbox.Pulse();
		}

		/// <summary>
		/// call this method to start the thread - enters loop forever in the 
		/// method Run()
		/// </summary>
		public void Start()
		{
			_isAlive = true;
			if (Timeout == TIMEOUT_INFINITE)
			{
				_thread = new Thread(this.Run);
			}
			else
			{
				_thread = new Thread(this.RunTimeout);
			}
			_thread.Priority = Priority;
			_thread.Start();
		}

		public ThreadState GetState()
		{
			return _state;
		}

		/// <summary>
		/// returns maximum time call to HandleMessage() took (microseconds)
		/// </summary>
		public long GetLongestJob()
		{
			return longestJobTime / 10;
		}

		public void WaitForTermination()
		{
			_thread.Join();
		}


		public string Name
		{
			get;
			set;
		}

		protected Mailbox<Message> _mailbox;
		protected bool _isAlive;
		protected ThreadState _state;
		protected Thread _thread;
		protected string _name;
		protected long longestJobTime;
		protected ThreadPriority threadPriority;

		protected static int TIMEOUT_INFINITE = -1;
		protected int Timeout
		{
			get;
			set;
		}
	}
}//namespace JQuant
