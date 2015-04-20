
using System;
using System.ComponentModel;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using JQuant;
using FMRShell;

//I need TaskBar objects
#if USEFMRSIM
using TaskBarLibSim;
#else
using TaskBarLib;
#endif

namespace Algo
{
	class BoxMessage
	{
	}
	
	class BoxAlgoFSM : MailboxThread<BoxMessage>, IResourceStatistics
	{
		BoxAlgoFSM(): base("BoxAlgo", 500)
		{
		}
		
		public void GetEventCounters(out System.Collections.ArrayList names, out System.Collections.ArrayList values)
		{
			names = new System.Collections.ArrayList(12);
			values = new System.Collections.ArrayList(12);
		}
		
	}//class BoxArbBase
}//namspace Algo
