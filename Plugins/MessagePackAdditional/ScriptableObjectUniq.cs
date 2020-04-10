using MessagePack;
using UnityEngine;

namespace System.Collections.Generic
{
    public abstract class ScriptableObjectUniq<T> : ScriptableObject where T : ScriptableObjectUniq<T>
    {
        [IgnoreMember][SerializeField] private string _path;
        [IgnoreMember] public string Path => _path;

        public static T GetByPath(string path) => 
            UnityEngine.Resources.Load<T>(path);
    }
}