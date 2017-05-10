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
    }
}
