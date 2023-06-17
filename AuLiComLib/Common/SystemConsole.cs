using AuLiComLib.CommandExecutor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib
{
    public class SystemConsole : ICommandConsole
    {
        public string ReadLineTrim() => Console.ReadLine().Trim();

        public void WriteLine(string line) => Console.WriteLine(line);

        public void WriteLine() => Console.WriteLine();
    }
}
