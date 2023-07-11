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
        public static object Observe<T>(this IObservable<T> observable, [CallerMemberName] string callerFunctionName = null) =>
            Observe<T>(observable, null, callerFunctionName);

        public static object Observe<T>(this IObservable<T> observable, object? callerParameters, [CallerMemberName] string callerFunctionName = null) =>
            ExcelAsyncUtil.Observe(callerFunctionName: callerFunctionName,
                                   callerParameters: callerParameters,
                                   observableSource: () => new ExcelObservable<T>(observable));
    }
}
