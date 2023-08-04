using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.CommandExecutor
{
    public interface ICommandNamedSceneManager
    {
        void SetScene(string name, IReadOnlyUniverse universe);
        void ActivateSingleScene(string name);
    }
}
