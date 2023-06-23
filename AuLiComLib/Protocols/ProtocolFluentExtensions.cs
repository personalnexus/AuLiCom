using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Protocols
{
    /// <summary>
    /// Helper methods to enable a fluent syntax for <see cref="IConnection"/>, <see cref="IReadOnlyUniverse"/> and <see cref="IMutableUniverse"/>.
    /// </summary>
    public static class ProtocolFluentExtensions
    {
        public static IReadOnlyUniverse ToReadOnlyUniverse(this IEnumerable<ChannelValue> channelValues) =>
           new MutableUniverse(channelValues).AsReadOnly();

        public static IMutableUniverse ToMutableUniverse(this IReadOnlyUniverse source) =>
           new MutableUniverse(source);

        public static ChannelValue GetValue(this IConnection connection, int channel) =>
            connection
            .CurrentUniverse.
            GetValue(channel);

        public static void SetValue(this IConnection connection, ChannelValue channelValue) => 
            connection
            .CurrentUniverse
            .ToMutableUniverse()
            .SetValue(channelValue)
            .AsReadOnly()
            .SendTo(connection);

        public static void SetValuesToZero(this IConnection connection) => 
            Universe
            .CreateEmptyReadOnly()
            .SendTo(connection);

        public static void SendTo(this IReadOnlyUniverse universe, IConnection connection) => 
            connection
            .SendUniverse(universe);
    }
}
