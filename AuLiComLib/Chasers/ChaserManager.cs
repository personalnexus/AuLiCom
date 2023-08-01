using AuLiComLib.Common;
using AuLiComLib.Protocols;
using AuLiComLib.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Chasers
{
    public class ChaserManager: IChaserManager
    {
        public ChaserManager(IConnection connection)
        {
            _connection = connection;
            _chasersByName = new Dictionary<string, IChaser>();
        }

        private readonly IConnection _connection;
        private readonly Dictionary<string, IChaser> _chasersByName;

        public IReadOnlyDictionary<string, IChaser> ChasersByName => _chasersByName;

        public IChaser SetChaser(string name, ChaserKind kind, IReadOnlyUniverseProvider[] steps)
        {
            var newChaser = new Chaser(name, kind, steps);
            if (!_chasersByName.TryGetValue(name, out IChaser oldChaser))
            {
                // This is an entirely new chaser
                _chasersByName[name] = newChaser;
                _observers.OnNext(this);
            }
            else
            {
                // This was an existing chaser, but with updated values
                if (oldChaser.Steps.Length != newChaser.Steps.Length ||
                    !oldChaser.Steps.SequenceEqual(newChaser.Steps, ReadOnlyUniverse.HasSameValuesComparer))
                {
                    _chasersByName[name] = newChaser;
                    _observers.OnNext(this);
                }
            }   
            return newChaser;
        }


        // IObservable

        private readonly Observers<IChaserManager> _observers = new();

        public IDisposable Subscribe(IObserver<IChaserManager> observer) => _observers.Subscribe(observer);
        
        public int Version => _observers.Version;

        public void UpdateObservers() => _observers.OnNext(this);
    }
}
