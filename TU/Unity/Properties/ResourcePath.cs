using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif



[Serializable] public class ResourcePathObject : ResourcePath<Object> {}
[Serializable] public class ResourcePathGameObject : ResourcePath<Object> {}

[Serializable]
public class ResourcePath<T> where T : Object
{
    [ValueDropdown("GetAllResourcesPathsWithUnity")]
    public string path;

    
#if UNITY_EDITOR
    
    private static List<string> results = new List<string>(16);
    private const string resourcesPathStringSystem = "Resources\\";
    private const string resourcesPathStringUnity = "Resources/";
    private IEnumerable<string> GetAllResourcesPaths() // Too heavy
    {
        results.Clear();
        results.Add("Null");
        
        DirectoryInfo levelDirectoryPath = new DirectoryInfo(Application.dataPath);
        FileInfo[] fileInfos = levelDirectoryPath.GetFiles("*", SearchOption.AllDirectories);
        foreach (var fileInfo in fileInfos)
        {
            var filePath = fileInfo.FullName;
            if  (filePath.EndsWith(".meta")) continue;
            var resourcesLastIndex = filePath.LastIndexOf(resourcesPathStringSystem);
            if (resourcesLastIndex == -1) continue;
            filePath = filePath.Substring(resourcesLastIndex + resourcesPathStringSystem.Length);
            var dotLastIndex = filePath.LastIndexOf('.');
            filePath = filePath.Substring(0, dotLastIndex);
            Debug.Log(filePath);
            filePath = filePath.Replace('\\', '/');
            results.Add(filePath);
        }

        return results;
    }

    private IEnumerable<string> GetAllResourcesPathsWithUnity()
    {
        results.Clear();
        results.Add("Null");

        var allResources = Resources.LoadAll<T>("");
        foreach (var resource in allResources)
        {
            var filePath = AssetDatabase.GetAssetPath(resource);
            filePath = filePath.Substring(filePath.LastIndexOf(resourcesPathStringUnity) + resourcesPathStringUnity.Length);
            filePath = filePath.Substring(0, filePath.LastIndexOf('.'));
            results.Add(filePath);
        }
        
        return results;
    }
    
#endif
    
}