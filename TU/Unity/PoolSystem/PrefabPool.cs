using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.PoolSystem
{
    [Serializable]
    public class PrefabPool
    {
        public GameObject prefab;
        [SerializeField] private Transform _parentAfterDespawnTo;
        
        private readonly Stack<PrefabInfoContainer> _available = new Stack<PrefabInfoContainer>();
        private readonly List<PrefabInfoContainer>  _busy      = new List<PrefabInfoContainer>();

        public Transform ParentAfterDespawnTo
        {
            get => _parentAfterDespawnTo;
            set
            {
                _parentAfterDespawnTo = value;
                
                foreach (var prefabInfoContainer in _available)
                {
                    prefabInfoContainer.transform.parent = value;
                }
            }
        }
        
        public PrefabPool(GameObject prefab, Transform parentAfterDespawnTo)
        {
            this.prefab = prefab;
            this._parentAfterDespawnTo = parentAfterDespawnTo;
        }
        
        public PrefabInfoContainer Spawn(Transform parent = null, Action<PrefabInfoContainer> doOnFirstSpawn = null,
            Action<PrefabInfoContainer> doOnSpawn = null)
        {
            PrefabInfoContainer instance;
            
            if (_available.Count > 0)
            {
                instance = _available.Pop();
                instance.transform.parent = parent;
            }
            else
            {
                instance = Object.Instantiate(prefab, parent).GetComponent<PrefabInfoContainer>();
                doOnFirstSpawn?.Invoke(instance);
            }
            
            doOnSpawn?.Invoke(instance);
            instance.gameObject.SetActive(true);
            
            _busy.Add(instance);

            return instance;
        }

        public void Despawn(PrefabInfoContainer instance) =>
            Despawn(instance.gameObject);
        
        public void Despawn(GameObject instance)
        {
            int foundObjectID = -1;
            for (var i = 0; i < _busy.Count; i++)
            {
                if (_busy[i].gameObject != instance) continue;
                
                foundObjectID = i;
                break;
            }
            
            if (foundObjectID == -1) throw new KeyNotFoundException($"{instance.name} is not in that pool");
            
            _available.Push(_busy[foundObjectID]);
            _busy.RemoveAt(foundObjectID);
            
            instance.SetActive(false);
            if (!ReferenceEquals(_parentAfterDespawnTo, null))
                instance.transform.parent = _parentAfterDespawnTo;
        }
    }
}