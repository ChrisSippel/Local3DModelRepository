using System;
using System.Collections.Generic;

namespace Local3DModelRepository.ExtensionMethods
{
    public static class EnumerableExtensionMethods
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }
    }
}