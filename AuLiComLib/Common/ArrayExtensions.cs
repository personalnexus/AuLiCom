using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Common
{
    public static class ArrayExtensions
    {
        public static byte[] BlockCopy(this byte[] source)
        {
            var result = new byte[source.Length];
            Buffer.BlockCopy(source, 0, result, 0, source.Length);
            return result;
        }

        public static int[] ToIntegerArray(this double[] values) => values.Select(x => (int)x).ToArray();
    }
}
