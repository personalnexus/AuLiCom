namespace AuLiComLib.CommandExecutor
{
    public interface ICommandConsole
    {
        string ReadLineTrim();
        void WriteLine(string line);
        void WriteLine();
    }
}