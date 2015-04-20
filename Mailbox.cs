
using System;
using System.Threading;
using System.Collections.Generic;

namespace JQuant
{

	/// <summary>
	/// this guy is a simple Queue base implemenation of mailbox - queue of messages with
	/// send and receive methods
	/// </summary>
	public class Mailbox<Message> : Queue<Message>, IResourceMailbox, IDisposable
	{
		public Mailbox(string name, int capacity)
			: base(capacity)
		{
			Name = name;
			_capacity = capacity;
			_maxCount = 0;

			_dropped = 0;
			_sent = 0;
			_received = 0;
			_timeouts = 0;

			semaphore = new Semaphore(0, Int32.MaxValue);

			// add myself to the list of created mailboxes
			Resources.Mailboxes.Add(this);
		}

		public void Dispose()
		{
			Clear();
			semaphore.Close();

			// remove myself from the list of created mailboxes
			Resources.Mailboxes.Remove(this);
		}

		~Mailbox()
		{
			Console.WriteLine("Mailbox " + Name + " destroyed");
		}

		/// <summary>
		/// add a message to the queue
		/// </summary>
		/// <param name="message">
		/// A <see cref="Message"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// returns True is there is enough place in the message queue
		/// </returns>
		public bool Send(Message message)
		{
			bool result = false;

			lock (this)
			{
				if (Count < _capacity)
				{
					Enqueue(message);
					result = true;
					_sent++;
				}
				else
				{
					Monitor.Pulse(this);
					_dropped++;
				}
				_maxCount = Math.Max(_maxCount, Count);
			}
			semaphore.Release();

			return result;
		}

		/// <summary>
		/// Send pulse and unblock the thread waiting for the message
		/// 
		/// </summary>
		public void Pulse()
		{
			semaphore.Release();
		}

		/// <summary>
		/// blocking call - wait for a message 
		/// </summary>
		/// <param name="message">
		/// A <see cref="Message"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// True - if a new message arrived
		/// </returns>
		public bool Receive(out Message message)
		{
			bool result = false;
			message = default(Message);

			result = semaphore.WaitOne();


			lock (this)
			{
				if (Count != 0)
				{
					_received++;
					message = Dequeue();
					result = true;
				}
				else
				{
					result = false;
				}
			}


			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message">
		/// A <see cref="Message"/>
		/// </param>
		/// <param name="timeout">
		/// A <see cref="System.Int32"/>
		/// Block the calling thread until a new message or timeout expires 
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// True - if a new message arrived
		/// </returns>
		public bool Receive(out Message message, int timeout)
		{
			bool result = false;
			message = default(Message);

			result = semaphore.WaitOne(timeout, true);

			lock (this)
			{
				if ((result) && (Count > 0))
				{
					_received++;
					message = Dequeue();
				}
				else
				{
					_timeouts++;
					result = false;
				}
			}

			return result;
		}

		public string Name
		{
			get;
			set;
		}

		public void GetEventCounters(out System.Collections.ArrayList names, out System.Collections.ArrayList values)
		{
			names = new System.Collections.ArrayList(8);
			values = new System.Collections.ArrayList(8);

			names.Add("Size"); values.Add(_capacity);
			names.Add("MaxCount"); values.Add(_maxCount);
			names.Add("Pending"); values.Add(Count);
			names.Add("Timeout"); values.Add(_timeouts);
			names.Add("Received"); values.Add(_received);
			names.Add("Sent"); values.Add(_sent);
			names.Add("Dropped"); values.Add(_dropped);
		}


		protected int _capacity;
		protected int _maxCount;
		protected int _dropped;
		protected int _sent;
		protected int _received;
		protected int _timeouts;

		protected Semaphore semaphore;

	}
}//namespace JQuant
