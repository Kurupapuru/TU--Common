using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggersCollector : MonoBehaviour
{
    public LayerMask layerMask;

    [NonSerialized] public List<Collider> colliders = new List<Collider>(1);
    
    private void OnTriggerEnter(Collider other)
    {
        if (!layerMask.LayerCheck(other.gameObject.layer)) return;
        colliders.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!layerMask.LayerCheck(other.gameObject.layer)) return;
        colliders.Remove(other);
    }
}
