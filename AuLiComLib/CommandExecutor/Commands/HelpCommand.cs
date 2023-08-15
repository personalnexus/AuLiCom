using AuLiComLib.Common;
using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.CommandExecutor.Commands
{
    internal class HelpCommand: ICommand
    {
        public HelpCommand(IConnection connection,
                           ICommandWriteConsole console, 
                           ICommandExecutor commandExecutor)
        {
            _connection = connection;
            _console = console;
            _commandExecutor = commandExecutor;
        }

        private readonly IConnection _connection;
        private readonly ICommandWriteConsole _console;
        private readonly ICommandExecutor _commandExecutor;

        public string Description => "HELP lists descriptions for all commands";

        public bool TryExecute(string command)
        {
            bool result;
            if (command.Equals("Help", StringComparison.OrdinalIgnoreCase))
            {
                _commandExecutor.GetCommandDescriptions().ForEach(_console.WriteLine);
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
    }
}
