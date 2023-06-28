using AuLiComLib.Common;
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
    public class CommandExecutor : ICommandExecutor
    {
        private static ICommand[] CreateCommands(IConnection connection, ICommandWriteConsole console) => new ICommand[]
        {
            new ClearChannelValuesCommand(connection),
            new SetChannelValueCommand(connection),
            new ListChannelValuesCommand(connection, console),
            // TODO: register commands here when creating new ones
        };

        public CommandExecutor(IConnection connection,
                               ICommandWriteConsole console)
        {
            _commands = CreateCommands(connection, console);
        }

        private readonly ICommand[] _commands;

        public string Execute(string commandString)
        {
            string result;
            if (!_commands.Any(x => x.TryExecute(commandString)))
            {
                result = $"INVALID '{commandString}'.";
            }
            else
            {
                result = "DONE.";
            }
            return result;
        }

        public IEnumerable<string> GetCommandDescriptions() => 
            _commands
            .Select(x => x.Description)
            .Order();
    }
}
