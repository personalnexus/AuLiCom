using AuLiComLib.Protocols;

namespace AuLiComLib.CommandExecutor.ChannelValueAdjustments
{
    public class ChannelValueAdjustmentStrategyAddPercentage : IChannelValueAdjustmentStrategy
    {
        public ChannelValueAdjustmentStrategyAddPercentage(int addedPercentage)
        {
            _addedPercentage = addedPercentage;
        }

        private readonly int _addedPercentage;

        public ChannelValue ApplyTo(ChannelValue source) => ChannelValue.FromPercentageWithRangeLimits(source.Channel, source.ValueAsPercentage + _addedPercentage);
    }
}
