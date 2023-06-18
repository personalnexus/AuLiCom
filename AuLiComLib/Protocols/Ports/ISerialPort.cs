using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Protocols
{
    public interface ISerialPort: IDisposable
    {
        bool IsOpen { get; }
        bool BreakState { get; set; }
        string PortName { get; }

        void Close();
        void Open();
        void Write(byte[] values, int start, int count);
    }
}
