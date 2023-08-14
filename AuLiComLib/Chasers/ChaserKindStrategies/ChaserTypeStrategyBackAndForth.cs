using AuLiComLib.Protocols;

namespace AuLiComLib.Chasers.ChaserPlayStrategy
{
    internal class ChaserTypeStrategyBackAndForth: IChaserKindStrategy
    {
        private readonly IReadOnlyUniverse[] _steps;

        public ChaserTypeStrategyBackAndForth(IReadOnlyUniverse[] steps) => _steps = steps;

        public IEnumerable<IReadOnlyUniverse> GetSteps(CancellationToken cancellationToken)
        {
            IReadOnlyUniverse[] stepsBackAndForth = _steps.Concat(_steps.Reverse()).ToArray();
            while (true)
            {
                foreach (IReadOnlyUniverse step in stepsBackAndForth)
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
}
