using AuLiComLib.Common;

namespace AuLiComLib.Colors
{
    public interface IColorManager: IObservableEx<IColorManager>
    {
        IColor SetColor(string name, byte red, byte green, byte blue);
    }
}