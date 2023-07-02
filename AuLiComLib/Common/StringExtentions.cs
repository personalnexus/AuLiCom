using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Common
{
    internal static class StringExtentions
    {
        public static bool TrySplitIn(this string input, int partCount, char separator, out string[] parts)
        {
            parts = input.Split(separator);
            return parts.Length == partCount;
        }
    }
}
