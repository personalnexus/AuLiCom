using AuLiComLib.Chasers;
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
        public static string AuLiComSetScene(string name, double[] channels, double[] values)
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
        public static object AuLiComGetScenes() =>
            ExcelRuntime
            .GetInstance()
            .SceneManager
            .Select(sceneManager => sceneManager
                                    .ScenesByName
                                    .Keys
                                    .ToVerticalRange<string>())
            .Observe<object[,]>(callerParameters: null);

        [ExcelFunction]
        public static void AuLiComRemoveScene(string name) =>
            ExcelRuntime
            .GetInstance()
            .SceneManager
            .RemoveScene(name);

        [ExcelFunction]
        public static void AuLiComActivateSingleScene(string name, double fadeTimeInSeconds) =>
            ExcelRuntime
            .GetInstance()
            .SceneManager
            .ActivateSingleScene(name, TimeSpan.FromSeconds(fadeTimeInSeconds));

        [ExcelFunction]
        public static object AuLiComGetScenesVersion() =>
            ExcelRuntime
            .GetInstance()
            .SceneManager
            .Select(x => x.Version)
            .Observe<int>();

        [ExcelFunction]
        public static object AuLiComGetSceneChannelPercentages(string name) =>
            ExcelRuntime
            .GetInstance()
            .SceneManager
            .Select(sceneManager => sceneManager
                                    .ScenesByName[name]
                                    .Universe
                                    .GetChannelPercentages()
                                    .ToHorizontalRange())
            .Observe<object[,]>(callerParameters: name);



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
        public static object AuLiComGetChannelValue(int channel) =>
            ExcelRuntime
            .GetInstance()
            .DmxConnection
            .Select(connection => connection
                                  .GetValue(channel)
                                  .ValueAsPercentage)
            .Observe<int>();

        [ExcelFunction]
        public static object AuLiComGetChannelValues(double[] channels)
        {
            return ExcelRuntime
                   .GetInstance()
                   .DmxConnection
                   .Select(connection => connection
                                         .GetValues(channels.ToIntegerArray())
                                         .Select(x => x.ValueAsPercentage)
                                         .ToVerticalRange())
                   .Observe(callerParameters: channels);
        }

        [ExcelFunction]
        public static void AuLiComSetChannelValue(int channel, int percentage) =>
            ExcelRuntime
            .GetInstance()
            .DmxConnection
            .SetValue(ChannelValue.FromPercentage(channel, percentage));

        [ExcelFunction]
        public static object AuLiComGetChannelsVersion() =>
            ExcelRuntime
            .GetInstance()
            .DmxConnection
            .Select(x => x.Version)
            .Observe<int>();


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
        public static object AuLiComGetFixtureChannelCountTotal() =>
            ExcelRuntime
            .GetInstance()
            .FixtureManager
            .Select(x => x.GetChannelCountTotal())
            .Observe<int>();

        [ExcelFunction]
        public static object AuLiComGetFixtureChannelInfos(object fixtures) =>
            ExcelRuntime
            .GetInstance()
            .FixtureManager
            .Select(fixtureManager => fixtureManager
                                      .GetFixtureChannelInfos()
                                      .OrderBy(x => x.StartChannel)
                                      .Select(x => new object[] { x.FixtureName, x.FixtureType, x.ChannelName, x.StartChannel })
                                      .To2dRange())
            .Observe<object[,]>();

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
            .Select(x => x.Version)
            .Observe<int>();


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


        // Chasers

        [ExcelFunction]
        public static void AuLiComSetChaser(string name, string kindName, int stepDurationInMilliseconds, object[] sceneNames) =>
            ExcelRuntime
            .GetInstance()
            .SetChaser(name, 
                       kindName, 
                       stepDurationInMilliseconds, 
                       sceneNames
                       .Where(x => x != ExcelEmpty.Value)
                       .Select(x => x.ToString())
                       .ToArray());

        [ExcelFunction]
        public static object AuLiComGetChasers(object connection) =>
            ExcelRuntime
            .GetInstance()
            .ChaserManager
            .Select(x => x
                         .ChasersByName
                         .Keys
                         .ToVerticalRange())
            .Observe<object[,]>();

        [ExcelFunction]
        public static object AuLiComGetChaserTypes(object connection) =>
            Enum
            .GetNames(typeof(ChaserType))
            .ToVerticalRange();

        [ExcelFunction]
        public static void AuLiComStartChaser(string name) =>
            ExcelRuntime
            .GetInstance()
            .StartChaser(name);

        [ExcelFunction]
        public static void AuLiComStopChaser(string name) =>
            ExcelRuntime
            .GetInstance()
            .StopChaser(name);


        // Colors

        [ExcelFunction]
        public static void AuLiComSetColor(string name, int redPercentage, int greenPercentage, int bluePercentage) =>
            ExcelRuntime
            .GetInstance()
            .ColorManager
            .SetColor(name,
                      red:   ChannelValue.ByteFromPercentage(redPercentage),
                      green: ChannelValue.ByteFromPercentage(greenPercentage),
                      blue:  ChannelValue.ByteFromPercentage(bluePercentage));

        [ExcelFunction]
        public static object AuLiComGetColors() =>
            ExcelRuntime
            .GetInstance()
            .ColorManager
            .Select(x => x
                         .ColorsByName
                         .Keys
                         .ToVerticalRange())
            .Observe<object[,]>();

        [ExcelFunction]
        public static object AuLiComGetColorsVersion(object connection) =>
            ExcelRuntime
            .GetInstance()
            .ColorManager
            .Select(x => x.Version)
            .Observe<int>();

    }
}