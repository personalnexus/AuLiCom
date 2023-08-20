using AuLiComLib.Protocols;

namespace AuLiComLib.CommandExecutor.ChannelValueAdjustments
{
    public class ChannelValueAdjustmentStrategySetPercentage : SingleChannelValueAdjustmentStrategyBase
    {
        public ChannelValueAdjustmentStrategySetPercentage(int percentage)
        {
            _percentage = percentage;
        }

        private readonly int _percentage;

        protected override ChannelValue ApplyTo(ChannelValue source) => ChannelValue.FromPercentage(source.Channel, _percentage);
    }
}
