using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Common
{
    public interface IAsyncExecutor
    {
        void ExecuteAsync(Action action);
    }
}
