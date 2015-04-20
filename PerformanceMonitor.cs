
using System;

namespace JQuant
{
	namespace PM
	{

		/// <summary>
		/// interace of all performance monitors in the system
		/// </summary>
		interface Interface<Event>
		{
			void Tick();
			void Notify(Event e);
		}

		/// <summary>
		/// basic PM. Keeps a cyclic buffer of errors. Every time Notify is called an entry in the
		/// buffer increase by one (if event is Nok)
		/// When Tick() is called PM moves to the next entry in the cyclic buffer.
		/// At any given moment cyclic buffer contains "size" entries where every entry is number of NOKs
		/// in a given tick.
		/// Tick can be one second or one hour.
		/// For example, 24 hours statistics can look like cyclic buffer containing 24 counters - a counter
		/// of errors for every hour.
		/// </summary>
		public class Errors : JQuant.CyclicBuffer<int>,
							Interface<Errors.Event>,
							System.Collections.Generic.IEnumerable<int>
		{
			public enum Event
			{
				OK,
				NOK
			}

			public Errors(string name, int size)
				: base(size)
			{
			}

			/// <summary>
			/// move to the next entry in the cyclic buffer
			/// </summary>
			public void Tick()
			{
				lock (this)
				{
					head = IncIndex(head, Size);
					buffer[head] = 0;

					if (Count < Size)
					{
						Count++;
					}
				}
			}

			/// <summary>
			/// update current entry (at the head) in the cyclic buffer
			/// count NOK events in a  single tick
			/// </summary>
			public void Notify(Event e)
			{
				if (e == Event.NOK)
				{
					lock (this)
					{
						int val = (int)buffer[head];
						val++;
						buffer[head] = val;
					}
				}
			}

			protected new void Add(int o)
			{
			}

			protected new int Remove()
			{
				return 0;
			}

		}



	} // namespace PM
}//namespace JQuant
