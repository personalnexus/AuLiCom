namespace AuLiComLib.Protocols
{
    public interface IChannelValuePropertyHolder
    {
        int StartChannel { get; }
        IConnection Connection { get; }
    }
}