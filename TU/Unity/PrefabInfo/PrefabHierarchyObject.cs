using System;
using System.Threading.Tasks;
using Code.PoolSystem;
using TU.Unity;
using TU.Unity.Utils;
using TU.Unity.World;
using UnityEngine;

namespace _Serialized_Levels.Code.SerializedLevels
{
    [Serializable]
    public class PrefabHierarchyObject
    {
        public bool IsPrefab => String.IsNullOrEmpty(AssetPath);
        public string AssetPath;
        public PrefabHierarchyObject[] Childs;

        public SerializableVector3? localPosition;
        public SerializableVector3? localEulerAngles;
        public SerializableVector3? localScale;

        private static TimeSpan delay = TimeSpan.FromSeconds(.05f);

        public async Task<PrefabHierarchyObject> CollectFromAsync(Transform transform)
        {
            var prefabInfo = transform.GetComponent<PrefabInfoContainer>();
        
            if (!prefabInfo.IsNull())
            {
                // AssetPath = prefabInfo.adressablePath;
                AssetPath = prefabInfo.resourcesPath;
            
                var prefab = await LoadPrefab();
                var prefabTransform = prefab.transform;
            
                if (transform.localPosition != prefabTransform.localPosition)
                    localPosition = transform.localPosition;
                if (transform.localRotation != prefabTransform.localRotation)
                    localEulerAngles = transform.localEulerAngles;
                if (transform.localScale != prefabTransform.localScale)
                    localScale = transform.localScale;
            }
            else
            {
                if (transform.localPosition != Vector3.zero)
                    localPosition = transform.localPosition;
                if (transform.localRotation != Quaternion.identity)
                    localEulerAngles = transform.localEulerAngles;
                if (transform.localScale != Vector3.one)
                    localScale = transform.localScale;
                
                if (transform.childCount > 0)
                {
                    Childs = new PrefabHierarchyObject[transform.childCount];
                    Task<PrefabHierarchyObject>[] childCollectFromTasks = new Task<PrefabHierarchyObject>[transform.childCount];

                    for (int i = 0; i < transform.childCount; i++)
                    {
                        var child = transform.GetChild(i);
                        Childs[i] = new PrefabHierarchyObject();
                        childCollectFromTasks[i] = Childs[i].CollectFromAsync(child);
                    }

                    await Task.WhenAll(childCollectFromTasks);
                }
            }

            return this;
        }

        public async Task<GameObject> SpawnAsync(PoolsManager poolsManager, Transform parent, Action completedCallback = null)
        {
            GameObject spawned;

            if (!String.IsNullOrEmpty(AssetPath))
            {
                var prefab = await LoadPrefab();
                spawned = poolsManager.Spawn(prefab, parent).gameObject;
                
                spawned.transform.localPosition = localPosition.HasValue
                    ? (Vector3) localPosition.Value
                    : prefab.transform.localPosition;
                spawned.transform.localEulerAngles = localEulerAngles.HasValue
                    ? (Vector3) localEulerAngles.Value
                    : prefab.transform.localEulerAngles;
                spawned.transform.localScale =
                    localScale.HasValue ? (Vector3) localScale.Value : prefab.transform.localScale;
            }
            else
            {
                spawned = new GameObject("Group");
                spawned.transform.parent = parent;
                
                spawned.transform.localPosition = localPosition.HasValue
                    ? (Vector3) localPosition.Value
                    : Vector3.zero;
                spawned.transform.localEulerAngles = localEulerAngles.HasValue
                    ? (Vector3) localEulerAngles.Value
                    : Vector3.zero;
                spawned.transform.localScale =
                    localScale.HasValue ? (Vector3) localScale.Value : Vector3.one;
            }
            
            

            if (Childs!=null && Childs.Length>0)
            {
                spawned.gameObject.SetActive(false);
                Task<GameObject>[] childSpawnTasks = new Task<GameObject>[Childs.Length];
                for (int i = 0; i < childSpawnTasks.Length; i++)
                {
                    childSpawnTasks[i] = Childs[i].SpawnAsync(poolsManager, spawned.transform);
                }

                await Task.WhenAll(childSpawnTasks);
                spawned.gameObject.SetActive(true);
            }
        
            completedCallback?.Invoke();

            return spawned;
        }

        private async Task<GameObject> LoadPrefab() => await LoadPrefab(AssetPath);
        private static async Task<GameObject> LoadPrefab(string path)
        {
            
            // Loading through AssetDatabase or Addressables
//             GameObject prefab;
//             
// #if UNITY_EDITOR
//             if (!Application.isPlaying)
//                 prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
//             else
// #endif
//                 prefab = await Addressables.LoadAssetAsync<GameObject>(path).Task;
//             return prefab;
            
            // Loading through Resources
            var prefabRequest = Resources.LoadAsync<GameObject>(path);
            while (!prefabRequest.isDone)
                await delay;
        
            return (GameObject) prefabRequest.asset;
        }

        public Task<GameObject>[] SpawnChildsAsync(PoolsManager poolsManager, Transform parent)
        {
            Task<GameObject>[] childSpawnTasks = new Task<GameObject>[Childs.Length];
        
            for (int i = 0; i < childSpawnTasks.Length; i++)
            {
                childSpawnTasks[i] = Childs[i].SpawnAsync(poolsManager, parent);
            }

            return childSpawnTasks;
        }
    }
}