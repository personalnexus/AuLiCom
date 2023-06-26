using ExcelDna.ComInterop;
using ExcelDna.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComXL
{
    [ComVisible(false)]
    class ExcelAddin : IExcelAddIn
    {
        public void AutoOpen()
        {
            ComServer.DllRegisterServer();
            ExcelRuntime.InitializeWithOnlyDmxPort();
            ExcelIntegration.RegisterUnhandledExceptionHandler(ProcessUnhandledException);
        }

        public void AutoClose()
        {
            ExcelRuntime.DisposeInstance();
            ComServer.DllUnregisterServer();
        }

        private object ProcessUnhandledException(object exceptionObject)
        {
            // catch any exception from an Excel function and return the exception message
            return $"#ERROR: {(exceptionObject as Exception)?.Message ?? exceptionObject}";
        }
    }
}
