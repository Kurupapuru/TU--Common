using System.Collections.Generic;
using UnityEngine;

namespace TU.Unity.SerializedLevels
{
    public class LevelSerializedRoot : MonoBehaviour
    {
        public List<Transform> loaders = new List<Transform>();
        public LevelSerializer levelSerializer = new LevelSerializer();
    }
}