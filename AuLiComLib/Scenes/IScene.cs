using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Scenes
{
    public interface IScene: IReadOnlyUniverseProvider
    {
        string Name { get; }
    }
}
