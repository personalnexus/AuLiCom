using AuLiComLib.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Protocols
{
    public class Universe : IMutableUniverse, IReadOnlyUniverse
    {
        internal Universe()
        {
            _values = CreateEmptyValuesCopy();
        }

        internal Universe(IReadOnlyUniverse source)
        {
            _values = source.GetValuesCopy();
        }

        internal Universe(IEnumerable<ChannelValue> channelValues) : this()
        {
            foreach (ChannelValue channelValue in channelValues)
            {
                _values[channelValue.Channel] = channelValue.Value;
            }
        }

        public const int ChannelCount = 512;
        private const int FirstChannel = 1;
        private const int ValuesLength = FirstChannel + ChannelCount;

        private readonly byte[] _values;

        //
        // IMutableUniverse
        //

        public static IMutableUniverse CreateEmpty() => new Universe();

        public IMutableUniverse SetValue(ChannelValue channelValue)
        {
            _values[channelValue.Channel] = channelValue.Value;
            return this;
        }

        public IMutableUniverse SetValues(IEnumerable<ChannelValue> channelValues)
        {
            foreach (ChannelValue channelValue in channelValues)
            {
                SetValue(channelValue);
            }
            return this;
        }

        public IReadOnlyUniverse AsReadOnly() => this;

        //
        // IReadOnlyUniverse
        //

        public ChannelValue GetValue(int channel) => ChannelValue.FromByte(channel, _values[channel]);

        public IEnumerable<ChannelValue> GetValues()
        {
            for (int channel = FirstChannel; channel < ValuesLength; channel++)
            {
                yield return ChannelValue.FromByte(channel, _values[channel]);
            }
        }

        public byte[] GetValuesCopy() => _values.BlockCopy();

        public void WriteValuesTo(ISerialPort port)
        {
            port.Write(_values, 0, ValuesLength);
        }

        //
        // Helper functions for internal use only
        //

        internal static byte[] CreateEmptyValuesCopy() => new byte[ValuesLength];

        internal static bool IsValidChannel(int channel) => channel is > 0 and <= ChannelCount;

        internal Universe CombineWith(IReadOnlyUniverse other, Func<byte, byte, byte> aggregatingChannelValuesWith)
        {
            for (int channel = 1; channel < ChannelCount; channel++)
            {
                _values[channel] = aggregatingChannelValuesWith(_values[channel], other.GetValue(channel).Value);
            }
            return this;
        }
    }
}
