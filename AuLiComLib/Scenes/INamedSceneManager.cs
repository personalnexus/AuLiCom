using AuLiComLib.Protocols;
using System.Xml.Linq;

namespace AuLiComLib.Scenes
{
    public interface INamedSceneManager: ISceneManager
    {
        IReadOnlyDictionary<string, IScene> ScenesByName { get; }

        void RemoveScene(string name);

        void SetSingleActiveScene(string name, TimeSpan fadeTime);
    }
}