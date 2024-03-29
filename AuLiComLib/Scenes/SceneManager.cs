﻿using AuLiComLib.Protocols;

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

        // ISceneManager

        public IScene SetSceneFromCurrentUniverse(string name) => SetScene(name, _connection.CurrentUniverse);

        public virtual IScene SetScene(string name, IReadOnlyUniverse universe)
        {
            var result = new Scene(name, universe);
            return result;
        }

        public bool IsActiveScene(IScene scene) => _activeScenes.Contains(scene);

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

        public void ActivateSingleScene(IScene newScene, TimeSpan fadeTime)
        {
            if (newScene is Scene sceneObject)
            {
                _activeScenes.Clear();
                _activeScenes.Add(sceneObject);
                ProcessActiveScenesChange(fadeTime);
            }
        }

        public void DeactivateAllScenes()
        {
            _activeScenes.Clear();
            ProcessActiveScenesChange(TimeSpan.Zero);
        }

        private void ProcessActiveScenesChange(TimeSpan fadeTime)
        {
            //
            // If there is only one scene, that is our target universe
            //
            IReadOnlyUniverse targetUniverse = _activeScenes.Count == 1 
                ? _activeScenes.First().Universe 
                : CombineActiveScenesIntoTargetUniverse();
            //
            // Only fade if there is actually a change
            //
            if (!targetUniverse.HasSameValuesAs(_connection.CurrentUniverse))
            {
                _connection.FadeTo2(targetUniverse, fadeTime);
            }
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
