using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XliffLib.Utils
{
    public static class ListExtensions
    {
        public static void AddAll<T>(this IList<T> list, IList<T> newValues)
        {
            foreach (var item in newValues)
            {
                list.Add(item);
            }
        }

        public static void AddAll<TKey, TValue>(this IDictionary<TKey, TValue> list, IDictionary<TKey, TValue> newValues)
        {
            foreach (var item in newValues)
            {
                list.Add(item);
            }
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue ret;
            // Ignore return value
            dictionary.TryGetValue(key, out ret);
            return ret;
        }

    }
}
