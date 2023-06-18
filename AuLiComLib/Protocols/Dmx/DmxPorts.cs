using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Protocols.Dmx
{
    public static class DmxPorts
    {
        public static Dictionary<string, ISerialPort> GetPortsByName() =>
            SerialPort
            .GetPortNames()
            .Select(CreatePort)
            .Where(SerialPorts.IsNotEmpty)
            .ToDictionary(x => x.PortName, x => x);

        private static ISerialPort CreatePort(string portName)
        {
            ISerialPort result = new SystemSerialPort(portName)
            {
                BaudRate = 250000,
                DataBits = 8,
                Handshake = Handshake.None,
                Parity = Parity.None,
                StopBits = StopBits.Two
            };

            try
            {
                // This throws an exception if the port is not compatible with the DMX settings, i.e. is not a DMX port
                result.Open();
                result.Close();
            }
            catch
            {
                result = SerialPorts.Empty;
            }

            return result;
        }
    }
}
