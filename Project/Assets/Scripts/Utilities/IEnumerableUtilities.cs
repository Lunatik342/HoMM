using System;
using System.Collections.Generic;

namespace Utilities
{
    public static class IEnumerableUtilities
    {
        public static void Foreach<T>(this IEnumerable<T> enumerableCollection, Action<T> action)
        {
            foreach (var enumerable in enumerableCollection)
            {
                action(enumerable);
            }
        }
    }
}