using AuLiComLib.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.CommandExecutor
{
    public interface ICommandFixtures : ICommandColorFixtures
    {
        void SetFixtures(IEnumerable<IFixture> fixtures);
        bool TryGetChannelsByName(string channelName, out IEnumerable<int> channels);
    }
}
