namespace AuLiComLib.Protocols
{
    public interface IChannelValuePropertyHolder
    {
        int Channel { get; }
        IConnection Connection { get; }
    }
}