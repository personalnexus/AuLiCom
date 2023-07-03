using AuLiComLib.Protocols;

namespace AuLiComLib.CommandExecutor.ChannelValueCommands
{
    public interface IChannelValueAdjustmentStrategy
    {
        ChannelValue ApplyTo(ChannelValue source);
    }
}