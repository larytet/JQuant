
using System;
using System.IO;
using FMRShell;
using Algo;

#if USEFMRSIM
using TaskBarLibSim;
#else
using TaskBarLib;
#endif

namespace JQuant
{
	partial class Program
	{

		protected void LoadCommandLineInterface_box()
		{
			Menu menuBox = cli.RootMenu.AddMenu(
				"box",
				"Box arb algo",
				"Box Arb, by default runs in semiautomatic mode");

		}//LoadCommandLineInterface_box
	}//partial class Program
}//namespace JQuant
