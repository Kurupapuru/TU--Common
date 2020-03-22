using System;
using System.IO;
using System.Threading.Tasks;
using NaughtyAttributes;
using TU.Unity.Extensions;
using TU.Unity.World;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TU.Unity.SerializedLevels
{
    public class LevelSerializedPart : MonoBehaviour
    {
        public bool loaded;
        
        [Header("References")]
        public LevelSerializedRoot root;
        public Transform hierarchyRoot;


        [Header("Settings")] 
        public float loadDistance = 10;
        public float unloadDistance = 20;
        
        [Header("Location")]
        public TextAsset serializedAsset;
        public bool SerializedAssetIsNull => serializedAsset == null;
        
#if UNITY_EDITOR
        [Header("Or Create New")]
        [ShowIf("SerializedAssetIsNull")]
        public DefaultAsset folder;

        [Button]
        public async void SaveAsync()
        {
            string path;

            lock (folder)
                path = SerializedAssetIsNull ? $"{folder.GetFullPath()}/{name}.bytes" : serializedAsset.GetFullPath();

            var bytes = await root.levelSerializer.SerializeHierarchy(hierarchyRoot);
            
            FileInfo fileInfo = new FileInfo(path);
            
            using (var fileStream = fileInfo.OpenWrite())
            {
                await fileStream.WriteAsync(bytes, 0, bytes.Length);
            }
            
            await new TimeSpan(0, 0, 0, 0, 100);
            AssetDatabase.ImportAsset(path);
            AssetDatabase.Refresh();
            await new TimeSpan(0, 0, 0, 0, 100);
            
            lock (folder)
                serializedAsset = AssetDatabase.LoadAssetAtPath<TextAsset>($"{folder.GetAssetPath()}/{name}.bytes");
            
            Debug.Log($"Save completed \n Loaded: {serializedAsset}");
        }
#endif

        private void FixedUpdate()
        {
            var closestDistance = float.MaxValue;
        
            foreach (var loaderT in root.loaders)
            {
                var distance = (loaderT.position - transform.position).magnitude;
                if (distance < closestDistance)
                    closestDistance = distance;
            }
        
            if (!loaded && closestDistance < loadDistance)
            {
                LoadAsync();
                loaded = true;
            }
            else if (loaded && closestDistance > unloadDistance)
            {
                Unload(hierarchyRoot);
                loaded = false;
            }
        }

        private void Start()
        {
            loaded = hierarchyRoot.childCount > 0;
        }

        [Button]
        public void DestroyChilds()
        {
            for (int i = hierarchyRoot.childCount - 1; i >= 0; i--)
            {
                hierarchyRoot.GetChild(i).gameObject.DestroySmart();
            }

            loaded = false;
        }
        
        [Button]
        public async void LoadAsync()
        {
            if (loaded) return;
            loaded = true;
            
            var hierarchy = await root.levelSerializer.DeserializeHierarchy(serializedAsset.bytes);
            await Task.WhenAll(hierarchy.SpawnChildsAsync(root.levelSerializer.pools, hierarchyRoot));
        }
        
        private void Unload(Transform parent)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                var childT = parent.GetChild(i);
                var isPrefab = !childT.GetComponent<PrefabInfoContainer>().IsNull();
                
                if (!isPrefab)
                    Unload(childT);

                if (isPrefab) 
                    root.levelSerializer.pools.Despawn(childT.gameObject);
                else
                    childT.gameObject.DestroySmart();
            }
        }
    }
}