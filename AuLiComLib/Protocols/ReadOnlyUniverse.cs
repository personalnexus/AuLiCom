using AuLiComLib.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
    }
}
