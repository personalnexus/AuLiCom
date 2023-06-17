using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Common
{
    public static class AsyncExecutorFactory
    {
        public static IAsyncExecutor CreateThread(string threadName) => new SystemThread(threadName);
    }
}
