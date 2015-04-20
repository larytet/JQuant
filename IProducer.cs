
using System;

namespace JQuant
{
	/// <summary>
	/// consumer is a data consumer. the interface assumes asynchronous processing
	/// usually Notify() will send add the data object to the internal queue for 
	/// the future processing and get out
	/// Consumer registers itslef in the producer. Producer calls Notify()
	/// Different approach would be to use delegated method. I prefer interface - in 
	/// the future may be I will need more methods like real-time priority notification
	/// </summary>
	public interface IConsumer<DataType>
	{
		/// <summary>
		/// Producer calls the method to notfy consumer that there is new data available
		/// Notify() should be non-blocking - there are more than one consumer to be served 
		/// by the producer
		/// Important ! If consumer postpones the processing of the data consumer should clone 
		/// the data (call data.Clone()) and use the copy for further processing.
		/// Consumer should not modify data
		/// consumer has two options
		/// 1) handle the data in the context of the Collector thread
		/// 2) clone the data and and postopone the procesing (delegate to another thread)
		/// </summary>
		/// <param name="count">
		/// A <see cref="System.Int32"/>
		/// Number of objects available
		/// </param>
		void Notify(int count, DataType data);
	}

	public interface IProducer<DataType> : IResourceProducer
	{
		bool AddConsumer(IConsumer<DataType> consumer);
		bool RemoveConsumer(IConsumer<DataType> consumer);
	}

	public abstract class ProducerBase<DataType> : IDisposable, IProducer<DataType>
	{
		protected ProducerBase()
		{
			Resources.Producers.Add(this);
		}

		public virtual void Dispose()
		{
			Resources.Producers.Remove(this);
		}

		public string Name
		{
			get;
			set;
		}

		public abstract bool AddConsumer(IConsumer<DataType> consumer);
		public abstract bool RemoveConsumer(IConsumer<DataType> consumer);


		public abstract void GetEventCounters(out System.Collections.ArrayList names,
										 out System.Collections.ArrayList values);
	}
}//namespace JQuant
