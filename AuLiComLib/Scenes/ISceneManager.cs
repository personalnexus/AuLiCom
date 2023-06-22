using AuLiComLib.Protocols;

namespace AuLiComLib.Scenes
{
    public interface ISceneManager
    {
        IScene CreateSceneFromCurrentUniverse(string name);
        IScene CreateScene(string name, IReadOnlyUniverse universe);

        void ActivateScene(IScene newScene, TimeSpan fadeTime);
        void DeactivateScene(IScene newScene, TimeSpan fadeTime);

        void SetSingleActiveScene(IScene newScene, TimeSpan fadeTime);
    }
}