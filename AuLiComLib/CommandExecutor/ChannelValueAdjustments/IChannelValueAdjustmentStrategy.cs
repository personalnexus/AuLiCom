using AuLiComLib.Protocols;

namespace AuLiComLib.CommandExecutor.ChannelValueAdjustments
{
    public interface IChannelValueAdjustmentStrategy
    {
        void ApplyTo(ChannelValue source, IMutableUniverse target);
    }
}