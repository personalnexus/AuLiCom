using AuLiComLib.Protocols;

namespace AuLiComLib.CommandExecutor.ChannelValueCommands
{
    public class ChannelValueAdjustmentStrategySetPercentage : IChannelValueAdjustmentStrategy
    {
        public ChannelValueAdjustmentStrategySetPercentage(int percentage)
        {
            _percentage = percentage;
        }

        private readonly int _percentage;

        public ChannelValue ApplyTo(ChannelValue source) => ChannelValue.FromPercentage(source.Channel, _percentage);
    }
}
