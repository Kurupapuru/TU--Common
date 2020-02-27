using System.Collections.Generic;
using Code.PoolSystem;
using UnityEngine;

namespace _Serialized_Levels.Code
{
    public class LevelSerializedRoot : MonoBehaviour
    {
        public List<Transform> loaders = new List<Transform>();
        public LevelSerializer levelSerializer = new LevelSerializer();
    }
}