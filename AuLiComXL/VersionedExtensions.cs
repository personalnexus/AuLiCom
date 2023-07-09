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
    internal static class VersionedExtensions
    {
        public static object ObserveVersion(this IVersioned versioned, [CallerMemberName] string functionName = null) =>

                ExcelAsyncUtil.Observe(callerFunctionName: functionName,
                                       callerParameters: null,
                                       observableSource: () =>
                                           new ExcelObservable(observer =>
                                           {
                                               var eventHandler = new EventHandler<VersionChangedEventArgs>((sender, EventArgs) => observer.OnNext(versioned.Version));
                                               versioned.VersionChanged += eventHandler;
                                               return () => versioned.VersionChanged -= eventHandler;
                                           })
                                       );
    }
}
