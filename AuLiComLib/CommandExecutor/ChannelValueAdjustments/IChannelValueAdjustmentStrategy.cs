using AuLiComLib.Protocols;

namespace AuLiComLib.CommandExecutor.ChannelValueAdjustments
{
    public interface IChannelValueAdjustmentStrategy
    {
        ChannelValue ApplyTo(ChannelValue source);
    }
}