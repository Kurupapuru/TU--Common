using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.PoolSystem
{
    [Serializable]
    public class PoolsManager
    {
        public static readonly PoolsManager Default = new PoolsManager();
        
        [ShowInInspector, ReadOnly] private readonly List<PrefabPool> pools = new List<PrefabPool>();
        
        [SerializeField] private Transform _parentAfterDespawnTo;
        public Transform ParentAfterDespawnTo
        {
            get => _parentAfterDespawnTo;
            set
            {
                foreach (var pool in pools)
                {
                    pool.ParentAfterDespawnTo = value;
                }

                _parentAfterDespawnTo = value;
            }
        }

        public PrefabInfoContainer Spawn(GameObject prefab, Transform parent = null, Action<PrefabInfoContainer> doOnFirstSpawn = null,
            Action<PrefabInfoContainer> doOnSpawn = null)
        {
            PrefabPool pool = GetPrefabPool(prefab);
            return pool.Spawn(parent, doOnFirstSpawn, doOnSpawn);
        }

        /// <summary>
        /// Этот метод крайне плохо влияет на производительность, так как проходится по всем активным объектам во всех пулах
        /// </summary>
        public void Despawn(GameObject gameObject)
        {
            foreach (var pool in pools)
            {
                try
                {
                    pool.Despawn(gameObject);
                    return;
                }
                catch (KeyNotFoundException e)
                {
                    // ignore
                }
            }
            
            throw new KeyNotFoundException($"{gameObject.name} is not in static pools list");
        }

        public void Despawn(PrefabInfoContainer info)
        {
            GetPrefabPool(info.prefab).Despawn(info);
        }

        public void Despawn(GameObject prefab, GameObject gameObject)
        {
            GetPrefabPool(prefab).Despawn(gameObject);
        }

        public PrefabPool GetPrefabPool(GameObject prefab)
        {
            PrefabPool result = pools.FirstOrDefault(x => x.prefab == prefab);
            
            if (result == null)
            {
                result = new PrefabPool(prefab, _parentAfterDespawnTo);
                pools.Add(result);
            }
            
            return result;
        }
    }
}