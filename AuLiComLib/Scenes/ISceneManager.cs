using AuLiComLib.Protocols;

namespace AuLiComLib.Scenes
{
    public interface ISceneManager
    {
        IScene CreateSceneFromCurrentValues(string name, double order);
        IScene CreateScene(string name, double order, IEnumerable<ChannelValue> channelValues);

        void ActivateScene(IScene newScene, TimeSpan fadeTime);
        void DeactivateScene(IScene newScene, TimeSpan fadeTime);

        void SetSingleActiveScene(IScene newScene, TimeSpan fadeTime);
    }
}