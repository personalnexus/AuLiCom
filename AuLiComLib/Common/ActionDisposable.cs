using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Common
{
    public class ActionDisposable : IDisposable
    {
        public ActionDisposable(Action action) => _action = action;

        public void Dispose() => _action();

        private readonly Action _action;

        private static readonly Action EmptyAction = () => { };
        public static readonly ActionDisposable Empty = new(EmptyAction);
    }
}
