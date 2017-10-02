using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SuperFuncular.Helpers
{
    public class TimeKeeper : IDisposable
    {
        List<Timing> timings = new List<Timing>();
        private readonly TextWriter textWriter;

        public TimeKeeper(TextWriter textWriter)
        {
            this.textWriter = textWriter;
        }

        public void Dispose()
        {
            foreach (var timing in timings.OrderBy(r => r.ElapsedTime))
                timing.Print(textWriter);
        }

        internal void Add(Timing timing)
        {
            timings.Add(timing);
        }
    }
}
