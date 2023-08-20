using AuLiComLib.CommandExecutor;
using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComTest.Mocks
{
    internal class MockColorChannelValueProperties : ICommandColorChannelValueProperties
    {
        public MockColorChannelValueProperties(int startChannel)
        {
            Red   = ChannelValue.FromPercentage(startChannel + 0, 0);
            Green = ChannelValue.FromPercentage(startChannel + 1, 1);
            Blue  = ChannelValue.FromPercentage(startChannel + 2, 2);
        }

        public ChannelValue Red { get; }

        public ChannelValue Green { get; }

        public ChannelValue Blue { get; }
    }
}
