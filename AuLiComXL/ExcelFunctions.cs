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
        public static string AuLiComSetScene(object connection, string name, double[] channels, double[] values)
        {
            IReadOnlyUniverse universe = channels
            .Zip(values, (channel, value) => ChannelValue.FromPercentage((int)channel, (int)value))
            .ToReadOnlyUniverse();
            ExcelRuntime
            .GetInstance()
            .SceneManager
            .SetScene(name, universe);
            return name;
        }

        [ExcelFunction]
        public static object AuLiComGetScenes(object scenes) =>
            ExcelRuntime
            .GetInstance()
            .SceneManager
            .ScenesByName
            .Keys
            .ToVerticalRange<string>();

        [ExcelFunction]
        public static void AuLiComRemoveScene(object connection, string name) =>
            ExcelRuntime
            .GetInstance()
            .SceneManager
            .RemoveScene(name);

        [ExcelFunction]
        public static void AuLiComSetSingleActiveScene(object connection, string name, double fadeTimeInSeconds) =>
            ExcelRuntime
            .GetInstance()
            .SceneManager
            .SetSingleActiveScene(name, TimeSpan.FromSeconds(fadeTimeInSeconds));

        [ExcelFunction]
        public static object AuLiComGetScenesVersion(object connection) =>
            ExcelRuntime
            .GetInstance()
            .SceneManager
            .ObserveVersion();


        // DMX Ports

        [ExcelFunction]
        public static object AuLiComGetDmxPorts(bool forceRefresh) =>
            ExcelRuntime
            .GetDmxPorts(forceRefresh)
            .Keys
            .ToVerticalRange<string>();


        // DMX Connection

        [ExcelFunction]
        public static string AuLiComSetDmxConnection(string portName) => 
            ExcelRuntime
            .Initialize(portName);

        [ExcelFunction]
        public static int AuLiComGetChannelValue(object channelsVersion, int channel) =>
            ExcelRuntime
            .GetInstance()
            .DmxConnection
            .GetValue(channel)
            .ValueAsPercentage;

        [ExcelFunction]
        public static void AuLiComSetChannelValue(object connection, int channel, int percentage) =>
            ExcelRuntime
            .GetInstance()
            .DmxConnection
            .SetValue(ChannelValue.FromPercentage(channel, percentage));

        [ExcelFunction]
        public static object AuLiComGetChannelsVersion(object connection) =>
            ExcelRuntime
            .GetInstance()
            .DmxConnection
            .ObserveVersion();

        // Recalculation

        [ExcelFunction]
        public static double AuLiComSetTimer(object connection, double milliseconds) =>
            ExcelRuntime
            .SetRecalculationTimer(milliseconds);


        // Fixtures

        [ExcelFunction]
        public static object[,] AuLiComGetFixtureTypes(object connection) =>
            ExcelRuntime
            .GetInstance()
            .FixtureFactory
            .GetFixtureTypes()
            .ToVerticalRange();

        [ExcelFunction]
        public static int AuLiComGetFixtureChannelCount(object fixtures, string name) =>
            ExcelRuntime
            .GetInstance()
            .FixtureManager
            .Get<IFixture>(name)
            .ChannelCount;

        [ExcelFunction]
        public static int AuLiComGetFixtureChannelCountTotal(object fixtures) =>
            ExcelRuntime
            .GetInstance()
            .FixtureManager
            .GetChannelCountTotal();

        [ExcelFunction]
        public static object[,] AuLiComGetFixtureChannelInfos(object fixtures) =>
            ExcelRuntime
            .GetInstance()
            .FixtureManager
            .GetFixtureChannelInfos()
            .Select(x => new object[] { x.FixtureName, x.FixtureType, x.ChannelName, x.StartChannel })
            .To2dRange();

        [ExcelFunction]
        public static string AuLiComSetFixture(object connection, string name, string type, int channel, string alias = "")
        {
            ExcelRuntime runtime = ExcelRuntime.GetInstance();
            FixtureInfo fixtureInfo = new(FixtureName: name,
                                          FixtureType: type,
                                          StartChannel: channel,
                                          Alias: alias);
            IFixture fixture = runtime.FixtureFactory.CreateFromFixtureInfo(fixtureInfo);
            string result = runtime.FixtureManager.TryAdd(fixture) 
                ? "Added" 
                : "Already exists";
            return result;
        }

        [ExcelFunction]
        public static object AuLiComGetFixturesVersion(object connection) =>
            ExcelRuntime
            .GetInstance()
            .FixtureManager
            .ObserveVersion();


        // Commands

        [ExcelFunction]
        public static object[,] AuLiComGetCommandDescriptions(object connection) =>
             ExcelRuntime
            .GetInstance()
            .CommandExecutor
            .GetCommandDescriptions()
            .ToVerticalRange();

        [ExcelFunction]
        public static object[,] AuLiComExecuteCommand(object connection, string commandString) =>
             ExcelRuntime
            .GetInstance()
            .ExecuteCommandAndCaptureOutput(commandString)
            .ToVerticalRange();

        [ExcelFunction]
        public static object[,] AuLiComGetCommandOutput(object connection) =>
             ExcelRuntime
            .GetInstance()
            .GetLastCommandOutput()
            .ToVerticalRange();
    }
}