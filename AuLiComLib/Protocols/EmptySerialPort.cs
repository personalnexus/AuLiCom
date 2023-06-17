namespace AuLiComLib.Protocols
{
    internal class EmptySerialPort : ISerialPort
    {
        public void Dispose() { }

        public bool IsOpen { get; private set; }

        public bool BreakState { get; set; }

        public string PortName => "Empty";

        public void Close() => IsOpen = false;

        public void Open() => IsOpen = true;

        public void Write(byte[] values, int v, int valuesLength) { }
    }
}
