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
        public static bool TrySplitInTwo(this string input, char separator, out string part1, out string part2)
        {
            var parts = input.Split(new[] { separator }, 2);
            bool result = parts.Length == 2;
            if (result)
            {
                part1 = parts[0];
                part2 = parts[1];
            }
            else
            {
                part1 = default;
                part2 = default;
            }
            return result;
        }
    }
}
