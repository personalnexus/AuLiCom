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

            foreach (PropertyInfo? channelValuePropertyInfo in GetChannelValuePropertyInfos())
            {
                channelValuePropertyInfo.SetValue(this, new ChannelValueProperty(this, ChannelCount++));
            }
        }

        // IFixture

        public int ChannelCount { get; }

        public FixtureInfo GetFixtureInfo() => new FixtureInfo(FixtureName: Name,
                                                               FixtureType: GetType().Name,
                                                               StartChannel: StartChannel);

        public IEnumerable<FixtureChannelInfo> GetFixtureChannelInfos()
        {
            return GetChannelValuePropertyInfos()
                .Select(x => new FixtureChannelInfo(
                    FixtureName: Name,
                    FixtureType: GetType().Name,
                    ChannelName: x.Name,
                    StartChannel: ((ChannelValueProperty)x.GetValue(this)).Channel));
        }

        private IEnumerable<PropertyInfo> GetChannelValuePropertyInfos()
        {
            return GetType()
                   .GetProperties()
                   .Where(x => x.PropertyType == typeof(ChannelValueProperty));
        }

        // JSON configurable properties

        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int StartChannel { get; set; }

        // IChannelValuePropertyHolder

        [JsonIgnore]
        public IConnection Connection { get; }
    }
}
