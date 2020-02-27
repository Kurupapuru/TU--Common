using System;
using UnityEngine;

namespace _Serialized_Levels.Code
{
    public static class LevelLoader
    {
        public static void Load(byte[] savedLevel, Transform loadTo)
        {

        }

        public static byte[] Save(Transform root)
        {
            foreach (Transform child in root)
            {
                
            }
            
            throw new NotImplementedException();
        }
    }
}