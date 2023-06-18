using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuLiComLib.Common;

namespace AuLiComLib.Protocols.Dmx
{
    public class DmxConnection : IConnection
    {
        public DmxConnection(ISerialPort port,
                             IAsyncExecutor executor,
                             CancellationToken cancellationToken)
        {
            _port = port;
            _values = new byte[ValuesLength];
            _cancellationToken = cancellationToken;
            _valuesChanged = new WaitEvent();

            // Start sender loop only after everything else has been initialized
            executor.ExecuteAsync(SendLoop);
        }

        private const int FirstChannel = 1;
        private const int ValuesLength = FirstChannel + 512;

        private readonly ISerialPort _port;
        private readonly CancellationToken _cancellationToken;

        private readonly WaitEvent _valuesChanged;
        private byte[] _values; // must be mutable, because it is replaced in one atomic action so no intermediary channel values are sent

        private void SendLoop()
        {
            if (!_port.IsOpen)
            {
                _port.Open();
            }
            while (!_cancellationToken.IsCancellationRequested)
            {
                _port.BreakState = true;
                Thread.Sleep(TimeSpan.FromMilliseconds(1));
                _port.BreakState = false;
                _port.Write(_values, 0, ValuesLength);

                // No need to resend too often, unless something changes
                _valuesChanged.Wait(TimeSpan.FromSeconds(1));
            }
            _port.Close();
        }

        // IDmxConnection

        public void SetValue(ChannelValue channelValue)
        {
            var channelValues = new Span<ChannelValue>(ref channelValue);
            SetValues(channelValues);
        }

        public void SetValues(ReadOnlySpan<ChannelValue> channelValues)
        {
            var newValues = new byte[ValuesLength];

            Array.Copy(_values, newValues, ValuesLength);
            foreach (ChannelValue channelValue in channelValues)
            {
                newValues[channelValue.Channel] = channelValue.Value;
            }

            SetValues(newValues);
        }

        public void SetValuesToZero()
        {
            var newValues = new byte[ValuesLength];
            SetValues(newValues);
        }

        private void SetValues(byte[] newValues)
        {
            // Do not modify _values, but exchange in one atomic operation
            Interlocked.Exchange(ref _values, newValues);
            _valuesChanged.Set();
        }

        public IEnumerable<ChannelValue> GetValues()
        {
            for (int channel = FirstChannel; channel < ValuesLength; channel++)
            {
                yield return ChannelValue.FromByte(channel, _values[channel]);
            }
        }
    }
}
