using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Common
{
    public class Observers<T>
    {
        public Observers()
        {
            _observers = new();
        }

        private readonly List<IObserver<T>> _observers;

        public IDisposable Subscribe(IObserver<T> observer)
        {
            _observers.Add(observer);
            var self = this;
            return new ActionDisposable(() => self._observers.Remove(observer));
        }

        public void OnNext(T value)
        {
            Version++;
            _observers.ForEach(x => x.OnNext(value));
        }

        public int Version { get; private set; }
    }
}
