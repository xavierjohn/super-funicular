using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SuperFuncular.Helpers
{
    public class Timing
    {
        private readonly TextWriter textWriter;

        public Timing(string descripiton, TimeSpan elapsedTime)
        {
            Descripiton = descripiton;
            ElapsedTime = elapsedTime;
        }

        public string Descripiton { get; }
        public TimeSpan ElapsedTime { get; }

        public void Print(TextWriter textWriter = null)
        {
            if (textWriter != null)
                textWriter.WriteLine($"Timing for {Descripiton} is {ElapsedTime.Ticks}");
        }
    }

    public class PerformanceTimer : IDisposable
    {
        public PerformanceTimer(string description, TextWriter textWriter = null)
        {
            stopWatch = new Stopwatch();
            stopWatch.Start();
            this.description = description;
            this.textWriter = textWriter;
        }

        public PerformanceTimer(string description, TimeKeeper timeKeeper) : this(description)
        {
            this.timeKeeper = timeKeeper;
        }

        Stopwatch stopWatch;
        private readonly string description;
        private readonly TextWriter textWriter;
        private TimeKeeper timeKeeper;

        public void Dispose()
        {
            stopWatch.Stop();
            var timing = new Timing(description, stopWatch.Elapsed);
            timing.Print(textWriter);
            timeKeeper.Add(timing);
        }
    }
}
