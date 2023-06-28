using AuLiComLib.CommandExecutor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Common
{
    public class StringBuilderWriteConsole : ICommandWriteConsole
    {
        private readonly StringBuilder _lines = new();

        void ICommandWriteConsole.WriteLine(string line) => _lines.AppendLine(line);

        void ICommandWriteConsole.WriteLine() => _lines.AppendLine();

        public string GetLines() => _lines.ToString();
    }
}
