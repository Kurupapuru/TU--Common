using System;
using UniRx;
using UnityEngine;
using UXK.HealthSystem;

public class DestroyOnDeath : MonoBehaviour
{
    private IDisposable _destroyOnDeathSub;
    
    private void Start()
    {
        _destroyOnDeathSub = GetComponent<IHealthController>().IsAlive.Subscribe(x =>
        {
            if (x == false)
                Destroy(gameObject);
        });
    }

    private void OnDestroy()
    {
        _destroyOnDeathSub.Dispose();
    }
}
