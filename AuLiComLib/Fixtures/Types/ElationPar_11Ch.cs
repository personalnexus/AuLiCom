using AuLiComLib.Colors.Channels;
using AuLiComLib.Protocols;

namespace AuLiComLib.Fixtures.Types
{
    public class ElationPar_11Ch : FixtureBase, IFixture
    {
        public ElationPar_11Ch(IConnection connection) : base(connection)
        {
        }

        public ChannelValueProperty Intensity { get; set; }
        public ChannelValueProperty DimmerFine { get; set; }
        public ChannelValueProperty Strobe { get; set; }

        public RedChannelValueProperty Red { get; set; }
        public GreenChannelValueProperty Green { get; set; }
        public BlueChannelValueProperty Blue { get; set; }

        public ChannelValueProperty Ignore1 { get; set; }
        public ChannelValueProperty Ignore2 { get; set; }
        public ChannelValueProperty Cto { get; set; }
        public ChannelValueProperty Ignore3 { get; set; }
    }
}
