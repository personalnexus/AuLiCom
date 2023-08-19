using AuLiComLib.Colors.Channels;
using AuLiComLib.Protocols;

namespace AuLiComLib.Fixtures.Types
{
    public class CameoLedBar_3Ch2 : FixtureBase, IFixture
    {
        public CameoLedBar_3Ch2(IConnection connection) : base(connection)
        {
        }

        public RedChannelValueProperty Red { get; set; }
        public GreenChannelValueProperty Green { get; set; }
        public BlueChannelValueProperty Blue { get; set; }
    }
}
