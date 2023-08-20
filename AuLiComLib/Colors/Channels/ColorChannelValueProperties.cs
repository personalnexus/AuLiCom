using AuLiComLib.CommandExecutor;
using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Colors.Channels
{
    public class ColorChannelValueProperties : ICommandColorChannelValueProperties
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

        public bool ContainsChannel(int channel) =>
            Red.Channel == channel
            || Green.Channel == channel
            || Blue.Channel == channel;


        // ICommandColorChannelValueProperties

        ChannelValue ICommandColorChannelValueProperties.Red => Red.ChannelValue;

        ChannelValue ICommandColorChannelValueProperties.Green => Green.ChannelValue;

        ChannelValue ICommandColorChannelValueProperties.Blue => Blue.ChannelValue;
    }
}
