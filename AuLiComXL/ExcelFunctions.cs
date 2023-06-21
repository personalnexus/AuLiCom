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
            IEnumerable<ChannelValue> channelValues = channels.Zip(values, (channel, value) => ChannelValue.FromPercentage((int)channel, (int)value));
            IScene scene = GetSceneManager().CreateScene(name, 0.0, channelValues);
            Runtime.ScenesByName[name] = scene;
        }

        [ExcelFunction(IsVolatile = true)]
        public static object AuLiComGetScenes() => 
            Runtime.ScenesByName.Keys.ToVerticalRange<string>();

        [ExcelFunction]
        public static void AuLiComRemoveScene(string name) => 
            Runtime.ScenesByName.Remove(name);

        [ExcelFunction]
        public static void AuLiComSetSingleActiveScene(string name, double fadeTimeInSeconds) => 
            GetSceneManager().SetSingleActiveScene(Runtime!.ScenesByName[name], TimeSpan.FromSeconds(fadeTimeInSeconds));

        // DMX Ports

        [ExcelFunction]
        public static object AuLiComGetDmxPorts(bool forceRefresh) =>
             GetDmxPorts(forceRefresh).Keys.ToVerticalRange<string>();

        private static Dictionary<string, ISerialPort> GetDmxPorts(bool forceRefresh)
        {
            if (AvailablePortsByName == null || forceRefresh)
            {
                AvailablePortsByName = DmxPorts.GetPortsByName();
            }
            return AvailablePortsByName;
        }
        private static Dictionary<string, ISerialPort>? AvailablePortsByName;

        // DMX Connection

        [ExcelFunction]
        public static string AuLiComCreateDmxConnection(string portName) =>
            ExcelRuntime.EnsureInitialized(ref Runtime, portName, GetDmxPorts);

        [ExcelFunction(IsVolatile = true)]
        public static int AuLiComGetChannelValue(int channel) =>
            GetConnection().GetValue(channel).ValueAsPercentage;

        [ExcelFunction]
        public static void AuLiComSetChannelValue(int channel, int percentage) =>
            GetConnection().SetValue(ChannelValue.FromPercentage(channel, percentage));

        private static IConnection GetConnection([CallerMemberName] string? caller = null) => Runtime?.DmxConnection ?? throw new Exception($"Must call {nameof(AuLiComCreateDmxConnection)} before {caller}.");
        private static ISceneManager GetSceneManager([CallerMemberName] string? caller = null) => Runtime?.SceneManager ?? throw new Exception($"Must call {nameof(AuLiComCreateDmxConnection)} before {caller}.");
        private static ExcelRuntime? Runtime;

        // Recalculation

        [ExcelFunction]
        public static object AuLiComSetTimer(int milliseconds)
            => SetRecalculationTimer(milliseconds);

        private static int SetRecalculationTimer(double interval)
        {
            if (RecalculationTimer == null || RecalculationTimer.Interval != interval)
            {
                RecalculationTimer?.Dispose();
                RecalculationTimer = new System.Timers.Timer(interval);
                RecalculationTimer.Elapsed += (sender, EventArgs) => ExcelAsyncUtil.QueueAsMacro(x => XlCall.Excel(XlCall.xlcCalculateNow), null);
                RecalculationTimer.Enabled = true;
            }
            return (int)RecalculationTimer.Interval;
        }
        private static System.Timers.Timer? RecalculationTimer;

    }
}