using AuLiComLib.Colors.Channels;
using AuLiComLib.Fixtures;
using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComTest.TestData
{
    internal class FixtureWithTwoReds : FixtureBase
    {
        public FixtureWithTwoReds(IConnection connection) : base(connection)
        {
        }

        public RedChannelValueProperty Red1 { get; set; }
        public RedChannelValueProperty Red2 { get; set; }
    }
}
