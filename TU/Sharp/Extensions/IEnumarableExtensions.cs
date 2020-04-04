using System;
using System.Collections.Generic;
using System.Linq;

namespace TU.Sharp.Extensions
{
    public static class IEnumarableExtensions
    {
        public static void ForEachReverse<T> (this IEnumerable<T> source, Action<T> action) => source.Reverse().ForEach(action);
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var variable in source)
                action.Invoke(variable);
        }

        public static bool TryModifyElement<T>(this List<T> source, Predicate<T> match, RefAction<T> modify)
        {
            var index = source.FindIndex(match);
            
            if (index < 0)
                return false;
            
            var value = source[index];
            modify.Invoke(ref value);
            source[index] = value;
            return true; 
        }
    }
}