using AuLiComLib.Common;
using AuLiComLib.CommandExecutor.Commands;
using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Abstractions;

namespace AuLiComLib.CommandExecutor
{
    public class CommandExecutor : ICommandExecutor
    {
       public CommandExecutor(IConnection connection,
                              ICommandWriteConsole console,
                              ICommandFixtures fixtures,
                              ICommandNamedSceneManager sceneManager,
                              IFileSystem fileSystem)
        {
            _commands = new ICommand[]
            {
                new ClearChannelValuesCommand(connection),
                new ListChannelValuesCommand(connection, console),
                new LoadFileCommand(connection, console, fixtures, fileSystem),
                new SetSceneCommand(connection, sceneManager),
                new GetSceneCommand(sceneManager),
                // TODO: register commands here when creating new ones
                // SetChannelValueCommand must come last, because it does not start with a command
                // name and therefore outputs an error when encountering an invalid command.
                new ChangeChannelValueCommand(connection, console, fixtures),
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
