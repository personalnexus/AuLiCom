using AuLiComLib.Colors.Channels;
using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Fixtures.Types
{
    public class CameoPixBar650CPro_3Ch1 : FixtureBase, IFixture
    {
        public CameoPixBar650CPro_3Ch1(IConnection connection) : base(connection)
        {
        }

        public RedChannelValueProperty Red { get; set; }
        public GreenChannelValueProperty Green { get; set; }
        public BlueChannelValueProperty Blue { get; set; }
    }
}
