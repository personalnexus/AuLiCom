using AuLiComLib.Common;
using ExcelDna.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComXL
{
    internal class ExcelVersionObservable : IExcelObservable
    {
       public ExcelVersionObservable(Func<IExcelObserver, Action> subscribeAndMakeUnsubscriber) => 
            _processSubscribe = (observer) =>
            {
                Action unsubscriber = subscribeAndMakeUnsubscriber(observer);
                return new ActionDisposable(unsubscriber);
            };

        private readonly Func<IExcelObserver, IDisposable> _processSubscribe;

        public IDisposable Subscribe(IExcelObserver observer) => _processSubscribe(observer);
    }
}
