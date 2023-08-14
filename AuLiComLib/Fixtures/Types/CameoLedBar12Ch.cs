using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Fixtures.Types
{
    public class CameoLedBar12Ch : FixtureBase, IFixture
    {
        public CameoLedBar12Ch(IConnection connection) : base(connection)
        {
        }

        public ChannelValueProperty Ignore1 { get; set; }
        public ChannelValueProperty Ignore2 { get; set; }
        public ChannelValueProperty Ignore3 { get; set; }

        public ChannelValueProperty Red1 { get; set; }
        public ChannelValueProperty Green1 { get; set; }
        public ChannelValueProperty Blue1 { get; set; }

        public ChannelValueProperty Red2 { get; set; }
        public ChannelValueProperty Green2 { get; set; }
        public ChannelValueProperty Blue2 { get; set; }

        public ChannelValueProperty Red3 { get; set; }
        public ChannelValueProperty Green3 { get; set; }
        public ChannelValueProperty Blue3 { get; set; }
    }
}
