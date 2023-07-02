using AuLiComLib.CommandExecutor;
using AuLiComLib.Protocols;
using AuLiComTest.Mocks;
using FluentAssertions;
using FluentAssertions.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComTest
{
    [TestClass]
    public class ChannelValueCommandParserTest
    {
        [TestClass]
        public class TryParse
        {
            // Successes

            [TestMethod]
            public void EverythingWithWhitespace_AllChannelsAtPercentage() => ShouldBeSuccess(
                " 1 + 4        -  5 + BLue @ 99 ",
                ChannelValue.FromPercentage(1, 99),
                ChannelValue.FromPercentage(3, 99),
                ChannelValue.FromPercentage(4, 99),
                ChannelValue.FromPercentage(5, 99),
                ChannelValue.FromPercentage(6, 99));

            [TestMethod]
            public void DuplicateChannels_EveryChannelOnlyOnceAtPercentage() => ShouldBeSuccess(
                " 1 + 4 + 3-5 + 1-4 @ 95 ",
                ChannelValue.FromPercentage(1, 95),
                ChannelValue.FromPercentage(2, 95),
                ChannelValue.FromPercentage(3, 95),
                ChannelValue.FromPercentage(4, 95),
                ChannelValue.FromPercentage(5, 95));

            [TestMethod]
            public void OneChannelAndNoPercentage_ChannelAt100Percent() => ShouldBeSuccess(
                "12@",
                ChannelValue.FromPercentage(12, 100));

            [TestMethod]
            public void OneChannelAndPercentage_ChannelAtPercentage() => ShouldBeSuccess(
                "14@80",
                ChannelValue.FromPercentage(14, 80));

            [TestMethod]
            public void TwoChannelsAndPercentage_BothChannelsAtPercentage() => ShouldBeSuccess(
                "14+11@83",
                ChannelValue.FromPercentage(11, 83),
                ChannelValue.FromPercentage(14, 83));

            [TestMethod]
            public void ChannelRangeAndPercentage_EntireRangeAtPercentage() => ShouldBeSuccess(
                "11-14@86",
                ChannelValue.FromPercentage(11, 86),
                ChannelValue.FromPercentage(12, 86),
                ChannelValue.FromPercentage(13, 86),
                ChannelValue.FromPercentage(14, 86));

            [TestMethod]
            public void TwoChannelRangesAndPercentage_BothRangesAtPercentage() => ShouldBeSuccess(
                "11-12+14-15@86",
                ChannelValue.FromPercentage(11, 86),
                ChannelValue.FromPercentage(12, 86),
                ChannelValue.FromPercentage(14, 86),
                ChannelValue.FromPercentage(15, 86));

            [TestMethod]
            public void ChannelNameAndPercentage_BothChannelsWithThisNameAtPercentage() => ShouldBeSuccess(
                "red@89",
                ChannelValue.FromPercentage(1, 89),
                ChannelValue.FromPercentage(4, 89));

            // Errors

            [TestMethod]
            public void PercentageIsNotANumber_Error() => ShouldBeError(
                "1@humpelpumpel",
                "Percentage has to be an integer, not 'humpelpumpel'");

            [TestMethod]
            public void PercentageIsLessThan0_Error() => ShouldBeError(
                "1@-44",
                "Percentage has to be between 0 and 100, not '-44'");

            [TestMethod]
            public void PercentageIsGreaterThan100_Error() => ShouldBeError(
                "1@155",
                "Percentage has to be between 0 and 100, not '155'");

            [TestMethod]
            public void ChannelNamesDoNoExist_Error() => ShouldBeError(
               "NotAChannel@77",
               "There are no channels named 'NotAChannel'.");

            [TestMethod]
            public void ChannelsAreMissing_Error() => ShouldBeError(
                "@77",
                "Command has to contain at least one channel.");

            [TestMethod]
            public void ChannelRangeContainsTwoRangeIndicators_Error() => ShouldBeError(
                "1-2-4@78",
                "A channel range must contain only one '-'.");

            [TestMethod]
            public void ChannelRangeStartsWithALetter_Error() => ShouldBeError(
               "alpha-45@79",
               "A channel range must start with a number, not 'alpha'.");

            [TestMethod]
            public void ChannelRangeEndsWithALetter_Error() => ShouldBeError(
               "12-omega@81",
               "A channel range must end with a number, not 'omega'.");

            [TestMethod]
            public void TwoPercentageIndicators_Error() => ShouldBeError(
               "7@@82",
               "Command has to contain exactly one '@'.");

            // Helper methods

            private static void ShouldBeSuccess(string command, params ChannelValue[] expectedValues)
            {
                // Arrange
                ChannelValueCommandParser parser = Arrange();

                // Act
                var parseResult = parser.TryParse(command, out IEnumerable<ChannelValue> values, out string error);

                // Assert
                using (new AssertionScope())
                {
                    parseResult.Should().BeTrue();
                    values.Should().BeEquivalentTo(expectedValues);
                    error.Should().BeNullOrEmpty();
                }
            }

            private static void ShouldBeError(string command, string expectedError)
            {
                // Arrange
                ChannelValueCommandParser parser = Arrange();

                // Act
                var parseResult = parser.TryParse(command, out IEnumerable<ChannelValue> values, out string error);

                // Assert
                using (new AssertionScope())
                {
                    parseResult.Should().BeFalse();
                    values.Should().BeEmpty();
                    error.Should().Be(expectedError);
                }
            }

            private static ChannelValueCommandParser Arrange()
            {
                var fixtures = new MockCommandFixtures
                {
                    { "Red1", 1 },
                    { "Green1", 2 },
                    { "Blue1", 3 },
                    { "Red2", 4 },
                    { "Green2", 5 },
                    { "Blue2", 6 },
                };
                var result = new ChannelValueCommandParser(fixtures);
                return result;
            }
        }
    }
}
