using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Protocols
{
    /// <summary>
    ///  Immutable structure for the value of a single channel
    /// </summary>
    public readonly struct ChannelValue
    {
        private ChannelValue(int channel, byte value)
        {
            Channel = channel;
            Value = value;
        }

        public int Channel { get; }
        public byte Value { get; }

        public int ValueAsPercentage => (int)Math.Round(100 * (Value / 255d), MidpointRounding.AwayFromZero);
        public int ValueAsTenth => Value / 25;

        public static ChannelValue FromByte(int channel, byte value) => new(channel, value);
        public static ChannelValue FromPercentage(int channel, int percentage) => new(channel, (byte)Math.Round(255 * (percentage / 100d), MidpointRounding.ToZero));
        public static ChannelValue FromPercentageWithRangeLimits(int channel, double value) => FromPercentage(channel, value switch
        {
            > 100 => 100,
            < 0 => 0,
            _ => (int)value
        });

        //
        // Aggregator function used by combining channel values of multiple scenes/universes
        //

        public static byte First(ChannelValue first, ChannelValue _) => first.Value;
        public static byte Second(ChannelValue _, ChannelValue second) => second.Value;
        public static byte Min(ChannelValue first, ChannelValue second) => Math.Min(first.Value, second.Value);
        public static byte Max(ChannelValue first, ChannelValue second) => Math.Max(first.Value, second.Value);
    }
}
