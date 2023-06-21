using AuLiComLib.Protocols;
using AuLiComTest.Mocks;
using FluentAssertions;
using FluentAssertions.Execution;
using System;
using System.Collections.Generic;
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
                byte[] targetValues = connection.Values.ToArray();
                var changes = new ChannelValueChanges(connection, targetValues, TimeSpan.FromMilliseconds(200));

                // Act
                Action act = () => changes.Apply();

                // Assert
                using (new AssertionScope())
                {
                    changes.HasChanges.Should().BeFalse();
                    act.ExecutionTime().Should().BeLessThan(TimeSpan.FromMilliseconds(16));
                    connection.Values.Should().BeEquivalentTo(targetValues);
                }
            }

            [TestMethod]
            public void TargetValuesAreDifferent_TargetValuesAreApplied()
            {
                // Arrange
                Arrange(out MockConnection connection);
                var targetValues = new byte[513];
                targetValues[0] = 128;
                targetValues[1] = 32;
                var changes = new ChannelValueChanges(connection, targetValues, TimeSpan.FromMilliseconds(200));

                // Act
                Action act = () => changes.Apply();

                // Assert
                using (new AssertionScope())
                {
                    changes.HasChanges.Should().BeTrue();
                    act.ExecutionTime().Should()
                        .BeGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(200)).And
                        .BeLessThanOrEqualTo(TimeSpan.FromMilliseconds(300));
                    connection.Values.Should().BeEquivalentTo(targetValues);
                }
            }

            private static void Arrange(out MockConnection connection)
            {
                connection = new MockConnection();
                connection.Values = new byte[513];
                connection.Values[0] = 64;
                connection.Values[1] = 128;
            }
        }
    }
}
