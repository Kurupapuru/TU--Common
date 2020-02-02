using System;
using System.Collections.Generic;

namespace TU.Sharp.Extensions
{
    public static class DictionaryExtension
    {
        public static Value Get<Key, Value>(this Dictionary<Key, Value> source, Key key, Func<Value> defaultValue)
        {
            if (source.TryGetValue(key, out var result))
            {
                return result;
            }
            else
            {
                var createdDefaultValue = defaultValue.Invoke();
                source[key] = createdDefaultValue;
                return createdDefaultValue;
            }
        }
    }
}