using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

using UnityEditor;

[ExecuteInEditMode]
public class PrefabInfoContainer : MonoBehaviour
{
    public GameObject prefab;
    public string guid;
    public string assetPath;
    public string resourcesPath;
    public string adressablePath;


    [Button]
    private void UpdateInfo()
    {
        assetPath = AssetDatabase.GetAssetPath(gameObject);
        adressablePath = assetPath;
        guid = AssetDatabase.AssetPathToGUID(assetPath);
        resourcesPath = assetPath.Substring(assetPath.LastIndexOf("Resources/") + 10);
        resourcesPath = resourcesPath.Remove(resourcesPath.Length - 7, 7);
    }

    private void Awake()
    {
        prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
    }
}
