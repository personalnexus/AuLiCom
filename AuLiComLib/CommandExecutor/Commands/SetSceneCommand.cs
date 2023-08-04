using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.CommandExecutor.Commands
{
    public class SetSceneCommand : ICommand
    {
        public SetSceneCommand(IConnection connection,
                               ICommandNamedSceneManager sceneManager)
        {
            _connection = connection;
            _sceneManager = sceneManager;
        }

        private readonly IConnection _connection;
        private readonly ICommandNamedSceneManager _sceneManager;

        public string Description => "SET <NameOfScene> sets a scene under the given name with the current channel values (overrides the scene if it already exists)";

        public bool TryExecute(string command)
        {
            bool result;
            if (command.StartsWith("SET ", StringComparison.OrdinalIgnoreCase) && command.Length > 5)
            {
                string name = command[2..];
                _sceneManager.SetScene(name, _connection.CurrentUniverse);
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
