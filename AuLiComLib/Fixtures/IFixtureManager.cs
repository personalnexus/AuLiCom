using AuLiComLib.CommandExecutor;

namespace AuLiComLib.Fixtures
{
    public interface IFixtureManager: ICommandFixtures
    {
        bool TryAdd(IFixture fixture);
        T Get<T>(string name) where T : IFixture;
        IEnumerable<FixtureChannelInfo> GetFixtureChannelInfos();
        IEnumerable<FixtureInfo> GetFixtureInfos();
    }
}