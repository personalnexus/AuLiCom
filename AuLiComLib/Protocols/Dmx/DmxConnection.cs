using System;
using System.Collections.Concurrent;
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
            CurrentUniverse = Universe.CreateEmptyReadOnly();
            _port = port;
            _cancellationToken = cancellationToken;
            _sendLoopQueue = new BlockingCollection<IReadOnlyUniverse>(boundedCapacity: 2); // TODO: extract to configuration

            // Start sender loop only after everything else has been initialized
            executor.ExecuteAsync(SendLoop);
        }

        private readonly ISerialPort _port;
        private readonly CancellationToken _cancellationToken;
        private readonly BlockingCollection<IReadOnlyUniverse> _sendLoopQueue;
        
        private void SendLoop()
        {
            if (!_port.IsOpen)
            {
                _port.Open();
            }
            //
            // Start out by sending an empty universe
            //
            IReadOnlyUniverse universeToSend = Universe.CreateEmptyReadOnly();
            while (!_cancellationToken.IsCancellationRequested)
            {
                _port.BreakState = true;
                Thread.Sleep(TimeSpan.FromMilliseconds(1));
                _port.BreakState = false;
                universeToSend.WriteValuesTo(_port);
                //
                // Wait for a while if there is a new universe to send,
                // otherwise we resend the old one
                //
                if (_sendLoopQueue.TryTake(out IReadOnlyUniverse newUniverseToSend,
                                           millisecondsTimeout: 1000, // TODO: extract to configuration
                                           cancellationToken: _cancellationToken))
                {
                    universeToSend = newUniverseToSend;
                }

            }
            _port.Close();
        }

        public IReadOnlyUniverse CurrentUniverse { get; private set; }

        public void SendUniverse(IReadOnlyUniverse universe)
        {
            _sendLoopQueue.Add(universe, _cancellationToken);
            CurrentUniverse = universe;
        }
    }
}
