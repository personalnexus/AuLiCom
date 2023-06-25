using AuLiComLib.Common;
using AuLiComLib.Fixtures;
using AuLiComLib.Protocols;
using AuLiComLib.Protocols.Dmx;
using AuLiComLib.Scenes;
using ExcelDna.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComXL
{
    internal class ExcelRuntime: IDisposable
    {
        public static ExcelRuntime GetInstance([CallerMemberName] string? callingMethod = null) => _instance ?? throw new InvalidOperationException($"Must initialize AuLiCom's Excel runtime before calling {callingMethod}.");

        public static void DisposeInstance()
        {
            lock (_instanceInitializationLock)
            {
                _instance?.Dispose();
                _instance = null;
            }
        }

        private static ExcelRuntime? _instance;
        private static readonly object _instanceInitializationLock = new object();

        internal static string InitializeWithOnlyDmxPort()
        {
            lock (_instanceInitializationLock)
            {
                //
                // If there is only one port, connect to that one. Otherwise the user has to pick one and create the runtime themselves
                //
                Dictionary<string, ISerialPort> dmxPortsByName = GetDmxPorts(forceRefresh: true);
                if (dmxPortsByName.Count == 1)
                {
                    Initialize(dmxPortsByName.Values.First());
                    return GetRuntimeStatus();
                }
                else
                {
                    return $"There is more than one DMX port. Select the right one and pass it to {nameof(Initialize)}. {string.Join(", ", dmxPortsByName.Keys)}";
                }
            }
        }

        internal static string Initialize(string portName)
        {
            lock (_instanceInitializationLock)
            {
                if (GetDmxPorts(forceRefresh: false).TryGetValue(portName, out ISerialPort? port))
                {
                    Initialize(port);
                }
                return GetRuntimeStatus();
            }
        }

        internal static string Initialize(ISerialPort port)
        {
            lock (_instanceInitializationLock)
            {
                if (_instance?.PortName != port.PortName)
                {
                    _instance?.Dispose();
                }
                _instance = new ExcelRuntime(port);
                return GetRuntimeStatus();
            }
        }


        private static string GetRuntimeStatus() => _instance != null ? $"Connected to {_instance.PortName}" : "Not Connected";


        internal static double SetRecalculationTimer(double milliseconds)
        {
            if (RecalculationTimer == null || RecalculationTimer.Interval != milliseconds)
            {
                RecalculationTimer?.Dispose();
                RecalculationTimer = new System.Timers.Timer(milliseconds);
                RecalculationTimer.Elapsed += (sender, EventArgs) => ExcelAsyncUtil.QueueAsMacro(x => XlCall.Excel(XlCall.xlcCalculateNow), null);
                RecalculationTimer.Enabled = true;
            }
            return RecalculationTimer.Interval;
        }
        private static System.Timers.Timer? RecalculationTimer;


        internal static Dictionary<string, ISerialPort> GetDmxPorts(bool forceRefresh)
        {
            if (AvailablePortsByName == null || forceRefresh)
            {
                AvailablePortsByName = DmxPorts.GetPortsByName();
            }
            return AvailablePortsByName;
        }
        private static Dictionary<string, ISerialPort>? AvailablePortsByName;



        private ExcelRuntime(ISerialPort port)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            PortName = port.PortName;
            var executor = new SystemThread(PortName);

            DmxConnection = new DmxConnection(port, executor, _cancellationTokenSource.Token);
            SceneManager = new NamedSceneManager(DmxConnection);
            FixtureFactory = new FixtureFactory(DmxConnection);
            FixtureManager = new FixtureManager();
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }

        private readonly CancellationTokenSource _cancellationTokenSource;

        public string PortName { get; }

        public IConnection DmxConnection { get; }
        public INamedSceneManager SceneManager { get; }
        public IFixtureManager FixtureManager { get; }
        public IFixtureFactory FixtureFactory { get; }
    }
}
