namespace AuLiComLib.Protocols
{
    /// <summary>
    /// Abstract interface describing a connection e.g. via DMX to get/set channel values
    /// </summary>
    public interface IConnection: IDisposable
    {
        IReadOnlyUniverse CurrentUniverse { get; }
        void SendUniverse(IReadOnlyUniverse universe);
    }
}