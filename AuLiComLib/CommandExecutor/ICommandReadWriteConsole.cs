namespace AuLiComLib.CommandExecutor
{
    public interface ICommandReadWriteConsole: ICommandWriteConsole
    {
        string? ReadLineTrim();
    }
}