using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Scenes
{
    internal class Scene : Universe, IScene
    {
        public Scene(string name, IReadOnlyUniverse universe): base(universe)
        {
            Name = name;
        }

       public string Name { get; }
    }
}
