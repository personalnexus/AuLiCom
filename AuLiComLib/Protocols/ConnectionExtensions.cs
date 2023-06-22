using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Protocols
{
    /// <summary>
    /// Helper methods to give method calls involving <see cref="IConnection"/> a more natural order
    /// </summary>
    public static class ConnectionExtensions
    {
        public static ChannelValue GetValue(this IConnection connection, int channel) => connection.CurrentUniverse.GetValue(channel);

        public static void SetValue(this IConnection connection, ChannelValue channelValue)
        {
            new Universe(connection.CurrentUniverse)
                .SetValue(channelValue)
                .SendTo(connection);

        }

        public static void SetValuesToZero(this IConnection connection)
        {
            new Universe()
                .SendTo(connection);
        }

        public static void SendValuesTo(this IEnumerable<ChannelValue> channelValues, IConnection connection)
        {
            new Universe(connection.CurrentUniverse)
                .SetValues(channelValues)
                .SendTo(connection);

        }

        public static void SendTo(this IReadOnlyUniverse universe, IConnection connection) => connection.SendUniverse(universe);
    }
}
