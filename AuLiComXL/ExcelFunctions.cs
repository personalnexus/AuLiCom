using AuLiComLib.Common;
using AuLiComLib.Protocols;
using AuLiComLib.Protocols.Dmx;
using ExcelDna.Integration;
using System;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace AuLiComXL
{
    public static class ExcelFunctions
    {
        [ExcelFunction]
        public static object AuLiComGetDmxPorts(bool forceRefresh) => 
            GetAvailableDmxPorts(forceRefresh).Keys.ToVerticalRange<string>();

        [ExcelFunction]
        public static string AuLiComCreateDmxConnection(string portName) => 
            ExcelDmxConnection.EnsureInitialized(ref Connection, portName, GetAvailableDmxPorts);

        [ExcelFunction(IsVolatile = true)]
        public static int AuLiComGetChannelValue(int channel) =>
            GetConnection().GetValue(channel).ValueAsPercentage;

        [ExcelFunction]
        public static void AuLiComSetChannelValue(int channel, int percentage) =>
            GetConnection().SetValue(ChannelValue.FromPercentage(channel, percentage));

        [ExcelFunction]
        public static object AuLiComSetTimer(int milliseconds)
            => SetRecalculationTimer(milliseconds);

        // DMX Ports

        private static Dictionary<string, ISerialPort> GetAvailableDmxPorts(bool forceRefresh)
        {
            if (AvailablePortsByName == null || forceRefresh)
            {
                AvailablePortsByName = DmxPorts.GetPortsByName();
            }
            return AvailablePortsByName;
        }
        private static Dictionary<string, ISerialPort>? AvailablePortsByName;

        // DMX Connection

        private static IConnection GetConnection([CallerMemberName]string? caller = null) => Connection?.DmxConnection ?? throw new Exception($"Must call {nameof(AuLiComCreateDmxConnection)} before {caller}.");
        private static ExcelDmxConnection? Connection;

        // Recalculation

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