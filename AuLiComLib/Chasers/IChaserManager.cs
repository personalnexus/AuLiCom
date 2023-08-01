using AuLiComLib.Common;
using AuLiComLib.Protocols;
using AuLiComLib.Scenes;

namespace AuLiComLib.Chasers
{
    public interface IChaserManager: IObservableEx<IChaserManager>
    {
        IReadOnlyDictionary<string, IChaser> ChasersByName { get; }
        IChaser SetChaser(string name, ChaserKind kind, IReadOnlyUniverseProvider[] steps);
    }
}