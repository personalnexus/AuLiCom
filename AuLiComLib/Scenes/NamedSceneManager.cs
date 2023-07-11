using AuLiComLib.Common;
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
            if (!_scenesByName.TryGetValue(name, out IScene oldScene))
            {
                // This is an entirely new scene
                _scenesByName[name] = newScene;
                _observers.OnNext(this);
            }
            else
            {
                // This was an existing scene, but with updated values
                if (!oldScene.Universe.HasSameValuesAs(newScene.Universe))
                {
                    if (IsActiveScene(oldScene))
                    {
                        DeactivateScene(oldScene, fadeTime: TimeSpan.Zero);
                        ActivateScene(newScene, fadeTime: TimeSpan.Zero);
                    }
                    _scenesByName[name] = newScene;
                    _observers.OnNext(this);
                }
            }
            return newScene;
        }

        public void RemoveScene(string name) => _scenesByName.Remove(name);

        public void ActivateSingleScene(string name, TimeSpan fadeTime) => ActivateSingleScene(_scenesByName[name], fadeTime);

        public IReadOnlyDictionary<string, IScene> ScenesByName => _scenesByName;

        // IObservable

        private readonly Observers<INamedSceneManager> _observers = new();
        public IDisposable Subscribe(IObserver<INamedSceneManager> observer) => _observers.Subscribe(observer);

        public int Version => _observers.Version;
    }
}
