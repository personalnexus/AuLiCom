using AuLiComLib.Common;
using ExcelDna.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComXL
{
    internal static class ExcelObservableExtensions
    {
        public static object Observe<T>(this IObservable<T> observable, object callerParameters, [CallerMemberName] string callerFunctionName = null) =>
            ExcelAsyncUtil.Observe(callerFunctionName: callerFunctionName,
                                   callerParameters: callerParameters,
                                   observableSource: () => new ExcelObservable<T>(observable));

        public static object ObserveVersion(this IVersioned versioned, [CallerMemberName] string callerFunctionName = null) =>
            ExcelAsyncUtil.Observe(callerFunctionName: callerFunctionName,
                                   callerParameters: null,
                                   observableSource: () =>
                                       new ExcelVersionObservable(observer =>
                                       {
                                           var eventHandler = new EventHandler<VersionChangedEventArgs>((sender, EventArgs) => observer.OnNext(versioned.Version));
                                           versioned.VersionChanged += eventHandler;
                                           return () => versioned.VersionChanged -= eventHandler;
                                       })
                                   );
    }
}
