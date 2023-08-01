using AuLiComLib.Protocols;

namespace AuLiComLib.Chasers
{
    public interface IChaser
    {
        string Name { get; }
        ChaserKind Kind { get; }
        IReadOnlyUniverseProvider[] Steps { get; }
    }
}