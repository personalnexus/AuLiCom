namespace AuLiComLib.Fixtures
{
    public interface IFixtureDefinition
    {
        string Kind { get; }
        string Mode { get; }
        string Name { get; }
        int Channel { get; }
    }
}