using AuLiComLib.CommandExecutor.Commands;
using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.CommandExecutor
{
    public class CommandExecutor
    {
        public static ICommand[] CreateCommands(IConnection connection, ICommandConsole console) => new ICommand[]
        {
            new ClearChannelValuesCommand(connection),
            new SetChannelValueCommand(connection),
            new ListChannelValuesCommand(connection, console),
            // TODO: register commands here when creating new ones
        };

        public CommandExecutor(IConnection connection,
                               ICommandConsole console)
        {
            _commands = CreateCommands(connection, console);
            _console = console;
        }

        private readonly ICommand[] _commands;
        private readonly ICommandConsole _console;

        public void Loop()
        {
            _console.WriteLine();
            _console.WriteLine("The following commands are available. Press <Enter> after each command. An empty line terminates the program.");
            foreach (ICommand command in _commands.OrderBy(x => x.Description))
            {
                _console.WriteLine(command.Description);
            }
            _console.WriteLine();

            string commandString = _console.ReadLineTrim();
            while (!string.IsNullOrEmpty(commandString))
            {
                if (!_commands.Any(x => x.TryExecute(commandString)))
                {
                    _console.WriteLine($"invalid command '{commandString}'.");
                }
                commandString = _console.ReadLineTrim();
            }
        }
    }
}
