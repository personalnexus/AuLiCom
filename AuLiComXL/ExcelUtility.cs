using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComXL
{
    internal static class ExcelUtility
    {
        internal static object[,] ToVerticalRange<T>(this IEnumerable<T> items)
        {
            T[] array = items.ToArray();
            object[,] result = new object[array.Length, 1];
            for (int i = 0; i < array.Length; i++)
            {
                result[i, 0] = array[i];
            }

            return result;
        }
    }
}
