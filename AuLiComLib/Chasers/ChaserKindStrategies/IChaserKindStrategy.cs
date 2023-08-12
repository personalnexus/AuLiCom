using AuLiComLib.Protocols;

namespace AuLiComLib.Chasers.ChaserPlayStrategy
{
    internal interface IChaserKindStrategy
    {
        IEnumerable<IReadOnlyUniverse> GetSteps(CancellationToken cancellationToken);
    }
}
