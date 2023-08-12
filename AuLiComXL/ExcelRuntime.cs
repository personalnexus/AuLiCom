using AuLiComLib.Chasers;
using AuLiComLib.CommandExecutor;
using AuLiComLib.Common;
using AuLiComLib.Fixtures;
using AuLiComLib.Protocols;
using AuLiComLib.Protocols.Dmx;
using AuLiComLib.Scenes;
using ExcelDna.Integration;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComXL
{
    internal class ExcelRuntime: IDisposable
    {
        public static ExcelRuntime GetInstance([CallerMemberName] string? callingMethod = null) =>
            GetInstanceCore(callingMethod);

        private static ExcelRuntime GetInstanceCore(string? callingMethod) =>
            _instance ?? throw new InvalidOperationException($"Must initialize AuLiCom's Excel runtime before calling {callingMethod}.");

        public static void DisposeInstance()
        {
            lock (_instanceInitializationLock)
            {
                _instance?.Dispose();
                _instance = null;
            }
        }

        private static ExcelRuntime? _instance;
        private static readonly object _instanceInitializationLock = new();

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
                if (SerialPorts.EmptyPortName.Equals(portName, StringComparison.OrdinalIgnoreCase))
                {
                    Initialize(SerialPorts.Empty);
                }
                else if (GetDmxPorts(forceRefresh: false).TryGetValue(portName, out ISerialPort? port))
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
                    _instance = new ExcelRuntime(port);
                }
                return GetRuntimeStatus();
            }
        }


        private static string GetRuntimeStatus() => $"DMX:{_instance?.PortName ?? "(none)"}";


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
            PortName = port.PortName;
            _dmxConnectionThread = new SystemThread(PortName);
            _commandOutputWriter = new StringListWriteConsole();

            DmxConnection = new DmxConnection(port, _dmxConnectionThread);
            var sceneManager = new NamedSceneManager(DmxConnection);
            FixtureFactory = new FixtureFactory(DmxConnection);
            FixtureManager = new FixtureManager();
            CommandExecutor = new CommandExecutor(DmxConnection,
                                                  _commandOutputWriter,
                                                  FixtureManager,
                                                  sceneManager,
                                                  new FileSystem());
            ChaserManager = new ChaserManager(DmxConnection);
            SceneManager = sceneManager;
        }

        public void Dispose()
        {
            DmxConnection.Dispose();
            _dmxConnectionThread.Join();
        }

        private readonly SystemThread _dmxConnectionThread;
        private readonly StringListWriteConsole _commandOutputWriter;
        
        public string PortName { get; }

        public IConnection DmxConnection { get; }
        public INamedSceneManager SceneManager { get; }
        public IFixtureManager FixtureManager { get; }
        public ICommandExecutor CommandExecutor { get; }
        public IChaserManager ChaserManager { get; }


        public IFixtureFactory FixtureFactory { get; }
        
        // Commands

        public IEnumerable<string> ExecuteCommandAndCaptureOutput(string commandString)
        {
            _commandOutputWriter.Clear();
            string commandResult = CommandExecutor.Execute(commandString);
            IEnumerable<string> result = _commandOutputWriter.Append(commandResult);
            return result;
        }

        internal IEnumerable<string> GetLastCommandOutput() => _commandOutputWriter;

        internal void UpdateObservables()
        {
            FixtureManager.UpdateObservers();
            SceneManager.UpdateObservers();
            DmxConnection.UpdateObservers();
        }

        // Chasers

        internal void SetChaser(string name, string kindName, int stepDurationInMilliseconds, string[] sceneNames)
        {
            if (!Enum.TryParse<ChaserKind>(kindName, out ChaserKind kind))
            {
                throw new ArgumentOutOfRangeException($"Value of {nameof(kindName)} is invalid: {kindName}.");
            }
            else
            {
                ChaserManager.SetChaser(name, kind, TimeSpan.FromMilliseconds(stepDurationInMilliseconds), sceneNames);
            }
        }

        internal void StartChaser(string name) => ChaserManager.StartPlaying(name, SceneManager);

        internal void StopChaser(string name) => ChaserManager.StopPlaying(name);
    }
}
