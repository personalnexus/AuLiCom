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
        public SetChannelValueCommand(IConnection connection)
        {
            _connection = connection;
        }

        private readonly IConnection _connection;

        public string Description => "SET one or more channel values:\r\n" +
                                     "    1@50   set channel 1 to 50%\r\n" +
                                     "    1-3@60 set channels 1 through 3 to 60%\r\n" +
                                     "    1+4@70 set channels 1 and 4 to 70%\r\n" +
                                     "    1-5+7-10@80 set channels 1 through 5 and 7 through 10 to 80%\r\n" +
                                     "    1-12@ set channels 1 through 12 to 100%";

        public bool TryExecute(string command)
        {
            bool result;

            string[] channelAndPercentage = command.Split('@', count: 2);

            if (channelAndPercentage.Length == 2)
            {
                var channels = new List<int>();
                foreach (string channelString in channelAndPercentage[0].Split('+', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    if (channelString.Contains('-'))
                    {
                        string[] channelStartAndEnd = channelString.Split('-', StringSplitOptions.TrimEntries);
                        if (channelStartAndEnd.Length == 2
                            && int.TryParse(channelStartAndEnd[0], out int channelStart)
                            && int.TryParse(channelStartAndEnd[1], out int channelEnd)
                            && channelEnd > channelStart)
                        {
                            channels.AddRange(Enumerable.Range(channelStart, channelEnd-channelStart));
                        }
                    }
                    else
                    {
                        if (int.TryParse(channelString, out int channel))
                        {
                            channels.Add(channel);
                        }
                        else
                        {
                            channels.Clear();
                            break;
                        }
                    }
                }
                

                if (channels.Any())
                {
                    int percentage;
                    if (string.IsNullOrWhiteSpace(channelAndPercentage[1]))
                    {
                        percentage = 100;
                        result = true;
                    }
                    else
                    {
                        result = int.TryParse(channelAndPercentage[1], out percentage);
                    }

                    if (result)
                    {
                        IEnumerable<ChannelValue> channelValues = channels
                            .Select(x => ChannelValue.FromPercentage(x, percentage));
                        _connection.SetValues(channelValues);
                    }
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }

            return result;
        }
    }
}
