using AuLiComLib.Common;
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
    [ProgId("AuLiCom.AuLiComLink")]
    public class AuLiComLink
    {
        // TODO: design decision: should it be possible to have entirely separate runtimes in COM
        // or is this class just a way to control the runtime that the Excel functions use?

        public string Initialize(string portName) => 
            ExcelRuntime
            .Initialize(portName)
            .ToDelimitedString(Environment.NewLine);

        public string InitializeWithOnlyDmxPort => 
            ExcelRuntime
            .InitializeWithOnlyDmxPort()
            .ToDelimitedString(Environment.NewLine);

        public void UpdateObservables() =>
            ExcelRuntime
            .GetInstance()
            .UpdateObservables();

        // Scenes

        public int GetSceneCount() =>
            ExcelRuntime
            .GetInstance()
            .SceneManager
            .ScenesByName
            .Count;

        public void SetSingleActiveScene(string name, double fadeTimeInSeconds) =>
            ExcelRuntime
            .GetInstance()
            .SceneManager
            .ActivateSingleScene(name, TimeSpan.FromSeconds(fadeTimeInSeconds));

        public void DeactivateAllScenes() =>
            ExcelRuntime
            .GetInstance()
            .SceneManager
            .DeactivateAllScenes();

        public void CreateSceneFromCurrentUniverse(string name) =>
            ExcelRuntime
            .GetInstance()
            .SceneManager
            .SetSceneFromCurrentUniverse(name);


        // Chasers

        public int GetChaserCount() =>
            ExcelRuntime
            .GetInstance()
            .ChaserManager
            .ChasersByName
            .Count;

        public void StartChaser(string name) =>
            ExcelRuntime
            .GetInstance()
            .StartChaser(name);

        public void StopChaser(string name) =>
            ExcelRuntime
            .GetInstance()
            .StopChaser(name);
    }
}
