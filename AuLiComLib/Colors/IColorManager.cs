using AuLiComLib.Common;

namespace AuLiComLib.Colors
{
    public interface IColorManager: IObservableEx<IColorManager>
    {
        IReadOnlyDictionary<string, IColor> ColorsByName { get; }
        IColor SetColor(string name, byte red, byte green, byte blue);
    }
}