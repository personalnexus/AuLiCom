using AuLiComLib.CommandExecutor;
using AuLiComLib.Common;
using AuLiComLib.Protocols;
using AuLiComTest.Mocks;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComTest
{
    [TestClass]
    public class CommandExecutorTest
    {
        [TestClass]
        public class Execute
        {
            [TestMethod]
            public void SetValueCommandWithErrors_SynatxErrorReportedInOutput() => ExecuteCommand(
                command: "humpelpumpel",
                expectedResult: "INVALID 'humpelpumpel'.",
                expectedOutput: "Command has to contain exactly one '@'."
            );

            [TestMethod]
            public void SetValueOfChannel1To100_DoneNoError() => ExecuteCommand(
                command: "1@100",
                expectedResult: "DONE."
            );

            [TestMethod]
            public void List_DoneNoError() => ExecuteCommand(
                command: "LIST",
                expectedResult: "DONE.",
                expectedOutput: "ALL\t0"); // If SetChannelValueCommand was not last, it would 

            private static void ExecuteCommand(string command, string expectedResult, params string[] expectedOutput)
            {
                // Arrange
                var connection = new MockConnection(Universe.CreateEmptyReadOnly());
                var outputStringList = new StringListWriteConsole();
                var commandExecutor = new CommandExecutor(connection, outputStringList, null, null, null); //TODO: use fixtures, scene manager and file system mock

                // Act
                string result = commandExecutor.Execute(command);

                // Assert
                using (new AssertionScope())
                {
                    result.Should().Be(expectedResult);
                    outputStringList.ToArray().Should().BeEquivalentTo(expectedOutput);
                }
            }
        }
    }
}
