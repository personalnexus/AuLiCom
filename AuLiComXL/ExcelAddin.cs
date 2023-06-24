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
        public void AutoOpen() => ComServer.DllRegisterServer();
        public void AutoClose()
        {
            ComServer.DllUnregisterServer();
            ExcelRuntime.DisposeInstance();
        }
    }
}
