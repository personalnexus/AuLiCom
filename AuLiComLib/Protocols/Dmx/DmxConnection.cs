using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuLiComLib.Common;
using AuLiComLib.Fixtures;

namespace AuLiComLib.Protocols.Dmx
{
    public class DmxConnection : IConnection
    {
        public DmxConnection(ISerialPort port,
                             IAsyncExecutor executor)
        {
            CurrentUniverse = Universe.CreateEmptyReadOnly();
            _port = port;
            _cancellationTokenSource = new CancellationTokenSource();
            _sendLoopQueue = new BlockingCollection<IReadOnlyUniverse>(boundedCapacity: 2); // TODO: extract to configuration
            
            // Start sender loop only after everything else has been initialized
            executor.ExecuteAsync(SendLoop);
        }

        public void Dispose() => _cancellationTokenSource.Cancel();

        private readonly ISerialPort _port;
        private readonly CancellationTokenSource _cancellationTokenSource;
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
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                Send(universeToSend);
                //
                // Wait for a while if there is a new universe to send,
                // otherwise we resend the old one
                //
                if (_sendLoopQueue.TryTake(out IReadOnlyUniverse newUniverseToSend,
                                           millisecondsTimeout: 100, // TODO: extract to configuration
                                           cancellationToken: _cancellationTokenSource.Token))
                {
                    universeToSend = newUniverseToSend;
                }
            }
            //
            // Also send an empty universe before we exit
            //
            Send(Universe.CreateEmptyReadOnly());
            _port.Close();
        }

        private void Send(IReadOnlyUniverse universeToSend)
        {
            _port.BreakState = true;
            Thread.Sleep(TimeSpan.FromMilliseconds(1));
            _port.BreakState = false;
            universeToSend.WriteValuesTo(_port);
        }

        public IReadOnlyUniverse CurrentUniverse { get; private set; }

        public void SendUniverse(IReadOnlyUniverse universe)
        {
            _sendLoopQueue.Add(universe, _cancellationTokenSource.Token);
            CurrentUniverse = universe;
            _observers.OnNext(this);
        }

        // IObservable

        private readonly Observers<IConnection> _observers = new();
        public IDisposable Subscribe(IObserver<IConnection> observer) => _observers.Subscribe(observer);

        public int Version => _observers.Version;

        public void UpdateObservers() => _observers.OnNext(this);
    }
}
