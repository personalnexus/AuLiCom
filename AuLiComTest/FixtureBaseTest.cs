using AuLiComLib.Colors;
using AuLiComLib.Colors.Channels;
using AuLiComLib.Fixtures.Types;
using AuLiComTest.TestData;
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
    public class FixtureBaseTest
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void FixtureHasRedGreenBlueChannelValueProperties_ChannelValuePropertiesAreNumberedSequentiallyAndGroupedIntoOneColor()
            {
                // Act
                var ledBar = new CameoLedBar_3Ch2(null)  //TODO: use mock connection
                {
                    StartChannel = 14
                };

                // Assert
                using (new AssertionScope())
                {
                    ledBar.Red.Should().BeOfType<RedChannelValueProperty>();
                    ledBar.Red.Channel.Should().Be(14);

                    ledBar.Green.Should().BeOfType<GreenChannelValueProperty>();
                    ledBar.Green.Channel.Should().Be(15);

                    ledBar.Blue.Should().BeOfType<BlueChannelValueProperty>();
                    ledBar.Blue.Channel.Should().Be(16);

                    ledBar.Colors.Should().BeEquivalentTo(new[]
                    {
                        new
                        {
                            Red = ledBar.Red,
                            Green = ledBar.Green,
                            Blue = ledBar.Blue,
                        }
                    });
                }
            }

            [TestMethod]
            public void FixtureHasThreeSetsOfRedGreenBlueChannelValueProperties_ChannelValuePropertiesAreGroupedIntoThreeColors()
            {
                // Act
                var ledBar = new CameoLedBar_12Ch(null);

                // Assert
                using (new AssertionScope())
                {
                    ledBar.Colors.Should().BeEquivalentTo(new[]
                    {
                        new
                        {
                            Red = ledBar.Red1,
                            Green = ledBar.Green1,
                            Blue = ledBar.Blue1,
                        },
                        new
                        {
                            Red = ledBar.Red2,
                            Green = ledBar.Green2,
                            Blue = ledBar.Blue2,
                        },
                        new
                        {
                            Red = ledBar.Red3,
                            Green = ledBar.Green3,
                            Blue = ledBar.Blue3,
                        }
                    });
                }
            }

            [TestMethod]
            public void FixtureHasTwoReds_ColorGroupingFails() => new Action(() => new FixtureWithTwoReds(null)).Should().Throw<InvalidColorException>().WithMessage("Cannot add another Red channel at offset 1 before a color is complete with one Red, one Green and one Blue.");
            
            [TestMethod]
            public void CameoLedBar12Ch_ColorGroupingSuceeds() => new Action(() => new CameoLedBar_12Ch(null)).Should().NotThrow();

            [TestMethod]
            public void CameoLedBar3Ch2_ColorGroupingSuceeds() => new Action(() => new CameoLedBar_3Ch2(null)).Should().NotThrow();

            [TestMethod]
            public void CameoPixBar650CPro3Ch1_ColorGroupingSuceeds() => new Action(() => new CameoPixBar650CPro_3Ch1(null)).Should().NotThrow();

            [TestMethod]
            public void CameoThunderWash600Rgbw7Ch2_ColorGroupingSuceeds() => new Action(() => new CameoThunderWash600Rgbw_7Ch2(null)).Should().NotThrow();
        }
    }
}
