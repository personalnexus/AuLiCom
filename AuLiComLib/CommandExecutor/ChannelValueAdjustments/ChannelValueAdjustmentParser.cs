using AuLiComLib.Colors;
using AuLiComLib.Common;
using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AuLiComLib.CommandExecutor.ChannelValueAdjustments
{
    public class ChannelValueAdjustmentParser
    {
        public ChannelValueAdjustmentParser(ICommandColors colors,
                                            ICommandFixtures fixtures,
                                            IChannelValueAdjustmentProvider previousAdjustmentProvider)
        {
            _colors = colors;
            _fixtures = fixtures;
            _previousAdjustmentProvider = previousAdjustmentProvider;
        }

        private readonly ICommandColors _colors;
        private readonly ICommandFixtures _fixtures;
        private readonly IChannelValueAdjustmentProvider _previousAdjustmentProvider;

        private const string RepeatPreviousIndicator = "*";
        private const char PercentageIndicator = '@';
        private const char SectionIndicator = '+';
        private const char RangeIndicator = '-';

        private const char AddAdjustmentIndicator = '+';
        private const char SubtractAdjustmentIndicator = '-';
        private const char MultiplyAdjustmentIndicator = '*';
        private const char DivideAdjustmentIndicator = '/';

        public bool TryParse(string command, out ChannelValueAdjustment channelValueAdjustment, out string error)
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
                if (command.Trim() == RepeatPreviousIndicator)
                {
                    result = true;
                    channelValueAdjustment = _previousAdjustmentProvider.PreviousAdjustment;
                    error = "";
                }
                else
                {
                    if (!command.TrySplitInTwo(PercentageIndicator, out string channelsString, out string adjustmentStrategyString))
                    {
                        throw new ChannelValueAdjustmentParserException($"Command has to contain exactly one '{PercentageIndicator}'.");
                    }
                    ParseChannels(channelsString.Trim(), out IEnumerable<int> channelNumbers);
                    ParseAdjustmentStrategy(adjustmentStrategyString.Trim(), out IChannelValueAdjustmentStrategy adjustmentStrategy);

                    result = true;
                    channelValueAdjustment = new ChannelValueAdjustment(channelNumbers, adjustmentStrategy);
                    error = "";
                }
            }
            catch (ChannelValueAdjustmentParserException exception)
            {
                result = false;
                channelValueAdjustment = default;
                error = exception.Message;
            }
            return result;
        }

        private void ParseChannels(string channelsString, out IEnumerable<int> channels)
        {
            if (channelsString == RepeatPreviousIndicator)
            {
                channels = _previousAdjustmentProvider.PreviousAdjustment.Channels;
            }
            else
            {
                var channelsList = new SortedSet<int>();
                string[] channelSections = channelsString.Split(SectionIndicator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (channelSections.Length == 0)
                {
                    throw new ChannelValueAdjustmentParserException("Command has to contain at least one channel.");
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
                        throw new ChannelValueAdjustmentParserException($"There are no channels named '{channelSection}'.");
                    }
                }
                channels = channelsList;
            }
        }

        private static void ParseChannelRange(string channelGroup, out int channelStart, out int channelEnd)
        {
            string[] channelRangeStartAndEnd = channelGroup.Split(RangeIndicator, StringSplitOptions.TrimEntries);
            if (channelRangeStartAndEnd.Length != 2)
            {
                throw new ChannelValueAdjustmentParserException($"A channel range must contain only one '{RangeIndicator}'.");
            }
            else if (!int.TryParse(channelRangeStartAndEnd[0], out channelStart))
            {
                throw new ChannelValueAdjustmentParserException($"A channel range must start with a number, not '{channelRangeStartAndEnd[0]}'.");
            }
            else if (!int.TryParse(channelRangeStartAndEnd[1], out channelEnd))
            {
                throw new ChannelValueAdjustmentParserException($"A channel range must end with a number, not '{channelRangeStartAndEnd[1]}'.");
            }
            else if (channelEnd <= channelStart)
            {
                throw new ChannelValueAdjustmentParserException($"A channel range must start with a lower number and end with a higher, not '{channelRangeStartAndEnd}'.");
            }
        }

        private void ParseAdjustmentStrategy(string adjustmentString, out IChannelValueAdjustmentStrategy adjustmentStrategy)
        {
            adjustmentStrategy = adjustmentString == ""
                ? throw new ChannelValueAdjustmentParserException("The adjustment cannot be empty.")
                : adjustmentString == PercentageIndicator.ToString()
                    ? new ChannelValueAdjustmentStrategySetPercentage(100)
                    : adjustmentString[0] switch
                        {
                            AddAdjustmentIndicator => new ChannelValueAdjustmentStrategyAddPercentage(ParsePercentage(adjustmentString[1..])),
                            SubtractAdjustmentIndicator => new ChannelValueAdjustmentStrategyAddPercentage(-ParsePercentage(adjustmentString[1..])),
                            MultiplyAdjustmentIndicator => new ChannelValueAdjustmentStrategyMultiply(ParseFactor(adjustmentString[1..])),
                            DivideAdjustmentIndicator => new ChannelValueAdjustmentStrategyMultiply(1.0 / ParseFactor(adjustmentString[1..])),
                            _ => ParsePercentageOrColor(adjustmentString)
                        };
        }

        private IChannelValueAdjustmentStrategy ParsePercentageOrColor(string adjustmentString) =>
            _colors.TryGetColorByName(adjustmentString, out IColor color)
                ? new ChannelValueAdjustmentStrategyColor(color, _fixtures)
                : new ChannelValueAdjustmentStrategySetPercentage(ParsePercentage(adjustmentString));

        private static int ParsePercentage(string percentageString)
        {
            if (!int.TryParse(percentageString, out int percentage))
            {
                throw new ChannelValueAdjustmentParserException($"Percentage has to be an integer, not '{percentageString}'");
            }
            else if (percentage is < 0 or > 100)
            {
                throw new ChannelValueAdjustmentParserException($"Percentage has to be between 0 and 100, not '{percentageString}'");
            }
            return percentage;
        }

        private static double ParseFactor(string factorString)
        {
            if (!double.TryParse(factorString, out double factor))
            {
                throw new ChannelValueAdjustmentParserException($"Factor has to be a decimal number, not '{factorString}'");
            }
            else if (factor is <= 0 or >= 100)
            {
                throw new ChannelValueAdjustmentParserException($"Factor has to be greater than 0 and less than 100, not '{factorString}'");
            }
            return factor;
        }
    }
}
