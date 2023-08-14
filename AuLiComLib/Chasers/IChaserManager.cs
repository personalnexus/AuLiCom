using AuLiComLib.Common;
using AuLiComLib.Protocols;
using AuLiComLib.Scenes;

namespace AuLiComLib.Chasers
{
    public interface IChaserManager: IObservableEx<IChaserManager>
    {
        IReadOnlyDictionary<string, IChaser> ChasersByName { get; }
        IChaser SetChaser(string name, ChaserType kind, TimeSpan stepDuration, string[] stepNames);

        Task StartPlaying(string name, IReadOnlyUniverseProvider universeProvider);
        void StopPlaying(string name);
    }
}