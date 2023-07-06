using AuLiComLib.CommandExecutor.ChannelValueCommands;
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
        public SetChannelValueCommand(IConnection connection,
                                      ICommandWriteConsole console,
                                      ICommandFixtures fixtures)
        {
            _connection = connection;
            _console = console;
            _parser = new ChannelValueAdjustmentParser(fixtures);
        }

        private readonly IConnection _connection;
        private readonly ICommandWriteConsole _console;
        private readonly ChannelValueAdjustmentParser _parser;

        public string Description => "SET one or more channel values:\r\n" +
                                     "    1@+10  increase channel 1 by 10 percentage points\r\n" +
                                     "    2+3@-15  lower channels 2 and 3 by 15 percentage points\r\n" +
                                     "    4-7@*3  triple the percentage values of channels 4 through 7\r\n" +
                                     "    8-9+10@/2  halve the percentage values of channels 8 through 9 and 10\r\n" +
                                     "    11@50   set channel 11 to 50%\r\n" +
                                     "    12-15@60 set channels 11 through 15 to 60%\r\n" +
                                     "    16+19@70 set channels 16 and 19 to 70%\r\n" +
                                     "    20-25+27-30@80 set channels 20 through 25 and 27 through 30 to 80%\r\n" +
                                     "    Red@90 set channels on fixtures where channel name, fixture name or alias contains 'red' (ignoring case) to 90%\r\n" +
                                     "    31-33@ set channels 31 through 33 to 100%";

        public bool TryExecute(string command)
        {
            bool result = _parser.TryParse(command, out ChannelValueAdjustment adjustment, out string errors);
            if (!result)
            {
                _console.WriteLine(errors);
            }
            else
            {
                adjustment
                .ApplyTo(_connection.CurrentUniverse)
                .AsReadOnly()
                .SendTo(_connection);
            }
            return result;
        }
    }
}
