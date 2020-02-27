#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace TU.Unity
{
    public static class UnityEditorExtensions
    {
        public static string GetFullPath(this Object asset)
        {
            var assetPath = AssetDatabase.GetAssetPath(asset).Remove(0, 6);
            return Application.dataPath + assetPath;
        }
        public static string GetAssetPath(this Object asset)
        {
            return AssetDatabase.GetAssetPath(asset);
        }
    }
}
#endif
