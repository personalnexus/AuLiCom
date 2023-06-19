using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AuLiComLib.Fixtures.Kinds
{
    public abstract class FixtureBase : IChannelValuePropertyHolder
    {
        public FixtureBase(IConnection connection)
        {
            Kind = this.GetType().Name;
            Connection = connection;
        }

        // JSON configurable properties

        public string Kind { get; set; }

        public string? Name { get; set; }

        public int Channel { get; set; }

        // IChannelValuePropertyHolder

        [JsonIgnore]
        public IConnection Connection  { get; }
    }
}
