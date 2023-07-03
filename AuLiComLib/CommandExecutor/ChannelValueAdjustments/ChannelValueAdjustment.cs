using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AuLiComLib.CommandExecutor.ChannelValueCommands
{
    public readonly struct ChannelValueAdjustment
    {
        public ChannelValueAdjustment(IEnumerable<int> channels, IChannelValueAdjustmentStrategy adjustmentStrategy)
        {
            _channels = channels;
            _adjustmentStrategy = adjustmentStrategy;
        }

        private readonly IEnumerable<int> _channels;
        private readonly IChannelValueAdjustmentStrategy _adjustmentStrategy;

        public IMutableUniverse ApplyTo(IReadOnlyUniverse source) 
        {
            var result = source.ToMutableUniverse();
            foreach (int channel in _channels)
            {
                ChannelValue sourceChannel = source.GetValue(channel);
                ChannelValue targetChannel = _adjustmentStrategy.ApplyTo(sourceChannel);
                result.SetValue(targetChannel);
            }
            return result;
        }
    }
}
