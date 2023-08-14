using AuLiComLib.Protocols;

namespace AuLiComLib.Fixtures.Types
{
    public class CameoPixBar650CPro3Ch2: FixtureBase, IFixture
    {
        public CameoPixBar650CPro3Ch2(IConnection connection) : base(connection)
        {
        }

        public ChannelValueProperty Intensity { get; set; }
        public ChannelValueProperty Strobe { get; set; }
        public ChannelValueProperty ColorMacro { get; set; }
    }
}
