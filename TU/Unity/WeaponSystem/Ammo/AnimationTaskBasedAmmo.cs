using System;
using AnimationTools.AnimationTasks;
using Plugins.Lean.Pool;
using SubProjects.GiantHalls.Code;
using TU.Unity.HealthSystem;
using TU.Unity.HealthSystem.Interfaces;
using UniRx;
using UnityEngine;
using WeaponSystem.Ammo;
using ExplosionCreator = TU.Unity.World.ExplosionCreator;

public class AnimationTaskBasedAmmo : AbstractAmmo
{
    [NonSerialized] public AnimationTask animTask;
    [NonSerialized] private Vector3 fromPos;
    [NonSerialized] private static DamageDealer _damageDealer = new DamageDealer();
    [NonSerialized] private AnimationTaskBasedAmmoSettings conv_settings;

    public override void Initialize(Transform target, IAmmoSettings ammoSettings, bool createMuzzle = true)
    {
        if (target == null)
        {
            gameObject.Despawn();
            return;
        }
        
        base.Initialize(target, ammoSettings);
        conv_settings = (AnimationTaskBasedAmmoSettings) settings;
        
        fromPos = transform.position;

        var distanceToTarget = (transform.position - targetPos).magnitude;

        var animationLength = settings.calculateSpeedByFixedValue ? settings.fixedFlyTime : distanceToTarget / settings.speedPerSec;
        animTask = new AnimationTask(animationLength);

        animTask.onUpdate.Subscribe(PositionUpdate);
        animTask.onFinish.Subscribe(_ => StartCoroutine(Hit(target)));
        animTask.Play();
    }

    protected void OnDisable()
    {
        animTask?.Dispose();
    }

    protected override void OnHit(Transform hittedObject)
    {
        var targetHealth = hittedObject.GetComponent<IHealthSystem>();
        if (targetHealth!=null)
            _damageDealer.DealDamage(targetHealth, settings.damage);
        
        if (settings.explosion)
            ExplosionCreator.Explosion(transform.position, settings.explosionSettings);
    }

    protected virtual void PositionUpdate(IAnimationTask animTask)
    {
        if (target == null)
        {
            animTask.Stop();
            return;
        }
        
        var prevPos = transform.position;
        float flyPosValue = conv_settings.flyCurve.Evaluate(animTask.progress);
        var newPos = Vector3.Lerp(fromPos, target.position, flyPosValue);
        newPos.y += conv_settings.yAddCurve.Evaluate(flyPosValue) * conv_settings.yAddCurveMult;
        transform.position = newPos;
        var direction = newPos - prevPos;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }
}