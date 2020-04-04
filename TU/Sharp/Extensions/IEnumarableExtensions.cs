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

        public static int FindIndex<T>(this IList<T> source, Func<T, bool> match)
        {
            for (int i = 0; i < source.Count; i++)
            {
                if (match.Invoke(source[i]))
                    return i;
            }

            return -1;
        }

        public static bool TryModifyElement<T>(this IList<T> source, Func<T, bool> match, RefAction<T> modify)
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