﻿using AuLiComLib.Protocols;

namespace AuLiComLib.CommandExecutor.ChannelValueAdjustments
{
    public class ChannelValueAdjustmentStrategyAddPercentage : SingleChannelValueAdjustmentStrategyBase
    {
        public ChannelValueAdjustmentStrategyAddPercentage(int addedPercentage)
        {
            _addedPercentage = addedPercentage;
        }

        private readonly int _addedPercentage;

        protected override ChannelValue ApplyTo(ChannelValue source) => ChannelValue.FromPercentageWithRangeLimits(source.Channel, source.ValueAsPercentage + _addedPercentage);
    }
}
