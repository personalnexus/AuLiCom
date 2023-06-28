namespace AuLiComLib.CommandExecutor
{
    public interface ICommandExecutor
    {
        string Execute(string commandString);
        IEnumerable<string> GetCommandDescriptions();
    }
}