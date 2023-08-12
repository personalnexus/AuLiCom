using AuLiComLib.Chasers.ChaserPlayStrategy;
using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Chasers
{
    public class Chaser : IChaser
    {
        public Chaser(string name,
                      ChaserKind kind,
                      TimeSpan stepDuration,
                      string[] stepNames)
        {
            Name = name;
            Kind = kind;
            StepDuration = stepDuration;
            StepNames = stepNames;
        }

        public string Name { get; }
        public ChaserKind Kind { get; }
        public string[] StepNames { get; }
        public TimeSpan StepDuration { get; }

        private CancellationTokenSource _cancellationTokenSourceForCurrentPlay;

        public async Task StartPlaying(IConnection connection,
                                       IReadOnlyUniverseProvider provider)
        {
            IReadOnlyUniverse[] steps = StepNames.Select(provider.GetUniverseByName).ToArray();

            IChaserKindStrategy strategy = Kind switch
            {
                ChaserKind.Once => new ChaserKindStrategyOnce(steps),
                ChaserKind.Loop => new ChaserKindStrategyLoop(steps),
                ChaserKind.BackAndForth => new ChaserKindStrategyBackAndForth(steps),
                ChaserKind.Random => new ChaserKindStrategyRandom(steps),
                _ => throw new ArgumentOutOfRangeException(nameof(Kind)),
            };

            // Start only when all inputs have been validated
            StopPlaying();
            _cancellationTokenSourceForCurrentPlay = new CancellationTokenSource();

            foreach (var step in strategy.GetSteps(_cancellationTokenSourceForCurrentPlay.Token))
            {
                connection.SendUniverse(step);
                await Task.Delay(StepDuration, _cancellationTokenSourceForCurrentPlay.Token);
            }

            // Reset after playing
            connection.SendUniverse(Universe.CreateEmptyReadOnly());
        }

        public void StopPlaying() => _cancellationTokenSourceForCurrentPlay?.Cancel();
    }
}
