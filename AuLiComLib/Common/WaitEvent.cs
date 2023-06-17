using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Common
{
    internal class WaitEvent
    {
        private readonly object _lock = new();

        public void Wait(TimeSpan timeout)
        {
            lock (_lock)
            { 
                Monitor.Wait(_lock, timeout);
            }
        }

        public void Set()
        {
            lock (_lock) 
            {
                Monitor.PulseAll(_lock);
            }
        }
    }
}
