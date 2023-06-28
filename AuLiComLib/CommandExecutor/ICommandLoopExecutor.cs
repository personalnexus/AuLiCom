namespace AuLiComLib.CommandExecutor
{
    public interface ICommandLoopExecutor
    {
        void Loop();
        IEnumerable<string> GetCommandDescriptions();
    }
}