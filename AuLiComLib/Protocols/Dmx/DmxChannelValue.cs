using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Protocols.Dmx
{
    public readonly struct DmxChannelValue
    {
        private DmxChannelValue(int channel, byte value)
        {
            Channel = channel;
            Value = value;
        }

        public int Channel { get; }
        public byte Value { get; }
        public int ValueAsPercentage => (int)Math.Round(100 * (Value / 255d), MidpointRounding.AwayFromZero);
        public int ValueAsTenth => Value / 25;

        public static DmxChannelValue FromByte(int channel, byte value) => new(channel, value);
        public static DmxChannelValue FromPercentage(int channel, int percentage) => new(channel, (byte)Math.Round(255 * (percentage / 100d), MidpointRounding.ToZero));
    }
}
