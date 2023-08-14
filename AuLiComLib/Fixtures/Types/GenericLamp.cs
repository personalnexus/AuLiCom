using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AuLiComLib.Fixtures.Types
{
    /// <summary>
    /// A generic lamp with a single channel to control intensity
    /// </summary>
    public class GenericLamp : FixtureBase, IFixture
    {
        public GenericLamp(IConnection connection) : base(connection)
        {
            Intensity = new ChannelValueProperty(this, 0);
        }

        public ChannelValueProperty Intensity { get; set; }
    }
}
