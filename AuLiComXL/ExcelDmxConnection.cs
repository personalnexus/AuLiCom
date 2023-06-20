using AuLiComLib.Common;
using AuLiComLib.Protocols;
using AuLiComLib.Protocols.Dmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComXL
{
    internal class ExcelDmxConnection: IDisposable
    {
        public ExcelDmxConnection(ISerialPort port)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            try
            {
                PortName = port.PortName;
                var executor = new SystemThread(PortName);
                Status = $"Connected to {PortName}";
                DmxConnection = new DmxConnection(port, executor, _cancellationTokenSource.Token);
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

        internal static string EnsureInitialized(ref ExcelDmxConnection? connection, 
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
                    connection = new ExcelDmxConnection(port);
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
    }
}
