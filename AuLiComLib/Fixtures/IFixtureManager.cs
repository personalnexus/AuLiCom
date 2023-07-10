using AuLiComLib.CommandExecutor;
using AuLiComLib.Common;

namespace AuLiComLib.Fixtures
{
    public interface IFixtureManager: ICommandFixtures, IVersioned
    {
        bool TryAdd(IFixture fixture);
        T Get<T>(string name) where T : IFixture;
        IEnumerable<FixtureChannelInfo> GetFixtureChannelInfos();
        IEnumerable<FixtureInfo> GetFixtureInfos();
        int GetChannelCountTotal();
    }
}