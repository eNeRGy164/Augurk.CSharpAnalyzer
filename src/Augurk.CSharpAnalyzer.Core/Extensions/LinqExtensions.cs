using System;
using System.Collections.Generic;

namespace System.Linq
{
    internal static class LinqExtensions
    {
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> childrenSelector)
        {
            List<T> result = new List<T>();
            foreach (T item in source)
            {
                result.Add(item);
                result.AddRange(childrenSelector(item).Flatten(childrenSelector));
            }

            return result;
        }
    }
}
