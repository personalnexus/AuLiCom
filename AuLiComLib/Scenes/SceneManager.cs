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

        public IScene CreateSceneFromCurrentValues(string name, double order) => CreateScene(name, order, _connection.GetValues());

        public IScene CreateScene(string name, double order, IEnumerable<ChannelValue> channelValues)
        {
            var result = new Scene(name, order, channelValues);
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
            byte[] targetChannelValues = GetTargetChannelValuesFromActiveScenes();
            var changes = new ChannelValueChanges(_connection, targetChannelValues, fadeTime);
            changes.Apply();
        }

        private byte[] GetTargetChannelValuesFromActiveScenes()
        {
            var targetChannelValues = new byte[513];
            foreach (Scene scene in _activeScenes)
            {
                for (int channel = 1; channel < targetChannelValues.Length; channel++)
                {
                    targetChannelValues[channel] = Math.Max(targetChannelValues[channel], scene.GetChannelValue(channel));
                }
            }
            return targetChannelValues;
        }
    }
}
