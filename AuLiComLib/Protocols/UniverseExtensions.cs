using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Protocols
{
    public static class UniverseExtensions
    {
        public static IReadOnlyUniverse ToReadOnlyUniverse(this IEnumerable<ChannelValue> channelValues) => new Universe(channelValues);

        public static IMutableUniverse SetValue(this IMutableUniverse universe, int channel, byte value) => universe.SetValue(ChannelValue.FromByte(channel, value));
    }
}
