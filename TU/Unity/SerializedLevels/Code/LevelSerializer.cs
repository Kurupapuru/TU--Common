using System;
using System.Threading.Tasks;
using _Serialized_Levels.Code.SerializedLevels;
using Code.PoolSystem;
using KurupapuruLab;
using UnityEngine;

namespace TU.Unity.SerializedLevels
{
    [Serializable]
    public class LevelSerializer
    {
        public PoolsManager pools;

        public async Task<byte[]> SerializeHierarchy(Transform root)
        {
            var rootName = root.gameObject.name;
            Debug.Log($"Starting serializing {rootName} hierarchy");
        
            var hierarchy = await new PrefabHierarchyObject().CollectFromAsync(root);
            var bytes = BinarySerializer.BinaryFormatterSerialize(hierarchy);

            Debug.Log($"Completed serializing {rootName} hierarchy, root childs: \n {hierarchy.Childs.Length}");
            return bytes;
        }

        public async Task<PrefabHierarchyObject> DeserializeHierarchy(byte[] bytes)
        {
            return await Task.Run(() => (PrefabHierarchyObject)BinarySerializer.BinaryFormatterDeserialize(bytes));
        }
    }
}