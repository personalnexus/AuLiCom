using AuLiComLib.Colors;
using AuLiComLib.Colors.Channels;
using AuLiComLib.CommandExecutor;
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
            _colors = new List<ColorChannelValueProperties>();
            _channelValuePropertyInfos =
                GetType()
                .GetProperties()
                .Where(x => x.PropertyType.IsAssignableTo(typeof(ChannelValueProperty)))
                .ToArray();

            RedChannelValueProperty red = null;
            GreenChannelValueProperty green = null;
            BlueChannelValueProperty blue = null;

            foreach (PropertyInfo? channelValuePropertyInfo in _channelValuePropertyInfos)
            {
                ChannelValueProperty newValue = channelValuePropertyInfo.PropertyType.Name switch
                {
                    nameof(RedChannelValueProperty) => AddColor(new RedChannelValueProperty(this, ChannelCount++), ref red, "Red"),
                    nameof(GreenChannelValueProperty) => AddColor(new GreenChannelValueProperty(this, ChannelCount++), ref green, "Green"),
                    nameof(BlueChannelValueProperty) => AddColor(new BlueChannelValueProperty(this, ChannelCount++), ref blue, "Blue"),
                    _ => new ChannelValueProperty(this, ChannelCount++)

                };
                channelValuePropertyInfo.SetValue(this, newValue);

                T AddColor<T>(T newValue, ref T target, string colorName)
                {
                    if (target != null)
                    {
                        throw new InvalidColorException($"Cannot add another {colorName} channel at offset {ChannelCount-1} before a color is complete with one Red, one Green and one Blue.");
                    }
                    target = newValue;
                    if (red != null && green != null && blue != null) 
                    {
                        _colors.Add(new ColorChannelValueProperties(red, green, blue));
                        red = null;
                        green = null;
                        blue = null;
                    }
                    return newValue;
                }
            }
        }

        // Colors

        private readonly List<ColorChannelValueProperties> _colors;

        public IReadOnlyList<ColorChannelValueProperties> Colors => _colors;


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
                StartChannel: GetChannelValueProperty(x).Channel));

        public bool TryGetColorChannelValueProperties(int channel, out ICommandColorChannelValueProperties colorProperties)
        {
            colorProperties = _colors.FirstOrDefault(x => x.ContainsChannel(channel));
            return colorProperties != null;
        }


        // Cached accessors for channel value properties

        private PropertyInfo[] _channelValuePropertyInfos;

        private ChannelValueProperty GetChannelValueProperty(PropertyInfo x) => (ChannelValueProperty)x.GetValue(this);

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
