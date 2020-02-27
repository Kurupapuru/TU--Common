﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Serialized_Levels.Code.LevelParts;
using _Serialized_Levels.Code.SerializedLevels;
using KurupapuruLab;
using Sirenix.OdinInspector;
using TU.Unity.ComponentUtils;
using TU.Unity.Extensions;
using UniRx;
using UnityEngine;

public class LevelPart : MonoBehaviour
{
    [Header("References")] 
    public LevelPartsManager manager;
    public List<Transform> loaders = new List<Transform>();
    public float loadDistance = 10, unloadDistance = 20;
    public Transform hierarchyRoot;
    public byte[] hierarchyBytes;

    private PrefabInfoContainer[] spawned;
    private CompositeDisposable onEnabledDisposables = new CompositeDisposable();
    private bool loaded;


    
    private void FixedUpdate()
    {
        var closestDistance = float.MaxValue;
        
        foreach (var loaderT in loaders)
        {
            var distance = (loaderT.position - transform.position).magnitude;
            if (distance < closestDistance)
                closestDistance = distance;
        }
        
        if (closestDistance < loadDistance)
            Load();
        else if (closestDistance > unloadDistance)
            Unload();
    }

    [Button]
    private async void Save()
    {
        var hierarchy = new PrefabHierarchyObject();
        await hierarchy.CollectFromAsync(hierarchyRoot);
        await Task.Run(() =>
        {
            lock (hierarchyBytes)
                hierarchyBytes = BinarySerializer.BinaryFormatterSerialize(hierarchy);
        });
        
        Debug.Log("Save Completed");
    }

    [Button]
    public void DestroyChilds()
    {
        for (int i = hierarchyRoot.childCount - 1; i >= 0; i--)
        {
            hierarchyRoot.GetChild(i).gameObject.DestroySmart();
        }
    }
    
    private async void Load()
    {
        if (loaded) return;
        loaded = true;

        Debug.Log("Load");
        
        PrefabHierarchyObject hierarchy = (PrefabHierarchyObject) BinarySerializer.BinaryFormatterDeserialize(hierarchyBytes);

        await Task.WhenAll(hierarchy.SpawnChildsAsync(manager.pools, hierarchyRoot));
    }

    private void Unload()
    {
        if (!loaded) return;
        loaded = false;

        foreach (Transform child in hierarchyRoot)
        {
            manager.pools.Despawn(child.GetComponent<PrefabInfoContainer>());
        }
    }
}
