using AuLiComLib.Chasers;
using AuLiComLib.CommandExecutor;
using AuLiComLib.Common;
using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Colors
{
    public class ColorManager: IColorManager, ICommandColors
    {
        public ColorManager()
        {
            _colorsByName = new Dictionary<string, IColor>(StringComparer.CurrentCultureIgnoreCase);
        }

        private readonly Dictionary<string, IColor> _colorsByName;

        public IReadOnlyDictionary<string, IColor> ColorsByName => _colorsByName;

        public IColor SetColor(string name,
                               byte red,
                               byte green,
                               byte blue)
        {
            var newColor = new Color(name, red, green, blue);
            if (!_colorsByName.TryGetValue(name, out IColor oldColor))
            {
                // This is an entirely new color
                _colorsByName[name] = newColor;
                _observers.OnNext(this);
            }
            else
            {
                // This was an existing chaser, but with updated values
                if (oldColor.Red != red
                    || oldColor.Green != green
                    || oldColor.Blue != blue)
                {
                    _colorsByName[name] = newColor;
                    _observers.OnNext(this);
                }
            }
            return newColor;
        }

        // ICommandColors

        public bool TryGetColorByName(string name, out IColor color) => _colorsByName.TryGetValue(name, out color);


        // IObservable

        private readonly Observers<IColorManager> _observers = new();

        public IDisposable Subscribe(IObserver<IColorManager> observer) => _observers.Subscribe(observer);

        public int Version => _observers.Version;

        public void UpdateObservers() => _observers.OnNext(this);
    }
}
