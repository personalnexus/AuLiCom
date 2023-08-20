using AuLiComLib.Protocols;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
