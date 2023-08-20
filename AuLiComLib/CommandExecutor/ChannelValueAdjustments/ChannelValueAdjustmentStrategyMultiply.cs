using AuLiComLib.Protocols;

namespace AuLiComLib.CommandExecutor.ChannelValueAdjustments
{
    public class ChannelValueAdjustmentStrategyMultiply : SingleChannelValueAdjustmentStrategyBase
    {
        public ChannelValueAdjustmentStrategyMultiply(double factor)
        {
            _factor = factor;
        }
        public double _factor;

        protected override ChannelValue ApplyTo(ChannelValue source) => ChannelValue.FromPercentageWithRangeLimits(source.Channel, source.ValueAsPercentage * _factor);
    }
}
