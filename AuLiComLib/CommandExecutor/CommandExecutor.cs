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
       public CommandExecutor(IConnection connection,
                              ICommandWriteConsole console,
                              ICommandFixtures fixtures)
        {
            _commands = new ICommand[]
            {
                new ClearChannelValuesCommand(connection),
                new SetChannelValueCommand(connection, console, fixtures),
                new ListChannelValuesCommand(connection, console),
                // TODO: register commands here when creating new ones
            };
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
