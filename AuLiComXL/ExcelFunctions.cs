using AuLiComLib.Common;
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
    }
}