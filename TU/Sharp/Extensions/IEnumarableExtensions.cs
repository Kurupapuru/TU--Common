using System;
using System.Collections.Generic;
using System.Linq;

namespace Code.Extensions
{
    public static class IEnumarableExtensions
    {
        public static void ForEachReverse<T> (this IEnumerable<T> source, Action<T> action) => source.Reverse().ForEach(action);
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var variable in source)
                action.Invoke(variable);
        }
    }
}