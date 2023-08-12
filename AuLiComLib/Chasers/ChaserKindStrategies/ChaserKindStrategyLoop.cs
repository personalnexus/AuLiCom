using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Chasers.ChaserPlayStrategy
{
    internal class ChaserKindStrategyLoop : IChaserKindStrategy
    {
        private readonly IReadOnlyUniverse[] _steps;

        public ChaserKindStrategyLoop(IReadOnlyUniverse[] steps) => _steps = steps;

        public IEnumerable<IReadOnlyUniverse> GetSteps(CancellationToken cancellationToken)
        {
            while (true)
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
}
