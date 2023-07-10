using AuLiComLib.Protocols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Fixtures
{
    public abstract class FixtureBase : IChannelValuePropertyHolder, IConfigurableFixture
    {
        public FixtureBase(IConnection connection)
        {
            Connection = connection;
            Name = "";
            _channelValuePropertyInfos =
                GetType()
                .GetProperties()
                .Where(x => x.PropertyType == typeof(ChannelValueProperty))
                .ToArray();
            foreach (PropertyInfo? channelValuePropertyInfo in _channelValuePropertyInfos)
            {
                channelValuePropertyInfo.SetValue(this, new ChannelValueProperty(this, ChannelCount++));
            }
        }

        // IFixture

        public int ChannelCount { get; }

        public FixtureInfo GetFixtureInfo() => new FixtureInfo(FixtureName: Name,
                                                               FixtureType: GetType().Name,
                                                               StartChannel: StartChannel,
                                                               Alias: Alias);

        public IEnumerable<FixtureChannelInfo> GetFixtureChannelInfos() =>
            _channelValuePropertyInfos
            .Select(x => new FixtureChannelInfo(
                FixtureName: Name,
                FixtureType: GetType().Name,
                FixtureAlias: Alias,
                ChannelName: x.Name,
                StartChannel: GetChannelValue(x).Channel));

        // Cached accessors for channel value properties

        private PropertyInfo[] _channelValuePropertyInfos;

        private ChannelValue GetChannelValue(PropertyInfo x) => ((ChannelValueProperty)x.GetValue(this)).ChannelValue;


        // JSON configurable properties

        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int StartChannel { get; set; }

        [JsonProperty]
        public string? Alias { get; set; }

        // IChannelValuePropertyHolder

        [JsonIgnore]
        public IConnection Connection { get; }
    }
}
