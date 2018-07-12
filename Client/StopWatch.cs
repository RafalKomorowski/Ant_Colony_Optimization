using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Client
{
    class StopWatch
    {
        private static Stopwatch watch = new Stopwatch();

        public void WatchStart()
        {
            watch.Start();
        }

        public long WatchStop()
        {
            watch.Stop();
            long czas = watch.ElapsedMilliseconds;
            return czas;
        }
    }
}
