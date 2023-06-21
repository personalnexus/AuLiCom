using AuLiComLib.Common;
using AuLiComLib.Protocols;
using AuLiComLib.Protocols.Dmx;
using AuLiComLib.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComXL
{
    internal class ExcelRuntime: IDisposable
    {
        public ExcelRuntime(ISerialPort port)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            ScenesByName = new Dictionary<string, IScene>();
            try
            {
                PortName = port.PortName;
                var executor = new SystemThread(PortName);
                Status = $"Connected to {PortName}";
                DmxConnection = new DmxConnection(port, executor, _cancellationTokenSource.Token);
                SceneManager = new SceneManager(DmxConnection);
            }
            catch (Exception exception)
            {
                Status = exception.Message;
                PortName = null;
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }

        internal static string EnsureInitialized(ref ExcelRuntime? connection, 
                                                 string portName, 
                                                 Func<bool, Dictionary<string, ISerialPort>> getAvailableDmxPorts)
        {
            string result;
            if (connection?.PortName == portName)
            {
                // Keep using the existing connection
                result = connection.Status;
            }
            else
            {
                // Recreate connection
                connection?.Dispose();
                if (getAvailableDmxPorts(false).TryGetValue(portName, out ISerialPort? port))
                {
                    connection = new ExcelRuntime(port);
                    result = connection.Status;
                }
                else
                {
                    connection = null;
                    result = $"Invalid port {portName}";
                }
            }
            return result;
        }

        private readonly CancellationTokenSource _cancellationTokenSource;

        public string Status { get; }
        public IConnection? DmxConnection { get; }
        public string? PortName { get; }

        public ISceneManager? SceneManager { get; }
        public Dictionary<string, IScene> ScenesByName { get; }
    }
}
