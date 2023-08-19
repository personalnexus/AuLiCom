using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Colors.Channels
{
    public class ColorChannelValueProperties
    {
        public ColorChannelValueProperties(RedChannelValueProperty red,
                                           GreenChannelValueProperty green,
                                           BlueChannelValueProperty blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public RedChannelValueProperty Red { get; }
        public GreenChannelValueProperty Green { get; }
        public BlueChannelValueProperty Blue { get; }

        public void SetColor(IColor color)
        {
            Red.Value = color.Red;
            Green.Value = color.Green;
            Blue.Value = color.Blue;
        }
    }
}
