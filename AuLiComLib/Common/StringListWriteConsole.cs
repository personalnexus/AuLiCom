using AuLiComLib.CommandExecutor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Common
{
    public class StringListWriteConsole : ICommandWriteConsole, IEnumerable<string>
    {
        private readonly List<string> _lines = new();

        public void Clear() => _lines.Clear();

        // IEnumerable<string>

        public IEnumerator<string> GetEnumerator()
        {
            return ((IEnumerable<string>)_lines).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_lines).GetEnumerator();
        }

        // ICommandWriteConsole

        void ICommandWriteConsole.WriteLine(string line) => _lines.Add(line);

        void ICommandWriteConsole.WriteLine() => _lines.Add("");
    }
}
