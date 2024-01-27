using AuLiComLib.Colors.Channels;
using AuLiComLib.Protocols;

namespace AuLiComLib.Fixtures.Types
{
    public class ElationProfiler_10Ch : FixtureBase, IFixture
    {
        public ElationProfiler_10Ch(IConnection connection) : base(connection)
        {
        }

        public RedChannelValueProperty Red { get; set; }
        public GreenChannelValueProperty Green { get; set; }
        public BlueChannelValueProperty Blue { get; set; }

        public ChannelValueProperty Cto { get; set; }
        public ChannelValueProperty Ignore1 { get; set; }
        public ChannelValueProperty Ignore2 { get; set; }

        public ChannelValueProperty Ignore3 { get; set; }
        public ChannelValueProperty Intensity { get; set; }
        public ChannelValueProperty Ignore4 { get; set; }

        public ChannelValueProperty Ignore5 { get; set; }
    }
}
