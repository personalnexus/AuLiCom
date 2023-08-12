using AuLiComLib.Protocols;

namespace AuLiComLib.Chasers.ChaserPlayStrategy
{
    internal class ChaserKindStrategyRandom: IChaserKindStrategy
    {
        private readonly IReadOnlyUniverse[] _steps;

        public ChaserKindStrategyRandom(IReadOnlyUniverse[] steps) => _steps = steps;

        public IEnumerable<IReadOnlyUniverse> GetSteps(CancellationToken cancellationToken)
        {
            var random = new Random();
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    yield break;
                }
                else
                {
                    yield return _steps[random.Next(0, _steps.Length)];
                }
            }
        }
    }
}
