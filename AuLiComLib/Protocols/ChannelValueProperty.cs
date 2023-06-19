using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Protocols
{
    public readonly struct ChannelValueProperty
    {
        public ChannelValueProperty(IChannelValuePropertyHolder propertyHolder, int channelOffset)
        {
            _propertyHolder = propertyHolder;
            _channelOffset = channelOffset;
        }

        private readonly IChannelValuePropertyHolder _propertyHolder;
        private readonly int _channelOffset;

        public byte Value
        {
            get => _propertyHolder.Connection.GetValue(_propertyHolder.Channel + _channelOffset);
            set => _propertyHolder.Connection.SetValue(ChannelValue.FromByte(_propertyHolder.Channel + _channelOffset, value));
        }

        public static implicit operator byte(ChannelValueProperty valueProperty) => valueProperty.Value;
    }
}
