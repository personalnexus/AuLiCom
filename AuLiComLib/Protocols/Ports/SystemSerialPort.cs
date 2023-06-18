using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Protocols
{
    /// <summary>
    /// Implement <see cref="ISerialPort"/> via <see cref="SerialPort"/> from System.IO.Ports
    /// </summary>
    internal class SystemSerialPort : SerialPort, ISerialPort
    {
        public SystemSerialPort(string portName) : base(portName) { }
    }
}
