using AuLiComLib.Colors.Channels;
using AuLiComLib.Protocols;

namespace AuLiComLib.Fixtures.Types
{
    public class CameoThunderWash600Rgbw_7Ch2 : FixtureBase, IFixture
    {
        public CameoThunderWash600Rgbw_7Ch2(IConnection connection) : base(connection)
        {
        }

        public ChannelValueProperty Intensity { get; set; }
        public ChannelValueProperty StrobeSpeed { get; set; }
        public ChannelValueProperty FlashDuration { get; set; }

        public RedChannelValueProperty Red { get; set; }
        public GreenChannelValueProperty Green { get; set; }
        public BlueChannelValueProperty Blue { get; set; }

        public ChannelValueProperty White { get; set; }
    }
}
