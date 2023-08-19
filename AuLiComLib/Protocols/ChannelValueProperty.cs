using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Protocols
{
    public class ChannelValueProperty
    {
        public ChannelValueProperty(IChannelValuePropertyHolder propertyHolder, int channelOffset)
        {
            _propertyHolder = propertyHolder;
            _channelOffset = channelOffset;
        }

        private readonly IChannelValuePropertyHolder _propertyHolder;
        private readonly int _channelOffset;

        public int Channel => _propertyHolder.StartChannel + _channelOffset;

        public byte Value
        {
            get => _propertyHolder.Connection.GetValue(Channel).Value;
            set => _propertyHolder.Connection.SetValue(ChannelValue.FromByte(Channel, value));
        }

        public ChannelValue ChannelValue => ChannelValue.FromByte(Channel, Value);

        public static implicit operator byte(ChannelValueProperty valueProperty) => valueProperty.Value;
    }
}
