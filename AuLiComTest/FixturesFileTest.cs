using AuLiComLib.Fixtures;
using AuLiComLib.Fixtures.Kinds;
using FluentAssertions;
using FluentAssertions.Execution;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO.Abstractions.TestingHelpers;

namespace AuLiComTest
{
    [TestClass]
    public class FixturesFileTest
    {
        [TestClass]
        public class Load
        {
            [TestMethod]
            public void FileContainsTwoDifferentFixtures_TwoFixturesAreCreated()
            {
                // Arrange
                Arrange(out FixturesFile fixturesFile)
                    .AddFile(FixturesFilePath, new MockFileData(TwoFixtures));

                // Act
                IFixture[] fixtures = fixturesFile.Load(FixturesFilePath);

                // Assert
                    fixtures.Should().BeEquivalentTo(new IFixture[]
                    {
                        new GenericLamp(null)
                        {
                            Name = "Lamp1",
                            Channel = 1
                        },
                        new CameoLedBar3Ch2(null)
                        {
                            Name = "LED",
                            Channel = 2
                        },
                    });
            }

            [TestMethod]
            public void FileDoesNotExist_FileNotFoundExceptionIsThrown()
            {
                // Arrange
                Arrange(out FixturesFile fixturesFile);

                // Act
                Action act = () => fixturesFile.Load(NonExistentFilePath);

                // Assert
                act.Should().Throw<FileNotFoundException>();
            }

            [TestMethod]
            public void FileHasWrongExtension_ArgumentExceptionIsThrown()
            {
                // Arrange
                Arrange(out FixturesFile fixturesFile);

                // Act
                Action act = () => fixturesFile.Load(WrongExtensionFilePath);

                // Assert
                act.Should().Throw<ArgumentException>();
            }

            private MockFileSystem Arrange(out FixturesFile fixturesFile)
            {
                var fileSystem = new MockFileSystem();
                fixturesFile = new FixturesFile(null, fileSystem);
                return fileSystem;
            }

            private const string FixturesFilePath = "fixtures" + FixturesFile.Extension;
            private const string NonExistentFilePath = "ThisFileDoesNotExist" + FixturesFile.Extension;
            private const string WrongExtensionFilePath = "WrongExtension" + FixturesFile.Extension + "SomethingElse";

            private const string TwoFixtures = """
[
  {
    "Kind": "GenericLamp",
    "Name": "Lamp1",
    "Channel": 1
  },
  {
    "Kind": "CameoLedBar3Ch2",
    "Name": "LED",
    "Channel": 2
  }
]
""";
        }
    }
}