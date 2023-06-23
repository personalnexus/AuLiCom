using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Scenes
{
    public class SceneManager : ISceneManager
    {
        public SceneManager(IConnection connection)
        {
            _connection = connection;
            _activeScenes = new HashSet<Scene>();
        }

        private readonly IConnection _connection;
        private readonly HashSet<Scene> _activeScenes;

        public IScene CreateSceneFromCurrentUniverse(string name) => CreateScene(name, _connection.CurrentUniverse);

        public IScene CreateScene(string name, IReadOnlyUniverse universe)
        {
            var result = new Scene(name, universe);
            return result;
        }

        public void ActivateScene(IScene newScene, TimeSpan fadeTime)
        {
            if ((newScene is Scene sceneObject) && _activeScenes.Add(sceneObject))
            {
                ProcessActiveScenesChange(fadeTime);
            }
        }

        public void DeactivateScene(IScene newScene, TimeSpan fadeTime)
        {
            if ((newScene is Scene sceneObject) && _activeScenes.Remove(sceneObject))
            {
                ProcessActiveScenesChange(fadeTime);
            }
        }

        public void SetSingleActiveScene(IScene newScene, TimeSpan fadeTime)
        {
            if (newScene is Scene sceneObject)
            {
                _activeScenes.Clear();
                _activeScenes.Add(sceneObject);
                ProcessActiveScenesChange(fadeTime);
            }
        }

        private void ProcessActiveScenesChange(TimeSpan fadeTime)
        {
            //
            // If there is only one scene, that is our target universe
            //
            IReadOnlyUniverse targetUniverse = _activeScenes.Count == 1 
                ? _activeScenes.First().Universe 
                : CombineActiveScenesIntoTargetUniverse();
            var changes = new ChannelValueChanges(_connection, targetUniverse, fadeTime);
            changes.Apply();
        }

        private IReadOnlyUniverse CombineActiveScenesIntoTargetUniverse()
        {
            IMutableUniverse result = Universe.CreateEmpty();
            foreach (Scene scene in _activeScenes)
            {
                result.CombineWith(scene.Universe, aggregatingChannelValuesWith: ChannelValue.Max);
            }
            return result.AsReadOnly();
        }
    }
}
