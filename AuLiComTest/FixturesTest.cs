using AuLiComLib.Fixtures;
using AuLiComLib.Fixtures.Kinds;
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
    public class FixturesTest
    {
        [TestClass]
        public class Get
        {
            [TestMethod]
            public void FixtureOfGivenNameAndTypeExists_FixtureIsReturned()
            {
                // Arrange
                Fixtures fixtures = Arrange();

                // Act
                GenericLamp lamp1 = fixtures.Get<GenericLamp>(name: Lamp1);

                // Assert
                using (new AssertionScope())
                {
                    lamp1.Should().NotBeNull();
                    lamp1.Channel.Should().Be(1);
                }
            }

            [TestMethod]
            public void FixtureWithGivenNameDoesNotExist_KeyNotFoundExceptionIsThrown()
            {
                // Arrange
                Fixtures fixtures = Arrange();

                // Act
                Action act = () => fixtures.Get<GenericLamp>(name: Lamp3DoesNotExist);

                // Assert
                act.Should().Throw<KeyNotFoundException>();
            }

            [TestMethod]
            public void FixtureOfGivenTypeDoesNotExist_InvalidCastExceptionIsThrown()
            {
                // Arrange
                Fixtures fixtures = Arrange();

                // Act
                Action act = () => fixtures.Get<CameoLedBar3Ch2>(name: Lamp1);

                // Assert
                act.Should().Throw<InvalidCastException>();
            }

            private Fixtures Arrange() => new Fixtures(
                new GenericLamp(null) { Name = Lamp1, Channel = 1 },
                new GenericLamp(null) { Name = Lamp2, Channel = 2 }
            );

            private const string Lamp1 = "Lamp1";
            private const string Lamp2 = "Lamp2";
            private const string Lamp3DoesNotExist = "Lamp3DoesNotExist";
        }
    }
}
