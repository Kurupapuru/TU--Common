using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LevelSerializerTest : MonoBehaviour
{
    [MenuItem("Test/GetAdressTest")]
    public static void GetAdressTest()
    {
        Addressables.LoadResourceLocationsAsync(Selection.activeObject).Completed +=
            adresses => Debug.Log(adresses.Result[0]);
    }
    
    [MenuItem("Test/GetObjectsInfoTest")]
    public static void GetObjectsInfoTest()
    {
        var levelSerializer = new LevelSerializer();
        levelSerializer.SerializeHierarchy(Selection.activeTransform);
    }
}
