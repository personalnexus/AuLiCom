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

        // IObservable

        private readonly Observers<IChaserManager> _observers = new();
        public IDisposable Subscribe(IObserver<IChaserManager> observer) => _observers.Subscribe(observer);
        
        public int Version => _observers.Version;

        public void UpdateObservers() => _observers.OnNext(this);
    }
}
