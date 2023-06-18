using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Protocols
{
    public class InvalidChannelValueException: Exception
    {
        public InvalidChannelValueException(string message) : base(message)
        {
        }

        internal static void ThrowIfOutOfRange(int channel, int start, int count)
        {
            if (channel < start && channel > (start + count))
            {
                throw new InvalidChannelValueException($"Channel has to between {start} and {start + count}, not {channel}");
            }
        }
    }
}
