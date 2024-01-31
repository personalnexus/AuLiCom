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

        public static async void FadeTo(this IConnection connection, IReadOnlyUniverse targetUniverse, TimeSpan fadeTime) =>
            await new ChannelValueChanges(connection, targetUniverse, fadeTime)
            .Apply();

        public static async void FadeTo2(this IConnection connection, IReadOnlyUniverse targetUniverse, TimeSpan fadeTime) =>
            await new ChannelValueChanges2(connection, targetUniverse, fadeTime).Apply();

        public static ChannelValue GetValue(this IConnection connection, int channel) =>
            connection
            .CurrentUniverse.
            GetValue(channel);

        public static IEnumerable<string> GetChannelPercentages(this IReadOnlyUniverse universe)
        {
            for (int channel = Universe.MinChannel; channel < Universe.MaxChannel; channel++)
            {
                ChannelValue channelValue = universe.GetValue(channel);
                if (channelValue.Value > 0)
                {
                    yield return channelValue.ToPercentageString();
                }
            }
        }

        public static IEnumerable<ChannelValue> GetValues(this IConnection connection, int[] channels) =>
            channels
            .Select(x => connection
                         .CurrentUniverse
                         .GetValue(x));

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
