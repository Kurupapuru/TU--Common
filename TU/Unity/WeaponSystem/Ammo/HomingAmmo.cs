using System;
using Plugins.Lean.Pool;
using TU.Unity.HealthSystem;
using TU.Unity.HealthSystem.Interfaces;
using TU.Unity.World;
using UniRx;
using UnityEngine;
using WeaponSystem.Ammo;

public class HomingAmmo : AbstractAmmo
{
    [NonSerialized] private IDisposable moveTask;
    [NonSerialized] private DamageDealer _damageDealer = new DamageDealer();
    [NonSerialized] private HomingAmmoSettings conv_settings;
    [NonSerialized] private float startTime;

    public override void Initialize(Transform target, IAmmoSettings ammoSettings, bool createMuzzle = true)
    {
        if (target == null)
        {
            gameObject.Despawn();
            return;
        }
        
        base.Initialize(target, ammoSettings, createMuzzle);
        conv_settings = (HomingAmmoSettings)ammoSettings;
        
        startTime = Time.time;

        moveTask?.Dispose();
        moveTask = Observable.EveryFixedUpdate()
            .Subscribe(_ => MoveToTarget(Time.fixedDeltaTime));
    }

    public void MoveToTarget(float deltaTime)
    {
        var newPos = transform.position;

        // Rotating
        if (target != null)
        {
            var dirToTarget = targetPos - newPos;
            var angleToTarget = Vector3.Angle(transform.forward, dirToTarget.normalized);
            if (angleToTarget <= conv_settings.homingAllowedAngle)
            {
                transform.rotation = Quaternion.Lerp(
                    a: transform.rotation,
                    b: Quaternion.LookRotation(dirToTarget),
                    t: conv_settings.rotateLerpSpeed * deltaTime);
            }
        }

        // Forward move
        var prevPos = newPos;
        newPos += settings.speedPerSec * deltaTime * transform.forward;

        // Hit Check
        var rHit = RaycastCheck(prevPos, newPos);
        if (rHit.collider != null && rHit.collider.transform == target)
        {
            newPos = rHit.point;
            StartCoroutine(Hit(rHit.transform));
        }

        transform.position = newPos;

        var direction = newPos - prevPos;
        transform.rotation = Quaternion.LookRotation(direction);

        if (Time.time - startTime > settings.disapearAfter)
        {
            Debug.Log($"Despawning {gameObject.name} becose of disapear time");
            moveTask?.Dispose();
            gameObject.Despawn();
        }
    }

    protected void OnDisable()
    {
        moveTask?.Dispose();
    }

    protected override void OnHit(Transform hittedObject)
    {
        var targetHealth = hittedObject.GetComponent<IHealthSystem>();
        if (targetHealth != null)
            _damageDealer.DealDamage(targetHealth, settings.damage);

        if (settings.explosion)
        {
            ExplosionCreator.Explosion(transform.position, settings.explosionSettings);
        }

        moveTask.Dispose();
    }
}