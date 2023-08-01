using AuLiComLib.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Protocols
{
    internal class ReadOnlyUniverse: Universe, IReadOnlyUniverse
    {
        internal ReadOnlyUniverse(byte[] values)
        {
            _values = values;
        }

        private readonly byte[] _values;

        protected void SetValueInternal(int channel, byte value) => _values[channel] = value;

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
            port.Write(_values, 0, Universe.ValuesLength);
        }

        // IEqualityComparer

        public static IEqualityComparer<IReadOnlyUniverseProvider>? HasSameValuesComparer { get; internal set; } = new ReadOnlyUniverseHasSameValuesComparer();

        private class ReadOnlyUniverseHasSameValuesComparer : IEqualityComparer<IReadOnlyUniverseProvider>
        {
            public bool Equals(IReadOnlyUniverseProvider? x, IReadOnlyUniverseProvider? y) =>
                ReferenceEquals(x?.Universe, y?.Universe)
                || (x?.Universe != null && y?.Universe != null && x.Universe.HasSameValuesAs(y.Universe));

            public int GetHashCode([DisallowNull] IReadOnlyUniverseProvider obj) => obj.GetHashCode();
        }

        public bool HasSameValuesAs(IReadOnlyUniverse other)
        {
            bool result = true;
            for(int channel = FirstChannel;channel < ValuesLength; channel++)
            {
                if (_values[channel] != other.GetValue(channel).Value)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
    }
}
