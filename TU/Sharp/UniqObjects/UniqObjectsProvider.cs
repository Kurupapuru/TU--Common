namespace ZR.Runtime.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class UniqProvider<T> where T : IUniq
    {
        private static List<T> collection = new List<T>();

        public static void Add(T obj) => collection.Add(obj);
        public static void Remove(T obj) => collection.Remove(obj);
        public static T Get(int ID)
        {
            RemoveNulls();
            return collection.First(x => x.ID == ID);
        }
        public static T Get(IAutoHashID hash) => Get(hash.ID);
        
        
        private static void RemoveNulls() => collection.RemoveAll(x => x == null);
    }
}