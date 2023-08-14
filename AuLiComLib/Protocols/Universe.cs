using AuLiComLib.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Protocols
{
    public class Universe
    {
        public const int ChannelCount = 512;
        internal const int MinChannel = 1;
        internal const int MaxChannel = ChannelCount;

        protected const int FirstChannel = 1;
        protected const int ValuesLength = FirstChannel + ChannelCount;

        //
        // Factory methods. Making the constructor protected prevents outside code from circumventing our factory method.
        //

        protected Universe(): base()
        {
        }

        public static IMutableUniverse CreateEmpty() => new MutableUniverse();

        public static IReadOnlyUniverse CreateEmptyReadOnly() => new ReadOnlyUniverse(CreateEmptyValues());

        //
        // Helper functions for internal use only
        //

        protected static byte[] CreateEmptyValues() => new byte[Universe.ValuesLength];

        internal static void ThrowIfInvalidChannel(int channel)
        {
            if (channel is < MinChannel or > MaxChannel)
            {
                throw new ArgumentOutOfRangeException($"Channel has to be between {MinChannel} and {MaxChannel}.");
            }
        }
    }
}
