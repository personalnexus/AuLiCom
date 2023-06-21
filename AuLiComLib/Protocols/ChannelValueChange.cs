using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Protocols
{
    internal readonly struct ChannelValueChange
    {
        public ChannelValueChange(ChannelValue currentChannelValue, byte targetValue, int stepCount)
        {
            _channel = currentChannelValue.Channel;
            _startValue = currentChannelValue.Value;
            _changePerStep = ((decimal)targetValue - (decimal)_startValue) / stepCount;
        }

        private readonly int _channel;
        private readonly byte _startValue;
        private readonly decimal _changePerStep;

        public int Channel => _channel;
        public bool HasChange => _changePerStep != 0m;

        public ChannelValue GetNextValue(int step)
        {
            byte newValue = (byte)Math.Round((_changePerStep * (step + 1)) + _startValue, 0, MidpointRounding.ToZero);
            return ChannelValue.FromByte(_channel, newValue);
        }
    }
}
