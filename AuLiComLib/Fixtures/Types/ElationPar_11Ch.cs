using AuLiComLib.Colors.Channels;
using AuLiComLib.Protocols;

namespace AuLiComLib.Fixtures.Types
{
    public class ElationKlParFc_11Ch : FixtureBase, IFixture
    {
        public ElationKlParFc_11Ch(IConnection connection) : base(connection)
        {
        }

        public ChannelValueProperty Intensity { get; set; }
        public ChannelValueProperty IntensityFine { get; set; }
        public ChannelValueProperty Strobe { get; set; }

        public RedChannelValueProperty Red { get; set; }
        public GreenChannelValueProperty Green { get; set; }
        public BlueChannelValueProperty Blue { get; set; }

        public ChannelValueProperty Mint { get; set; }
        public ChannelValueProperty Amber { get; set; }
        public ChannelValueProperty Cto { get; set; }

        public ChannelValueProperty ColorWheel { get; set; }
    }
}
