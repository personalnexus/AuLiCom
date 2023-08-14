using AuLiComLib.Protocols;

namespace AuLiComLib.Chasers
{
    public interface IChaser
    {
        string Name { get; }
        ChaserType Kind { get; }
        TimeSpan StepDuration { get; }
        string[] StepNames { get; }

        Task StartPlaying(IConnection connection,
                          IReadOnlyUniverseProvider provider);
        void StopPlaying();
    }
}