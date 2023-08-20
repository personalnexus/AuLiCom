using AuLiComLib.Colors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.CommandExecutor
{
    public interface ICommandColors
    {
        bool TryGetColorByName(string name, out IColor color);
    }
}
