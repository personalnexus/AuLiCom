using AuLiComLib.Common;
using AuLiComLib.Fixtures;
using AuLiComLib.Protocols;
using AuLiComLib.Protocols.Dmx;
using AuLiComLib.Scenes;
using ExcelDna.Integration;
using System;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace AuLiComXL
{
    public static class ExcelFunctions
    {
        // Scenes

        [ExcelFunction]
        public static void AuLiComCreateScene(string name, double[] channels, double[] values)
        {
            IReadOnlyUniverse universe = channels
            .Zip(values, (channel, value) => ChannelValue.FromPercentage((int)channel, (int)value))
            .ToReadOnlyUniverse();
            ExcelRuntime
            .GetInstance()
            .SceneManager
            .CreateScene(name, universe);
        }

        [ExcelFunction(IsVolatile = true)]
        public static object AuLiComGetScenes() =>
            ExcelRuntime
            .GetInstance()
            .SceneManager
            .ScenesByName
            .Keys
            .ToVerticalRange<string>();

        [ExcelFunction]
        public static void AuLiComRemoveScene(string name) =>
            ExcelRuntime
            .GetInstance()
            .SceneManager
            .RemoveScene(name);

        [ExcelFunction]
        public static void AuLiComSetSingleActiveScene(string name, double fadeTimeInSeconds) =>
            ExcelRuntime
            .GetInstance()
            .SceneManager
            .SetSingleActiveScene(name, TimeSpan.FromSeconds(fadeTimeInSeconds));


        // DMX Ports

        [ExcelFunction]
        public static object AuLiComGetDmxPorts(bool forceRefresh) =>
            ExcelRuntime
            .GetDmxPorts(forceRefresh)
            .Keys
            .ToVerticalRange<string>();


        // DMX Connection

        [ExcelFunction]
        public static string AuLiComCreateDmxConnection(string portName) => 
            ExcelRuntime
            .Initialize(portName);

        [ExcelFunction(IsVolatile = true)]
        public static int AuLiComGetChannelValue(int channel) =>
            ExcelRuntime
            .GetInstance()
            .DmxConnection
            .GetValue(channel)
            .ValueAsPercentage;

        [ExcelFunction]
        public static void AuLiComSetChannelValue(int channel, int percentage) =>
            ExcelRuntime
            .GetInstance()
            .DmxConnection
            .SetValue(ChannelValue.FromPercentage(channel, percentage));


        // Recalculation

        [ExcelFunction]
        public static double AuLiComSetTimer(double milliseconds) =>
            ExcelRuntime
            .SetRecalculationTimer(milliseconds);


        // Fixtures

        [ExcelFunction]
        public static object[,] AuLiComGetFixtureTypes() =>
            ExcelRuntime
            .GetInstance()
            .FixtureFactory
            .GetFixtureTypes()
            .ToVerticalRange();

        [ExcelFunction]
        public static int AuLiComGetFixtureChannelCount(string name) =>
            ExcelRuntime
            .GetInstance()
            .FixtureManager
            .Get<IFixture>(name)
            .ChannelCount;

        [ExcelFunction]
        public static object[,] AuLiComGetFixtureChannelInfos() =>
            ExcelRuntime
            .GetInstance()
            .FixtureManager
            .GetFixtureChannelInfos()
            .Select(x => new object[] { x.FixtureName, x.FixtureType, x.ChannelName, x.StartChannel })
            .To2dRange();

        [ExcelFunction]
        public static string AuLiComCreateFixture(string name, string type, int channel)
        {
            ExcelRuntime runtime = ExcelRuntime.GetInstance();
            FixtureInfo fixtureInfo = new(FixtureName: name,
                                          FixtureType: type,
                                          StartChannel: channel);
            IFixture fixture = runtime.FixtureFactory.CreateFromFixtureInfo(fixtureInfo);
            string result = runtime.FixtureManager.TryAdd(fixture) 
                ? "Added" 
                : "Already exists";
            return result;
        }

        // Commands

        [ExcelFunction]
        public static object[,] AuLiComGetCommandDescriptions() =>
             ExcelRuntime
            .GetInstance()
            .CommandExecutor
            .GetCommandDescriptions()
            .ToVerticalRange();

        [ExcelFunction]
        public static object[,] AuLiComExecuteCommand(string commandString) =>
             ExcelRuntime
            .GetInstance()
            .ExecuteCommandAndCaptureOutput(commandString)
            .ToVerticalRange();

        [ExcelFunction]
        public static object[,] AuLiComGetCommandOutput() =>
             ExcelRuntime
            .GetInstance()
            .GetLastCommandOutput()
            .ToVerticalRange();
    }
}