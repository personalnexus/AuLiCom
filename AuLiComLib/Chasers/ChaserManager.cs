using AuLiComLib.Common;
using AuLiComLib.Protocols;
using AuLiComLib.Scenes;
using System;
using System.Collections.Generic;
using System.Data.Common;
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
            _chasersByName = new Dictionary<string, IChaser>(StringComparer.CurrentCultureIgnoreCase);
        }

        private readonly IConnection _connection;
        private readonly Dictionary<string, IChaser> _chasersByName;

        public IReadOnlyDictionary<string, IChaser> ChasersByName => _chasersByName;

        public IChaser SetChaser(string name, 
                                 ChaserKind kind, 
                                 TimeSpan stepDuration, 
                                 string[] stepNames)
        {
            var newChaser = new Chaser(name, kind, stepDuration, stepNames);
            if (!_chasersByName.TryGetValue(name, out IChaser oldChaser))
            {
                // This is an entirely new chaser
                _chasersByName[name] = newChaser;
                _observers.OnNext(this);
            }
            else
            {
                // This was an existing chaser, but with updated values
                if (!oldChaser.StepNames.SequenceEqual(newChaser.StepNames) 
                    || oldChaser.StepDuration != newChaser.StepDuration
                    || oldChaser.Kind != newChaser.Kind)
                {
                    oldChaser.StopPlaying();
                    _chasersByName[name] = newChaser;
                    _observers.OnNext(this);
                }
            }   
            return newChaser;
        }

        public async Task StartPlaying(string name, IReadOnlyUniverseProvider universeProvider) =>
            await 
            _chasersByName[name]
            .StartPlaying(_connection, universeProvider);

        public void StopPlaying(string name) =>
            _chasersByName[name]
            .StopPlaying();


        // IObservable

        private readonly Observers<IChaserManager> _observers = new();

        public IDisposable Subscribe(IObserver<IChaserManager> observer) => _observers.Subscribe(observer);
        
        public int Version => _observers.Version;

        public void UpdateObservers() => _observers.OnNext(this);
    }
}
