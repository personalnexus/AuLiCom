using AuLiComLib.Common;
using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AuLiComLib.CommandExecutor
{
    public class ChannelValueCommandParser
    {
        public ChannelValueCommandParser(ICommandFixtures fixtures)
        {
            _fixtures = fixtures;
        }

        private readonly ICommandFixtures _fixtures;

        private const char PercentageIndicator = '@';
        private const char SectionIndicator = '+';
        private const char RangeIndicator = '-';

        private class ChannelValueCommandParserException : ArgumentException
        {
            public ChannelValueCommandParserException(string message) : base(message)
            {
            }
        }

        public bool TryParse(string command, out IEnumerable<ChannelValue> channelValues, out string error)
        {
            // Syntax:
            // PercentageIndicator separates one or more channel sections and the percentage
            // SectionIndicator separates individual channel sections
            // A channel section can be:
            //   a range of channel numbers where start and end are separated by RangeIndicator
            //   a channel number
            //   a channel name indicated by a non-numeric value

            bool result;
            try
            {
                if (!command.TrySplitIn(2, PercentageIndicator, out string[] channelsAndPercentage))
                {
                    throw new ChannelValueCommandParserException($"Command has to contain exactly one '{PercentageIndicator}'.");
                }
                ParseChannels(channelsAndPercentage[0].Trim(), out IEnumerable<int> channelNumbers);
                ParsePercentage(channelsAndPercentage[1].Trim(), out int percentage);

                result = true;
                channelValues = ChannelValue.From(channelNumbers, at: percentage);
                error = "";
            }
            catch (ChannelValueCommandParserException exception)
            {
                result = false;
                channelValues = Enumerable.Empty<ChannelValue>();
                error = exception.Message;
            }
            return result;
        }

        private void ParseChannels(string channelsString, out IEnumerable<int> channels)
        {
            var channelsList = new SortedSet<int>();
            string[] channelSections = channelsString.Split(SectionIndicator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (channelSections.Length == 0)
            {
                throw new ChannelValueCommandParserException("Command has to contain at least one channel.");
            }
            
            foreach (string channelSection in channelSections)
            {
                if (channelSection.Contains(RangeIndicator))
                {
                    ParseChannelRange(channelSection, out int channelStart, out int channelEnd);
                    Enumerable
                    .Range(channelStart, channelEnd - channelStart + 1) // end is inclusive
                    .AddTo(channelsList);
                }
                else if (int.TryParse(channelSection, out int channel))
                {
                    channelsList.Add(channel);
                }
                else if (_fixtures.TryGetChannelsByName(channelSection, out IEnumerable<int> namedChannels))
                {
                    namedChannels.AddTo(channelsList);
                }
                else
                {
                    throw new ChannelValueCommandParserException($"There are no channels named '{channelSection}'.");
                }
            }
            channels = channelsList;
        }

        private static void ParseChannelRange(string channelGroup, out int channelStart, out int channelEnd)
        {
            string[] channelRangeStartAndEnd = channelGroup.Split(RangeIndicator, StringSplitOptions.TrimEntries);
            if (channelRangeStartAndEnd.Length != 2)
            {
                throw new ChannelValueCommandParserException($"A channel range must contain only one '{RangeIndicator}'.");
            }
            else if (!int.TryParse(channelRangeStartAndEnd[0], out channelStart))
            {
                throw new ChannelValueCommandParserException($"A channel range must start with a number, not '{channelRangeStartAndEnd[0]}'.");
            }
            else if (!int.TryParse(channelRangeStartAndEnd[1], out channelEnd))
            {
                throw new ChannelValueCommandParserException($"A channel range must end with a number, not '{channelRangeStartAndEnd[1]}'.");
            }
            else if (channelEnd <= channelStart)
            {
                throw new ChannelValueCommandParserException($"A channel range must start with a lower number and end with a higher, not '{channelRangeStartAndEnd}'.");
            }
        }

        private static void ParsePercentage(string percentageString, out int percentage)
        {
            if (string.IsNullOrEmpty(percentageString))
            {
                percentage = 100;
            }
            else if (!int.TryParse(percentageString, out percentage))
            {
                throw new ChannelValueCommandParserException($"Percentage has to be an integer, not '{percentageString}'");
            }
            else if (percentage is < 0 or > 100)
            {
                throw new ChannelValueCommandParserException($"Percentage has to be between 0 and 100, not '{percentageString}'");
            }
        }
    }
}
