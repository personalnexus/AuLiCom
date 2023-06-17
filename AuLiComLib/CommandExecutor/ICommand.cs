using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.CommandExecutor
{
    public interface ICommand
    {
        string Description { get; }
        bool TryExecute(string command);
    }
}
