using AuLiComLib.Common;
using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AuLiComLib.CommandExecutor.Commands
{
    internal class SetChannelValueCommand : ICommand
    {
        public SetChannelValueCommand(IConnection connection, ICommandWriteConsole console, ICommandFixtures fixtures)
        {
            _connection = connection;
            _console = console;
            _parser = new ChannelValueCommandParser(fixtures);
        }

        private readonly IConnection _connection;
        private readonly ICommandWriteConsole _console;
        private readonly ChannelValueCommandParser _parser;

        public string Description => "SET one or more channel values:\r\n" +
                                     "    1@50   set channel 1 to 50%\r\n" +
                                     "    1-3@60 set channels 1 through 3 to 60%\r\n" +
                                     "    1+4@70 set channels 1 and 4 to 70%\r\n" +
                                     "    1-5+7-10@80 set channels 1 through 5 and 7 through 10 to 80%\r\n" +
                                     "    Red@90 set channels on fixtures where the channel name contains 'red' (ignoring case) to 90%\r\n" +
                                     "    1-12@ set channels 1 through 12 to 100%";

        public bool TryExecute(string command)
        {
            bool result = _parser.TryParse(command, out var channelValues, out string errors);
            if (!result)
            {
                _console.WriteLine(errors);
            }
            else
            {
                _connection
                .CurrentUniverse
                .ToMutableUniverse()
                .SetValues(channelValues)
                .AsReadOnly()
                .SendTo(_connection);
            }
            return result;
        }
    }
}
