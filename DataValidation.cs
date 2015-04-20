
using System;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

namespace JQuant
{

	/// <summary>
	/// Data validation in the simplest case is synchronous action which takes place
	/// between data producer, like Collector and data handler (consumer) like logger
	/// Data validation can contain a state machine which tracks previous events and
	/// flags the bad data based on the previous history
	/// </summary>
	public abstract class SyncVerifier<DataType, ErrorType> : IResourceDataVerifier, IDisposable
	{
		protected SyncVerifier(string name)
		{
			// set name of the verifier
			this.Name = name;

			// register in the system
			Resources.Verifiers.Add(this);
		}

		/// <summary>
		/// call this method to clean up - remove the verifier from the list of
		/// all running in the system verifiers
		/// </summary>
		public virtual void Dispose()
		{
			// remove myself from the list of created verifiers
			Resources.Verifiers.Remove(this);
		}

		public abstract ErrorType Verify(DataType data);
		public abstract void GetEventCounters(out System.Collections.ArrayList names, out System.Collections.ArrayList values);

		public string Name
		{
			get;
			set;
		}


	} // class SyncVerification


	public abstract class SyncVerifierBool<DataType> : SyncVerifier<DataType, bool>
	{
		protected SyncVerifierBool(string name)
			: base(name)
		{
		}

		public override void GetEventCounters(out System.Collections.ArrayList names, out System.Collections.ArrayList values)
		{
			names = new System.Collections.ArrayList(8);
			values = new System.Collections.ArrayList(8);

			names.Add("Calls"); values.Add(invocations);
			names.Add("True"); values.Add(returnedTrue);
			names.Add("False"); values.Add(returnedFalse);
		}

		public override abstract bool Verify(DataType data);

		protected int invocations;
		protected int returnedTrue;
		protected int returnedFalse;

	} // class SyncVerificationBool
} // namespace FMRShell
