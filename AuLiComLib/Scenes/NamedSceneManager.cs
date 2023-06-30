using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Scenes
{
    public class NamedSceneManager : SceneManager, INamedSceneManager
    {
        public NamedSceneManager(IConnection connection) : base(connection)
        {
            _scenesByName = new Dictionary<string, IScene>();
        }

        private readonly Dictionary<string, IScene> _scenesByName;

        public override IScene SetScene(string name, IReadOnlyUniverse universe)
        {
            IScene newScene = base.SetScene(name, universe);
            if (_scenesByName.Remove(name, out IScene? oldScene)
                && oldScene != null
                && IsActiveScene(oldScene))
            {
                DeactivateScene(oldScene, fadeTime: TimeSpan.Zero);
                ActivateScene(newScene, fadeTime: TimeSpan.Zero);
            }
            _scenesByName.Add(name, newScene);
            return newScene;
        }

        public void RemoveScene(string name) => _scenesByName.Remove(name);

        public void SetSingleActiveScene(string name, TimeSpan fadeTime) => SetSingleActiveScene(_scenesByName[name], fadeTime);

        public IReadOnlyDictionary<string, IScene> ScenesByName => _scenesByName;
    }
}
