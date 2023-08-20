using AuLiComLib.Colors;
using AuLiComLib.Colors.Channels;
using AuLiComLib.Protocols;

namespace AuLiComLib.CommandExecutor
{
    public interface ICommandColorChannelValueProperties
    {
        ChannelValue Red { get; }
        ChannelValue Green { get; }
        ChannelValue Blue { get; }
    }
}
