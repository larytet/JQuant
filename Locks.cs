
using System;

namespace JQuant
{
	public interface ICriticalSection
	{
		void Enter();
		void Exit();
	}

	/// <summary>
	/// do nothing to protect a critical section
	/// hopefully CLI will optimize out empty non-virtual methods
	/// </summary>
	public class DummyCriticalSection : ICriticalSection
	{
		public void Enter()
		{
		}

		public void Exit()
		{
		}
	}

	/// <summary>
	/// counts entries and exits. This class will not be used directly
	/// Use child classes instead
	/// </summary>
	public class CriticalSection : ICriticalSection
	{

		protected CriticalSection(string name)
		{
			Name = name;
			count = 0;
		}

		public void Enter()
		{
			if (count > 0)
			{
				Console.WriteLine("Lock " + Name + " is taken more than once count=" + count);
				count = 0;
			}
			count++;
		}

		public void Exit()
		{
			if (count < 1)
			{
				Console.WriteLine("Lock " + Name + " is freed more than once count=" + count);
				count = 1;
			}
			count--;
		}

		public string Name
		{
			get;
			protected set;
		}


		protected int count;
	}


	/// <summary>
	/// use System.Threading.Monitor API (similar to lock)
	/// </summary>
	public class LockCriticalSection : CriticalSection
	{
		public LockCriticalSection(string name, object lockObject)
			: base(name)
		{
			this.lockObject = lockObject;
		}

		public new void Enter()
		{
			System.Threading.Monitor.Enter(lockObject);
			base.Enter();
		}

		public new void Exit()
		{
			base.Exit();
			System.Threading.Monitor.Exit(lockObject);
		}

		protected object lockObject;
	}

	/// <summary>
	/// use System.Threading.Monitor API (similar to lock)
	/// The object itself is a synchronization object
	/// </summary>
	public class SyncObject : CriticalSection
	{
		public SyncObject(string name)
			: base(name)
		{
		}


		public new void Enter()
		{
			System.Threading.Monitor.Enter(this);
			base.Enter();
		}

		public new void Exit()
		{
			base.Exit();
			System.Threading.Monitor.Exit(this);
		}
	}

}//namespace JQuant
