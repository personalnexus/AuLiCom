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

        //
        // Aggregator function used by combining channel values of multiple scenes/universes
        //

        internal static byte First(byte _, byte second) => second;
        internal static byte Second(byte _, byte second) => second;
        internal static byte Min(byte first, byte second) => Math.Min(first, second);
        internal static byte Max(byte first, byte second) => Math.Max(first, second);
    }
}
