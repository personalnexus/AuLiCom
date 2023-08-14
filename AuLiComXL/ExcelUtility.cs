using AuLiComLib.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComXL
{
    public static class ExcelUtility
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
        internal static object[,] ToHorizontalRange<T>(this IEnumerable<T> items)
        {
            T[] array = items.ToArray();
            object[,] result = new object[1, array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[0, i] = array[i];
            }
            return result;
        }

        public static T[,] To2dRange<T>(this IEnumerable<T[]> items)
        {
            T[,] result;
            T[][] arrays = items.ToArray();
            if (arrays.Length == 0)
            {
                result = new T[0, 0];
            }
            else
            {
                var horizontalLength = arrays[0].Length;

                result = new T[arrays.Length, horizontalLength];
                for (var i = 0; i < arrays.Length; i++)
                {
                    var array = arrays[i];
                    for (var j = 0; j < horizontalLength; j++)
                    {
                        result[i, j] = array[j];
                    }
                }
            }
            return result;
        }
    }
}
