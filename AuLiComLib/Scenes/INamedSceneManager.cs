using AuLiComLib.Common;
using AuLiComLib.Protocols;
using System.Xml.Linq;

namespace AuLiComLib.Scenes
{
    public interface INamedSceneManager: ISceneManager, IVersioned
    {
        IReadOnlyDictionary<string, IScene> ScenesByName { get; }

        void RemoveScene(string name);

        void SetSingleActiveScene(string name, TimeSpan fadeTime);
    }
}