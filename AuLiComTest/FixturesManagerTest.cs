using AuLiComLib.Fixtures;
using AuLiComLib.Fixtures.Types;
using FluentAssertions;
using FluentAssertions.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AuLiComTest
{
    [TestClass]
    public class FixturesManagerTest
    {
        [TestClass]
        public class TryAdd
        {
            [TestMethod]
            public void DuplicateName_FalseIsReturned()
            {
                // Arrange
                var fixtures = new FixtureManager(
                    new GenericLamp(null) { Name = "Lamp1", StartChannel = 1 }//TODO: use Mock connection
                );
                var duplicateNameLamp = new GenericLamp(null) { Name = "Lamp1", StartChannel = 5 }; //TODO: use Mock connection

                // Act
                bool addResult = fixtures.TryAdd(duplicateNameLamp);

                // Assert
                using (new AssertionScope())
                {
                    addResult.Should().BeFalse();
                }
            }

            [TestMethod]
            public void DuplicateChannel_ArgumentExceptionIsThrown()
            {
                // Arrange
                var fixtures = new FixtureManager(
                    new GenericLamp(null) { Name = "Lamp1", StartChannel = 1 }//TODO: use Mock connection
                );
                var duplicateNameLamp = new GenericLamp(null) { Name = "Lamp2", StartChannel = 1 }; //TODO: use Mock connection

                // Act
                Func<bool> act = () => fixtures.TryAdd(duplicateNameLamp);

                // Assert
                using (new AssertionScope())
                {
                    act.Should().Throw<ArgumentException>().WithMessage($"Channel 1 is already in use by Lamp1 and cannot be used by Lamp2 (Start: 1, Count: 1)");
                }
            }
        }

        [TestClass]
        public class TryGetChannelsByName
        {
            [TestMethod]
            public void MatchesNothing_EmptyEnumerableIsReturned()
            {
                // Arrange
                var fixtures = new FixtureManager();

                // Act
                bool result = fixtures.TryGetChannelsByName("Do not match", out IEnumerable<int> channels);

                // Assert
                using (new AssertionScope())
                {
                    result.Should().BeFalse();
                    channels.Should().BeEmpty();
                }
            }

            [TestMethod]
            public void MatchesLedFixtureName_AllLedChannelsAreReturned() => ShouldBeSuccess(
                "led",
                2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);

            [TestMethod]
            public void MatchesAllAlias_AllFixtureChannelsAreReturned() => ShouldBeSuccess(
                "AlL",
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);

            [TestMethod]
            public void MatchesGreenChannels_ThreeGreenChannelsAreReturned() => ShouldBeSuccess(
                "Green",
                6, 9, 12);

            [TestMethod]
            public void MatchesLampIntensity_LampIntensityIsReturned() => ShouldBeSuccess(
                "Intensity",
                1);

            private void ShouldBeSuccess(string nameOrAliasSubstring, params int[] expectedChannels)
            {
                // Arrange
                FixtureManager fixtures = new FixtureManager(
                    new GenericLamp(null) { Name = "Lamp1", StartChannel = 1, Alias = "All" }, //TODO: use Mock connection
                    new CameoLedBar_12Ch(null) { Name = "LED", StartChannel = 2, Alias = "All Color" });  //TODO: use Mock connection

                // Act
                bool result = fixtures.TryGetChannelsByName("Intensity", out IEnumerable<int> channels);

                // Assert
                using (new AssertionScope())
                {
                    result.Should().BeTrue();
                    channels.Should().BeEquivalentTo(new[] { 1 });
                }
            }

        }

        [TestClass]
        public class GetFixtureChannelInfos
        {
            [TestMethod]
            public void Two1ChannelAndOne3ChannelFixture_FiveChannelInfosAreReturned()
            {
                // Arrange
                var fixtures = new FixtureManager(
                    new GenericLamp(null) { Name = "Lamp1", StartChannel = 1 },   //TODO: use Mock connection
                    new CameoLedBar_3Ch2(null) { Name = "LED", StartChannel = 2 }, //TODO: use Mock connection
                    new GenericLamp(null) { Name = "Lamp2", StartChannel = 5 }    //TODO: use Mock connection
                );

                // Act
                IEnumerable<FixtureChannelInfo> infos = fixtures.GetFixtureChannelInfos();

                // Assert
                using (new AssertionScope())
                {
                    infos.Should().BeEquivalentTo(new[]
                    {
                        new FixtureChannelInfo("Lamp1", "GenericLamp", null, "Intensity", 1),
                        new FixtureChannelInfo("LED", "CameoLedBar_3Ch2", null, "Red", 2),
                        new FixtureChannelInfo("LED", "CameoLedBar_3Ch2", null, "Green", 3),
                        new FixtureChannelInfo("LED", "CameoLedBar_3Ch2", null, "Blue", 4),
                        new FixtureChannelInfo("Lamp2", "GenericLamp", null, "Intensity", 5),
                    });
                }
            }
        }

        [TestClass]
        public class Get
        {
            [TestMethod]
            public void FixtureOfGivenNameAndTypeExists_FixtureIsReturned()
            {
                // Arrange
                FixtureManager fixtures = Arrange();

                // Act
                GenericLamp lamp1 = fixtures.Get<GenericLamp>(name: Lamp1);

                // Assert
                using (new AssertionScope())
                {
                    lamp1.Should().NotBeNull();
                    lamp1.Name.Should().Be(Lamp1);
                    lamp1.StartChannel.Should().Be(1);
                    lamp1.ChannelCount.Should().Be(1);
                }
            }

            [TestMethod]
            public void FixtureWithGivenNameDoesNotExist_KeyNotFoundExceptionIsThrown()
            {
                // Arrange
                FixtureManager fixtures = Arrange();

                // Act
                Action act = () => fixtures.Get<GenericLamp>(name: Lamp3DoesNotExist);

                // Assert
                act.Should().Throw<KeyNotFoundException>();
            }

            [TestMethod]
            public void FixtureOfGivenTypeDoesNotExist_InvalidCastExceptionIsThrown()
            {
                // Arrange
                FixtureManager fixtures = Arrange();

                // Act
                Action act = () => fixtures.Get<CameoLedBar_3Ch2>(name: Lamp1);

                // Assert
                act.Should().Throw<InvalidCastException>();
            }

            private static FixtureManager Arrange() => new FixtureManager(
                new GenericLamp(null) { Name = Lamp1, StartChannel = 1 }, //TODO: use Mock connection
                new GenericLamp(null) { Name = Lamp2, StartChannel = 2 }  //TODO: use Mock connection
            );

            private const string Lamp1 = "Lamp1";
            private const string Lamp2 = "Lamp2";
            private const string Lamp3DoesNotExist = "Lamp3DoesNotExist";
        }
    }
}
