using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Fixtures.Kinds
{
    public class CameoPixBar650CPro3Ch1 : FixtureBase, IFixture
    {
        public CameoPixBar650CPro3Ch1(IConnection connection) : base(connection)
        {
        }

        public ChannelValueProperty Red { get; set; }
        public ChannelValueProperty Green { get; set; }
        public ChannelValueProperty Blue { get; set; }
    }
}
