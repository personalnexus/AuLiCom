using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.CommandExecutor.Commands
{
    internal class ClearChannelValuesCommand: ICommand
    {
        public ClearChannelValuesCommand(IConnection connection)
        {
            _connection = connection;
        }

        private readonly IConnection _connection;

        public string Description => "CLEAR sets all channel values to zero";

        public bool TryExecute(string command)
        {
            bool result;
            if (command.Equals("Clear", StringComparison.OrdinalIgnoreCase))
            {
                _connection.SendUniverse(Universe.CreateEmpty());
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
