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
                      IReadOnlyUniverseProvider[] steps)
        {
            Name = name;
            Kind = kind;
            Steps = steps;
        }

        public string Name { get; }
        public ChaserKind Kind { get; }
        public IReadOnlyUniverseProvider[] Steps { get; }
    }
}
