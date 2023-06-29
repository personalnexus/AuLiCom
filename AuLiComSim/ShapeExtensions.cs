using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AuLiComSim
{
    internal static class ShapeExtensions
    {
        public static void FillWithRgbFrom(this Shape shape, IReadOnlyUniverse universe, int startingAtChannel) =>
            shape.Fill = new SolidColorBrush(Color.FromRgb(r: universe.GetValue(startingAtChannel + 0).Value,
                                                           g: universe.GetValue(startingAtChannel + 1).Value,
                                                           b: universe.GetValue(startingAtChannel + 2).Value));
    }
}
