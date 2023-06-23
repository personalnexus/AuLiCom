using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Scenes
{
    internal class Scene : IScene
    {
        public Scene(string name, IReadOnlyUniverse universe)
        {
            Universe = universe;
            Name = name;
        }

        public IReadOnlyUniverse Universe { get; }

        public string Name { get; }
    }
}
