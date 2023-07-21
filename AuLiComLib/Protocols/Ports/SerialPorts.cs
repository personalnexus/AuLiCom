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
        public static readonly ISerialPort Empty = new EmptySerialPort();

        public const string EmptyPortName = "Empty";

        public static bool IsNotEmpty(ISerialPort port) => !ReferenceEquals(port, SerialPorts.Empty);

        private class EmptySerialPort : ISerialPort
        {
            public void Dispose() { }

            public bool IsOpen { get; private set; }

            public bool BreakState { get; set; }

            public string PortName => EmptyPortName;

            public void Close() => IsOpen = false;

            public void Open() => IsOpen = true;

            public void Write(byte[] values, int v, int valuesLength) { }
        }
    }
}
