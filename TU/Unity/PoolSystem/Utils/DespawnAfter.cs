using System;
using System.Collections;
using System.Collections.Generic;
using Code.PoolSystem;
using UnityEngine;

public class DespawnAfter : MonoBehaviour
{
    public PrefabInfoContainer prefabInfoContainer;
    public float despawnAfter = 3;
    
    
    
    private void OnEnable()
    {
        StartCoroutine(DespawnAfterCoroutine());
    }

    private IEnumerator DespawnAfterCoroutine()
    {
        yield return new WaitForSeconds(despawnAfter);
        
        if (!ReferenceEquals(prefabInfoContainer, null))
            PoolsManager.Default.Despawn(prefabInfoContainer);
        else
            PoolsManager.Default.Despawn(gameObject);
    }
}
