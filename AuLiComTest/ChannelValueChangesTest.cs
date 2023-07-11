using AuLiComLib.Protocols;
using AuLiComTest.Mocks;
using FluentAssertions;
using FluentAssertions.Execution;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComTest
{
    [TestClass]
    public class ChannelValueChangesTest
    {
        [TestClass]
        public class Apply
        {
            [TestMethod]
            public void TargetAndCurrentAreSame_NoChanges()
            {
                // Arrange
                Arrange(out MockConnection connection);
                IReadOnlyUniverse targetUniverse = Universe
                                                   .CreateEmpty()
                                                   .SetValues(connection.CurrentUniverse.GetValues())
                                                   .AsReadOnly();
                var changes = new ChannelValueChanges(connection, targetUniverse, TimeSpan.FromMilliseconds(200));

                // Act
                Action act = () => changes.Apply();

                // Assert
                using (new AssertionScope())
                {
                    changes.HasChanges.Should().BeFalse();
                    act.ExecutionTime().Should().BeLessThan(TimeSpan.FromMilliseconds(16));
                    connection.CurrentUniverse.GetValuesCopy().Should().BeEquivalentTo(targetUniverse.GetValuesCopy());
                }
            }

            [TestMethod]
            public void TargetValuesAreDifferent_TargetValuesAreApplied()
            {
                // Arrange
                Arrange(out MockConnection connection);
                IReadOnlyUniverse targetUniverse = Universe.CreateEmpty()
                                                   .SetValue(ChannelValue.FromByte(1, 128))
                                                   .SetValue(ChannelValue.FromByte(2, 32))
                                                   .AsReadOnly();
                var changes = new ChannelValueChanges(connection, targetUniverse, TimeSpan.FromMilliseconds(200));

                // Act
                Action act = () => changes.Apply().GetAwaiter().GetResult();

                // Assert
                using (new AssertionScope())
                {
                    changes.HasChanges.Should().BeTrue();
                    act.ExecutionTime().Should()
                        .BeGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(200)).And
                        .BeLessThanOrEqualTo(TimeSpan.FromMilliseconds(300));
                    connection.CurrentUniverse.GetValuesCopy().Should().BeEquivalentTo(targetUniverse.GetValuesCopy());
                }
            }

            private static void Arrange(out MockConnection connection)
            {
                IReadOnlyUniverse initialUniverse = Universe
                                                    .CreateEmpty()
                                                    .SetValue(ChannelValue.FromByte(1, 64))
                                                    .SetValue(ChannelValue.FromByte(2, 128))
                                                    .AsReadOnly();
                connection = new MockConnection(initialUniverse);
            }
        }
    }
}
