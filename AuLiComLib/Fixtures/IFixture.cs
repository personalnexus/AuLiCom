using AuLiComLib.CommandExecutor;
using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Fixtures
{
    public interface IFixture
    {
        string Name { get; }

        int StartChannel { get; }
        int ChannelCount { get; }

        IEnumerable<FixtureChannelInfo> GetFixtureChannelInfos();
        FixtureInfo GetFixtureInfo();

        bool TryGetColorChannelValueProperties(int channelOffset, out ICommandColorChannelValueProperties colorProperties);
    }
}
