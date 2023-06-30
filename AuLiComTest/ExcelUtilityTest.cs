using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuLiComXL;
using FluentAssertions;

namespace AuLiComTest
{
    [TestClass]
    public class ExcelUtilityTest
    {
        [TestClass]
        public class To2dRange
        {
            [TestMethod]
            public void InputIsEmpty_EmptyArrayIsReturned()
            {
                // Arrange
                var input = new List<int[]>();

                // Act
                var output = input.To2dRange();

                // Assert
                using (new AssertionScope())
                {
                    output.Should().NotBeNull();
                    output.Length.Should().Be(0);
                }
            }

            [TestMethod]
            public void InputIsJaggedArray_2dArrayIsReturned()
            {
                // Arrange
                var input = new List<int[]>
                {
                    new[] {1, 2, 3},
                    new[] {2, 4, 6}
                };

                // Act
                var output = input.To2dRange();

                // Assert
                using (new AssertionScope())
                {
                    output.Should().NotBeNull();
                    output.GetLength(0).Should().Be(2);
                    output.GetLength(1).Should().Be(3);
                    output.Should().BeEquivalentTo(new int[2,3] { { 1, 2, 3 }, { 2, 4, 6 } });
                }
            }
        }
    }
}
