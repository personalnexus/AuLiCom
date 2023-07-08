using AuLiComLib.Common;
using AuLiComLib.Protocols;

namespace AuLiComLib.Scenes
{
    public interface ISceneManager
    {
        IScene SetSceneFromCurrentUniverse(string name);
        IScene SetScene(string name, IReadOnlyUniverse universe);

        void ActivateScene(IScene newScene, TimeSpan fadeTime);
        void DeactivateScene(IScene newScene, TimeSpan fadeTime);

        void SetSingleActiveScene(IScene newScene, TimeSpan fadeTime);
    }
}