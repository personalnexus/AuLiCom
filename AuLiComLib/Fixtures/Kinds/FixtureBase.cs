using AuLiComLib.Protocols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Fixtures.Kinds
{
    public abstract class FixtureBase : IChannelValuePropertyHolder
    {
        public FixtureBase(IConnection connection)
        {
            Connection = connection;
            Name = "";
        }

        // JSON configurable properties

        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        public int Channel { get; set; }

        // IChannelValuePropertyHolder

        [JsonIgnore]
        public IConnection Connection  { get; }
    }
}
