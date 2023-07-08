using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLiComLib.Common
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (T item in items)
            {
                action(item);
            }
        }

        public static string ToDelimitedString<T>(this IEnumerable<T> items, string delimiter) => string.Join(delimiter, items);

        public static IEnumerable<T> Order<T>(this IEnumerable<T> items) => items.OrderBy(x => x);

        public static void AddTo<T>(this IEnumerable<T> items, ICollection<T> collection) => items.ForEach(collection.Add);
    }
}
