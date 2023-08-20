using AuLiComLib.Colors;
using AuLiComLib.CommandExecutor.ChannelValueAdjustments;
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
    public class ChannelValueAdjustmentParserTest
    {
        [TestClass]
        public class TryParse
        {
            // Successes

            [TestMethod]
            public void Add10PercentToChannel1_Channel1Is21PercentAndRestIsUnchanged() => ShouldBeSuccessWithBaseUniverse(
                "1@+10",
                channel1: 21);

            [TestMethod]
            public void Subtract15PercentFromChannel1Through2_Channel1IsFlooredAt0And2IsAdjustedAnd3IsUnchanged() => ShouldBeSuccessWithBaseUniverse(
                "1-2@-15",
                channel1: 0,
                channel2: 7);

            [TestMethod]
            public void MultiplyChannels2And3With4_Channel2IsDoubledAndChannel3IsCappedAt100Percent() => ShouldBeSuccessWithBaseUniverse(
                "2-3@*4",
                channel2: 88,
                channel3: 100);

            [TestMethod]
            public void DivideChannels1And3By20_Channel1IsFlooredAt0Channel3IsRoundedTo2() => ShouldBeSuccessWithBaseUniverse(
                "1+3@/16",
                channel1: 0,
                channel3: 2);

            [TestMethod]
            public void EverythingWithWhitespace_AllChannelsAtPercentage() => ShouldBeSuccessWithEmptyUniverse(
                " 1 + 4        -  5 + BLue @ 99 ",
                ChannelValue.FromPercentage(1, 99),
                ChannelValue.FromPercentage(3, 99),
                ChannelValue.FromPercentage(4, 99),
                ChannelValue.FromPercentage(5, 99),
                ChannelValue.FromPercentage(6, 99));

            [TestMethod]
            public void DuplicateChannels_EveryChannelOnlyOnceAtPercentage() => ShouldBeSuccessWithEmptyUniverse(
                " 1 + 4 + 3-5 + 1-4 @ 95 ",
                ChannelValue.FromPercentage(1, 95),
                ChannelValue.FromPercentage(2, 95),
                ChannelValue.FromPercentage(3, 95),
                ChannelValue.FromPercentage(4, 95),
                ChannelValue.FromPercentage(5, 95));

            [TestMethod]
            public void OneChannelAndAnAtSignInsteadOfAPercentage_ChannelAt100Percent() => ShouldBeSuccessWithEmptyUniverse(
                "12@@",
                ChannelValue.FromPercentage(12, 100));

            [TestMethod]
            public void OneChannelAndPercentage_ChannelAtPercentage() => ShouldBeSuccessWithEmptyUniverse(
                "14@80",
                ChannelValue.FromPercentage(14, 80));

            [TestMethod]
            public void TwoChannelsAndPercentage_BothChannelsAtPercentage() => ShouldBeSuccessWithEmptyUniverse(
                "14+11@83",
                ChannelValue.FromPercentage(11, 83),
                ChannelValue.FromPercentage(14, 83));

            [TestMethod]
            public void ChannelRangeAndPercentage_EntireRangeAtPercentage() => ShouldBeSuccessWithEmptyUniverse(
                "11-14@86",
                ChannelValue.FromPercentage(11, 86),
                ChannelValue.FromPercentage(12, 86),
                ChannelValue.FromPercentage(13, 86),
                ChannelValue.FromPercentage(14, 86));

            [TestMethod]
            public void TwoChannelRangesAndPercentage_BothRangesAtPercentage() => ShouldBeSuccessWithEmptyUniverse(
                "11-12+14-15@86",
                ChannelValue.FromPercentage(11, 86),
                ChannelValue.FromPercentage(12, 86),
                ChannelValue.FromPercentage(14, 86),
                ChannelValue.FromPercentage(15, 86));

            [TestMethod]
            public void ChannelNameAndPercentage_BothChannelsWithThisNameAtPercentage() => ShouldBeSuccessWithEmptyUniverse(
                "red@89",
                ChannelValue.FromPercentage(1, 89),
                ChannelValue.FromPercentage(4, 89));

            [TestMethod]
            public void FirstColorChannelAndDefinedColorName_ThreeChannelsAreSet() => ShouldBeSuccessWithEmptyUniverse(
                $"{ColorChannel}@{ColorName}",
                ChannelValue.FromByte(240, 255),
                ChannelValue.FromByte(241, 127),
                ChannelValue.FromByte(242, 1));

            [TestMethod]
            public void SecondColorChannelAndDefinedColorName_ThreeChannelsAreSet() => ShouldBeSuccessWithEmptyUniverse(
                $"{ColorChannel + 1}@{ColorName}",
                ChannelValue.FromByte(240, 255),
                ChannelValue.FromByte(241, 127),
                ChannelValue.FromByte(242, 1));

            [TestMethod]
            public void NameOfColorFixtureAndDefinedColorName_ThreeChannelsAreSet() => ShouldBeSuccessWithEmptyUniverse(
                $"{ColorFixture}@{ColorName}",
                ChannelValue.FromByte(240, 255),
                ChannelValue.FromByte(241, 127),
                ChannelValue.FromByte(242, 1));

            // Errors

            [TestMethod]
            public void OneChannelAndNoPercentage_Error() => ShouldBeError(
                "12@", 
                "The adjustment cannot be empty.");

            [TestMethod]
            public void PercentageIsNotANumber_Error() => ShouldBeError(
                "1@humpelpumpel",
                "Percentage has to be an integer, not 'humpelpumpel'");

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
               "Percentage has to be an integer, not '@82'");

            [TestMethod]
            public void NoPercentageIndicator_Error() => ShouldBeError(
               "83",
               "Command has to contain exactly one '@'.");

            [TestMethod]
            public void UndefinedColor_Error() => ShouldBeError(
               $"{ColorChannel}@AnotherColor",
               "Percentage has to be an integer, not 'AnotherColor'");

            // Helper methods

            private const string ColorFixture = "RGB";
            private const int ColorChannel = 240;
            private const string ColorName = "orange";

            private static void ShouldBeSuccessWithEmptyUniverse(string command, params ChannelValue[] expectedValues) =>
                ShouldBeSuccess(command, Universe.CreateEmptyReadOnly(), expectedValues);

            private static void ShouldBeSuccessWithBaseUniverse(string command, 
                                                                int channel1 = 11,
                                                                int channel2 = 22,
                                                                int channel3 = 33) =>
                ShouldBeSuccess(command, 
                                BaseUniverse, 
                                ChannelValue.FromPercentage(1, channel1),
                                ChannelValue.FromPercentage(2, channel2),
                                ChannelValue.FromPercentage(3, channel3));

            private static void ShouldBeSuccess(string command, IReadOnlyUniverse universe, params ChannelValue[] expectedValues)
            {
                // Arrange
                ChannelValueAdjustmentParser parser = Arrange();
                byte[] expectedBytes = Universe.CreateEmpty().SetValues(expectedValues).AsReadOnly().GetValuesCopy();

                // Act
                bool parseResult = parser.TryParse(command, out ChannelValueAdjustment adjustment, out string error);
                byte[] actualBytes = adjustment.ApplyTo(universe).AsReadOnly().GetValuesCopy();

                // Assert
                using (new AssertionScope())
                {
                    parseResult.Should().BeTrue();
                    actualBytes.Should().BeEquivalentTo(expectedBytes);
                    error.Should().BeNullOrEmpty();
                }
            }

            private static void ShouldBeError(string command, string expectedError)
            {
                // Arrange
                ChannelValueAdjustmentParser parser = Arrange();

                // Act
                bool parseResult = parser.TryParse(command, out ChannelValueAdjustment adjustment, out string error);

                // Assert
                using (new AssertionScope())
                {
                    parseResult.Should().BeFalse();
                    adjustment.Should().Be(default(ChannelValueAdjustment));
                    error.Should().Be(expectedError);
                }
            }

            private static ChannelValueAdjustmentParser Arrange()
            {
                var colors = new ColorManager();
                colors.SetColor(ColorName, 255, 127, 1);
                var fixtures = new MockCommandFixtures
                {
                    { "Red1", 1 },
                    { "Green1", 2 },
                    { "Blue1", 3 },
                    { "Red2", 4 },
                    { "Green2", 5 },
                    { "Blue2", 6 },
                    { ColorFixture, ColorChannel }
                };
                fixtures.SetColorChannelValuePropertiesByChannel(ColorChannel);
                var result = new ChannelValueAdjustmentParser(colors, fixtures, null);  //TODO: use mock 
                return result;
            }

            private static readonly IReadOnlyUniverse BaseUniverse = 
                Universe
                .CreateEmpty()
                .SetValue(ChannelValue.FromPercentage(1, 11))
                .SetValue(ChannelValue.FromPercentage(2, 22))
                .SetValue(ChannelValue.FromPercentage(3, 33))
                .AsReadOnly();
        }
    }
}
