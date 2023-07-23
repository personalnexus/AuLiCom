using AuLiComLib.Common;
using AuLiComLib.Protocols;
using System.Xml.Linq;

namespace AuLiComLib.Scenes
{
    public interface INamedSceneManager: ISceneManager, IObservableEx<INamedSceneManager>
    {
        IReadOnlyDictionary<string, IScene> ScenesByName { get; }

        void RemoveScene(string name);

        void ActivateSingleScene(string name, TimeSpan fadeTime);

        void DeactivateAllScenes();
    }
}