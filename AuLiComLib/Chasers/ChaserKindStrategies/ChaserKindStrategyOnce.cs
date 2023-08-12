using AuLiComLib.Protocols;

namespace AuLiComLib.Chasers.ChaserPlayStrategy
{
    internal class ChaserKindStrategyOnce : IChaserKindStrategy
    {
        private readonly IReadOnlyUniverse[] _steps;

        public ChaserKindStrategyOnce(IReadOnlyUniverse[] steps) => _steps = steps;

        public IEnumerable<IReadOnlyUniverse> GetSteps(CancellationToken cancellationToken)
        {
            foreach (IReadOnlyUniverse step in _steps)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    yield break;
                }
                else
                {
                    yield return step;
                }
            }
        }
    }
}
