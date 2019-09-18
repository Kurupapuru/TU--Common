using System;
using Plugins.Lean.Pool;
using UniRx;
using UnityEngine;

public class PoolableParticleSystem : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public bool playOnEnable = true;
    public bool withChildren = true;

    public bool despawn = false;
    public float disableAfter = 4;
    private IDisposable disableAfterTask;

    protected void Awake()
    {
        if (particleSystem == null)
            particleSystem = GetComponent<ParticleSystem>();
    }

    protected virtual void OnEnable()
    {
        if (playOnEnable && particleSystem.isStopped)
            particleSystem.Play(withChildren);

        
        if (disableAfter > 0)
        {
            disableAfterTask = Observable
                .Timer(TimeSpan.FromSeconds(disableAfter))
                .Subscribe(_ =>
                {
                    if (gameObject.activeSelf)
                        gameObject.SetActive(false);
                });
        }
    }

    protected virtual void OnDisable()
    {
        disableAfterTask?.Dispose();
        particleSystem.Stop(withChildren);
        if (despawn) gameObject.Despawn();
    }
}
