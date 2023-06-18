﻿using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.CommandExecutor.Commands
{
    internal class ListChannelValuesCommand : ICommand
    {
        public ListChannelValuesCommand(IConnection connection, ICommandConsole console)
        {
            _connection = connection;
            _console = console;
        }

        private readonly IConnection _connection;
        private readonly ICommandConsole _console;

        public string Description => "LIST all channel values that aren't zero";

        public bool TryExecute(string command)
        {
            bool result;
            bool nonZeroChannelWasShown = false;
            if (command.Equals("List", StringComparison.OrdinalIgnoreCase))
            {
                foreach (ChannelValue channelValue in _connection
                                                         .GetValues()
                                                         .Where(x => x.Value > 0))
                {
                    _console.WriteLine($"{channelValue.Channel}\t" +
                                       $"{channelValue.ValueAsPercentage}\t" +
                                       $"{new string('#', channelValue.ValueAsTenth)}");
                    nonZeroChannelWasShown = true;
                }
                if (!nonZeroChannelWasShown)
                {
                    _console.WriteLine("ALL\t0");
                }
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