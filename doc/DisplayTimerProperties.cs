//Use this piece of code to determine the frequency and 
//the resolution of a StopWatch timer

using System;
using System.Text;
using System.Diagnostics;


namespace IsHighResCheck
{
    class DisplayTimerProperties
    {
        static void Main(string[] args)
        {
            if (Stopwatch.IsHighResolution)
            {
                Console.WriteLine("OK, operations timed using system's high-freq performance counter");
            }
            else
            {
                Console.WriteLine("NO, operations timed using the DateTime class");
            }
            long frequency = Stopwatch.Frequency;
            Console.WriteLine("Timer frequency in ticks per second = {0}",frequency);
            long nanosecsPerTick = (1000L * 1000L * 1000L) / frequency;
            Console.WriteLine("Timer is accurate within {0} nanoseconds",nanosecsPerTick);
            Console.ReadLine();
        }
    }
}
