
using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;

namespace JQuant
{

	partial class Program
	{


		protected void LoadCommandLineInterface()
		{
			cli.SystemMenu.AddCommand("exit", "Exit from the program",
				"Cleanup and exit", this.CleanupAndExit);

			LoadCommandLineInterface_oper();
			LoadCommandLineInterface_dbg();
			LoadCommandLineInterface_box();
			LoadCommandLineInterface_test();

			//LoadCommandLineInterface_sa();
			//LoadCommandLineInterface_feed();

#if USEFMRSIM
			//LoadCommandLineInterface_ms();
#endif
		}

	}//partial class Program
}//namespace JQuant
