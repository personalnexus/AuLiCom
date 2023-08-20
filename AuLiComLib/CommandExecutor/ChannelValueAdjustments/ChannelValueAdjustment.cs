using AuLiComLib.Protocols;

namespace AuLiComLib.CommandExecutor.ChannelValueAdjustments
{
    public readonly struct ChannelValueAdjustment
    {
        public ChannelValueAdjustment(IEnumerable<int> channels, IChannelValueAdjustmentStrategy adjustmentStrategy)
        {
            Channels = channels.ToArray();
            _adjustmentStrategy = adjustmentStrategy;
        }

        public int[] Channels { get; }
        private readonly IChannelValueAdjustmentStrategy _adjustmentStrategy;

        public IMutableUniverse ApplyTo(IReadOnlyUniverse source) 
        {
            var result = source.ToMutableUniverse();
            foreach (int channel in Channels)
            {
                ChannelValue sourceChannel = source.GetValue(channel);
                _adjustmentStrategy.ApplyTo(sourceChannel, result);
            }
            return result;
        }
    }
}
