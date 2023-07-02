using AuLiComLib.CommandExecutor;
using AuLiComLib.Fixtures.Kinds;
using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Fixtures
{
    /// <summary>
    /// Manages the fixtures which in turn have one or more channels.
    /// </summary>
    public class FixtureManager : IFixtureManager
    {
        public FixtureManager(params IFixture[] fixtures)
        {
            _fixturesByName = new();
            _fixturesByChannel = new();
            HashSet<string> duplicateNames = fixtures
                .Where(x => !TryAdd(x))
                .Select(x => x.Name)
                .ToHashSet();
            if (duplicateNames.Count > 0)
            {
                throw new ArgumentException($"{duplicateNames.Count} duplicate names: \r\n{string.Join(", ", duplicateNames)}");
            }
        }

        private readonly Dictionary<string, IFixture> _fixturesByName;
        private readonly Dictionary<int, IFixture> _fixturesByChannel;

        public bool TryAdd(IFixture fixture)
        {
            bool result = _fixturesByName.TryAdd(fixture.Name, fixture);
            if (result)
            {
                try
                {
                    foreach (int channel in Enumerable.Range(fixture.StartChannel, fixture.ChannelCount))
                    {
                        if (_fixturesByChannel.TryGetValue(channel, out IFixture? existingFixture))
                        {
                            throw new ArgumentException($"Channel {channel} is already in use by {existingFixture.Name} and cannot be used by {fixture.Name} (Start: {fixture.StartChannel}, Count: {fixture.ChannelCount})");
                        }
                        else
                        {
                            _fixturesByChannel.Add(channel, fixture);
                        }
                    }
                }
                catch
                {
                    _fixturesByName.Remove(fixture.Name);
                    throw;
                }
            }
            return result;
        }

        public T Get<T>(string name) where T : IFixture => (T)_fixturesByName[name];

        public IEnumerable<FixtureChannelInfo> GetFixtureChannelInfos() =>
            _fixturesByName
            .Values
            .SelectMany(x => x.GetFixtureChannelInfos());

        public IEnumerable<FixtureInfo> GetFixtureInfos() =>
            _fixturesByName
            .Values
            .Select(x => x.GetFixtureInfo());

        /// <summary>
        /// Return all channels where the channel name, fixture name or alias contains the given nameOrAliasSubstring
        /// </summary>
        public bool TryGetChannelsByName(string nameOrAliasSubstring, out IEnumerable<int> channels)
        {
            List<int> channelsList =
             GetFixtureChannelInfos()
            .Where(x => x.ChannelName.Contains(nameOrAliasSubstring, StringComparison.OrdinalIgnoreCase)
                        || x.FixtureName.Contains(nameOrAliasSubstring, StringComparison.OrdinalIgnoreCase)
                        || x.FixtureAlias?.Contains(nameOrAliasSubstring, StringComparison.OrdinalIgnoreCase) == true)
            .Select(x => x.StartChannel)
            .ToList();
            channels = channelsList;
            return channelsList.Any();
        }
    }
}
