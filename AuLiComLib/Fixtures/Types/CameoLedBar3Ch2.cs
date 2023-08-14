using AuLiComLib.Protocols;

namespace AuLiComLib.Fixtures.Types
{
    public class CameoLedBar3Ch2 : FixtureBase, IFixture
    {
        public CameoLedBar3Ch2(IConnection connection) : base(connection)
        {
        }

        public ChannelValueProperty Red { get; set; }
        public ChannelValueProperty Green { get; set; }
        public ChannelValueProperty Blue { get; set; }
    }
}
