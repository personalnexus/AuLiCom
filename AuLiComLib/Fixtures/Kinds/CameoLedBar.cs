using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Fixtures.Kinds
{
    public class CameoLedBar3Ch2: FixtureBase, IFixture
    {
        public CameoLedBar3Ch2(IConnection connection) : base(connection)
        {
            Red = new ChannelValueProperty(this, 0);
            Green = new ChannelValueProperty(this, 1);
            Blue = new ChannelValueProperty(this, 2);
        }

        public ChannelValueProperty Red { get; set; }
        public ChannelValueProperty Green { get; set; }
        public ChannelValueProperty Blue { get; set; }
    }
}
