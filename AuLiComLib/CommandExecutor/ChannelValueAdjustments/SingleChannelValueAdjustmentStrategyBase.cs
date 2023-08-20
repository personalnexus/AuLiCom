using AuLiComLib.Protocols;

namespace AuLiComLib.CommandExecutor.ChannelValueAdjustments
{
    public abstract class SingleChannelValueAdjustmentStrategyBase : IChannelValueAdjustmentStrategy
    {
        public void ApplyTo(ChannelValue source, IMutableUniverse target)
        {
            ChannelValue targetChannel = ApplyTo(source);
            target.SetValue(targetChannel);
        }

        protected abstract ChannelValue ApplyTo(ChannelValue source);
    }
}