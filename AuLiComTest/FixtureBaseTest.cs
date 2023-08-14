using AuLiComLib.Fixtures.Types;
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
            public void CalledOnClassWithThreeChannelValueProperties_ChannelValuePropertiesAreNumberedSequentially()
            {
                // Act
                var ledBar = new CameoLedBar_3Ch2(null)  //TODO: use mock connection
                {
                    StartChannel = 14
                };

                // Assert
                using (new AssertionScope())
                {
                    ledBar.Red.Channel.Should().Be(14);
                    ledBar.Green.Channel.Should().Be(15);
                    ledBar.Blue.Channel.Should().Be(16);
                }
            }
        }
    }
}
