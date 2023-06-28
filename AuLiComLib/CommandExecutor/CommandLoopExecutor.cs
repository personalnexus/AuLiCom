using AuLiComLib.Common;
using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AuLiComLib.CommandExecutor
{
    public class CommandLoopExecutor : CommandExecutor, ICommandLoopExecutor
    {
        public CommandLoopExecutor(IConnection connection, ICommandReadWriteConsole console) : base(connection, console)
        {
            _console = console;
        }

        private readonly ICommandReadWriteConsole _console;

        public void Loop()
        {
            _console.WriteLine();
            _console.WriteLine("The following commands are available. Press <Enter> after each command. An empty line terminates the program.");
            _console.WriteLine(GetCommandDescriptions().ToDelimitedString(Environment.NewLine));
            _console.WriteLine();

            string? commandString = _console.ReadLineTrim();
            while (!string.IsNullOrEmpty(commandString))
            {
                string commandResult = Execute(commandString);
                _console.WriteLine(commandResult);
                commandString = _console.ReadLineTrim();
            }
        }
    }
}
