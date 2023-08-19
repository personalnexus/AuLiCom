using AuLiComLib.Colors.Channels;
using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Fixtures.Types
{
    public class CameoLedBar_12Ch : FixtureBase, IFixture
    {
        public CameoLedBar_12Ch(IConnection connection) : base(connection)
        {
        }

        public ChannelValueProperty Ignore1 { get; set; }
        public ChannelValueProperty Ignore2 { get; set; }
        public ChannelValueProperty Ignore3 { get; set; }

        public RedChannelValueProperty Red1 { get; set; }
        public GreenChannelValueProperty Green1 { get; set; }
        public BlueChannelValueProperty Blue1 { get; set; }

        public RedChannelValueProperty Red2 { get; set; }
        public GreenChannelValueProperty Green2 { get; set; }
        public BlueChannelValueProperty Blue2 { get; set; }

        public RedChannelValueProperty Red3 { get; set; }
        public GreenChannelValueProperty Green3 { get; set; }
        public BlueChannelValueProperty Blue3 { get; set; }
    }
}
