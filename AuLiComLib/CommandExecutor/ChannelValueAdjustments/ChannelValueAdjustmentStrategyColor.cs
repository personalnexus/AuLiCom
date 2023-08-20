using AuLiComLib.Colors;
using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.CommandExecutor.ChannelValueAdjustments
{
    internal class ChannelValueAdjustmentStrategyColor : IChannelValueAdjustmentStrategy
    {
        public ChannelValueAdjustmentStrategyColor(IColor color, ICommandColorFixtures fixtures)
        {
            _color = color;
            _fixtures = fixtures;
        }

        private readonly IColor _color;
        private readonly ICommandColorFixtures _fixtures;

        public void ApplyTo(ChannelValue source, IMutableUniverse target)
        {
            if (_fixtures.TryGetColorChannelValuePropertiesByChannel(source.Channel, out ICommandColorChannelValueProperties colorProperties))
            {
                target.SetValue(ChannelValue.FromByte(colorProperties.Red.Channel, _color.Red));
                target.SetValue(ChannelValue.FromByte(colorProperties.Green.Channel, _color.Green));
                target.SetValue(ChannelValue.FromByte(colorProperties.Blue.Channel, _color.Blue));
            }
            // Ignore attempts trying to apply a color to a non-color channel, because we want to use this command to set the RGB channels
            // on devices that have additional channels via the "<DeviceName>@<ColorName>" syntax. The device name would resolve to all
            // channels, so everything but RGB needs to be ignored.
        }
    }
}
