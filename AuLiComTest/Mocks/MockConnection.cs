using AuLiComLib.Protocols;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComTest.Mocks
{
    internal class MockConnection : IConnection
    {
        public byte[] Values = new byte[513];

        public ChannelValue GetValue(int channel) => ChannelValue.FromByte(channel, Values[channel]);

        public IEnumerable<ChannelValue> GetValues() => Values.Select((value, channel) => ChannelValue.FromByte(channel, value));

        public void SetValue(ChannelValue channelValue) => Values[channelValue.Channel] = channelValue.Value;

        public void SetValues(IEnumerable<ChannelValue> channelValues)
        {
            foreach (ChannelValue channelValue in channelValues)
            {
                SetValue(channelValue);
            }
        }

        public void SetValuesToZero()
        {
            Values = new byte[513];
        }
    }
}
