using AuLiComLib.Protocols;

namespace AuLiComLib.CommandExecutor.Commands
{
    public class GetSceneCommand : ICommand
    {
        public GetSceneCommand(ICommandNamedSceneManager sceneManager)
        {
            _sceneManager = sceneManager;
        }

        private readonly IConnection _connection;
        private readonly ICommandNamedSceneManager _sceneManager;

        public string Description => "GET <NameOfScene> clears the current universe and displays only values from the given scene";

        public bool TryExecute(string command)
        {
            bool result;
            if (command.StartsWith("GET ", StringComparison.OrdinalIgnoreCase) && command.Length > 4)
            {
                string name = command[4..];
                _sceneManager.ActivateSingleScene(name);
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
