using AuLiComLib.Protocols.Dmx;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.CommandExecutor.Commands
{
    internal class ClearCommand: ICommand
    {
        public ClearCommand(IDmxConnection connection, ICommandConsole console)
        {
            _connection = connection;
            _console = console;
        }

        private readonly IDmxConnection _connection;
        private readonly ICommandConsole _console;

        public string Description => "CLEAR sets all values to zero";

        public bool TryExecute(string command)
        {
            bool result;
            if (command.Equals("Clear", StringComparison.OrdinalIgnoreCase))
            {
                _connection.SetValuesToZero();
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
    }
}
