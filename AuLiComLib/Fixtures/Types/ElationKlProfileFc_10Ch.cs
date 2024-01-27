using AuLiComLib.Colors.Channels;
using AuLiComLib.Protocols;

namespace AuLiComLib.Fixtures.Types
{
    public class ElationKlProfileFc_10Ch : FixtureBase, IFixture
    {
        public ElationKlProfileFc_10Ch(IConnection connection) : base(connection)
        {
        }

        public RedChannelValueProperty Red { get; set; }
        public GreenChannelValueProperty Green { get; set; }
        public BlueChannelValueProperty Blue { get; set; }

        public ChannelValueProperty Cto { get; set; }
        public ChannelValueProperty ColorWheel { get; set; }
        public ChannelValueProperty Gobo { get; set; }

        public ChannelValueProperty ShutterStrobe { get; set; }
        public ChannelValueProperty Intensity { get; set; }
        public ChannelValueProperty IntensityFine { get; set; }

        public ChannelValueProperty Control { get; set; }
    }
}
