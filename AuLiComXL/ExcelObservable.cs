using ExcelDna.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComXL
{
    internal class ExcelObservable<T> : IExcelObservable
    {
        public ExcelObservable(IObservable<T> observable)
        {
            _observable = observable;
        }

        private readonly IObservable<T> _observable;

        public IDisposable Subscribe(IExcelObserver observer) =>
            _observable.Subscribe(onNext: x => observer.OnNext(x),
                                  onCompleted: observer.OnCompleted,
                                  onError: observer.OnError);
    }
}
