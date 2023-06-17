using AuLiComLib.Common;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Protocols
{
    public static class SerialPorts
    {
        public static Dictionary<string, ISerialPort> GetDmxPortsByName() => 
            GetDmxPorts()
            .ToDictionary(x => x.PortName, x => x);

        public static IEnumerable<ISerialPort> GetDmxPorts() => 
            SerialPort
            .GetPortNames()
            .Select(CreateDmxPort)
            .Where(IsNotEmpty);

        public static ISerialPort CreateDmxPort(string portName)
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
                result = Empty;
            }

            return result;
        }

        public static bool IsNotEmpty(ISerialPort port) => !ReferenceEquals(port, Empty);

        private static readonly ISerialPort Empty = new EmptySerialPort();
    }
}
