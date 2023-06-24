using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComXL
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ProgId("ComAddin.FunctionLibrary")]
    public class AuLiComLink
    {
        // TODO: design decision: should it be possible to have entirely separate runtimes in COM
        // or is this class just a way to control the runtime that the Excel functions use?

        public string Initialize(string portName) => ExcelRuntime.Initialize(portName);

        public string InitializeWithOnlyDmxPort => ExcelRuntime.InitializeWithOnlyDmxPort();

        public void SetSingleActiveScene(string name, double fadeTimeInSeconds) => 
            ExcelRuntime
            .GetInstance()
            .SceneManager
            .SetSingleActiveScene(name, TimeSpan.FromSeconds(fadeTimeInSeconds));
    }
}
