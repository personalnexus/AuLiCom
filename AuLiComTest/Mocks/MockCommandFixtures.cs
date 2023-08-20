using AuLiComLib.CommandExecutor;
using AuLiComLib.Fixtures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComTest.Mocks
{
    internal class MockCommandFixtures : ICommandFixtures, IEnumerable
    {
        private readonly Dictionary<string, int> _channelsByName = new();
        private readonly HashSet<MockColorChannelValueProperties> _colorPropertiesByChannel = new();

        public void Add(string name, int channel) => _channelsByName.Add(name, channel);

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)_channelsByName).GetEnumerator();
        }

        public void SetFixtures(IEnumerable<IFixture> fixtures)
        {
            throw new NotImplementedException();
        }

        public bool TryGetChannelsByName(string channelName, out IEnumerable<int> channels)
        {
            List<int> channelsList = 
                _channelsByName
                .Where(x => x.Key.Contains(channelName, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Value)
                .ToList();
            channels = channelsList;
            return channelsList.Any();
        }

        public bool TryGetColorChannelValuePropertiesByChannel(int channel, out ICommandColorChannelValueProperties colorProperties)
        {
            colorProperties = _colorPropertiesByChannel.FirstOrDefault(x => x.Red.Channel == channel || x.Green.Channel == channel || x.Blue.Channel == channel);
            return colorProperties != null;
        }

        public void SetColorChannelValuePropertiesByChannel(int channel) => _colorPropertiesByChannel.Add(new MockColorChannelValueProperties(channel));
    }
}
