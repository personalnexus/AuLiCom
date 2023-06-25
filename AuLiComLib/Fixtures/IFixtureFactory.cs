namespace AuLiComLib.Fixtures
{
    public interface IFixtureFactory
    {
        IFixture CreateFromFixtureInfo(FixtureInfo fixtureInfo);
        IEnumerable<IFixture> CreateFromJson(string fileContents);
        IEnumerable<string> GetFixtureTypes();
    }
}