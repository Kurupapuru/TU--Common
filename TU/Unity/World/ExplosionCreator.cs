using System;
using System.Collections;
using System.Threading;
using DG.Tweening;
using DG.Tweening.Core;
using Plugins.Lean.Pool;
using TU.Unity.HealthSystem;
using TU.Unity.HealthSystem.Interfaces;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TU.Unity.World
{
    public static class ExplosionCreator
    {
        public const bool DEBUG_ENABLED = false;
        public const bool SPHERE_COLL_EXPL = true;
        public const int MAX_OBJECTS_COLLECT = 50;

        public static DamageDealer damageDealer = new DamageDealer();
        public static Collider[] collsBuffer = new Collider[MAX_OBJECTS_COLLECT];

        public static void Initialize()
        {
            if (collsBuffer == null)
                collsBuffer = new Collider[MAX_OBJECTS_COLLECT];
        }

        public static void Explosion(Vector3 position, ExplosionSettings settings)
        {
            // Create PS
            if (settings.explPS != null)
            {
                ParticleSystem ps = Object.Instantiate(settings.explPS);
                ps.transform.position = position;
                ps.gameObject.SetActive(true);
                ps.Clear();
                ps.Play();
            }

            for (int i = 0; i < settings.damageExplosionsLayers.Length; i++)
                DoDamageExplosion(settings.damageExplosionsLayers[i]);

            for (int i = 0; i < settings.physicsExplosionsLayers.Length; i++)
            {
                if (SPHERE_COLL_EXPL)
                    DoSphereCollExplosion(settings.physicsExplosionsLayers[i]);
                else
                    DoPhysicsExplosion(settings.physicsExplosionsLayers[i]);
            }

            #region Methods

            IEnumerator DoDamageExplosion(ExplosionSettings.DamageExplosion damageExplosion)
            {
                if (damageExplosion.activateAfter > 0)
                    yield return new WaitForSeconds(damageExplosion.activateAfter);

                var collsCount =
                    Physics.OverlapSphereNonAlloc(
                        position,
                        damageExplosion.radius,
                        collsBuffer,
                        damageExplosion.layerMask);

                for (int i = 0; i < collsCount; i++)
                {
                    var attachedHS = collsBuffer[i].GetComponent<IHealthSystem>();
                    if (attachedHS == null)
                    {
                        if (DEBUG_ENABLED) Debug.Log($"Didn't found IHealthSystem attached to object: {collsBuffer[i].gameObject.name}");
                    }
                    else
                    {
                        if (DEBUG_ENABLED) Debug.Log($"Explosion damaged {collsBuffer[i].gameObject.name} with {damageExplosion.damage} damage");
                        damageDealer.DealDamage(attachedHS, damageExplosion.damage);
                    }
                }
            }

            IEnumerator DoPhysicsExplosion(ExplosionSettings.PhysicsExplosion physicsExplosion)
            {
                if (physicsExplosion.activateTime > 0)
                    yield return new WaitForSeconds(physicsExplosion.activateTime);

                var collsCount =
                    Physics.OverlapSphereNonAlloc(
                        position, physicsExplosion.radius,
                        collsBuffer,
                        physicsExplosion.layerMask);

                for (int i = 0; i < collsCount; i++)
                {
                    if (collsBuffer[i].attachedRigidbody != null)
                    {
                        // Temp Disable for car
                        if (DEBUG_ENABLED) Debug.Log($"AddExplosionForce on {collsBuffer[i].gameObject.name}");
                        collsBuffer[i].attachedRigidbody.AddExplosionForce(
                            physicsExplosion.power,
                            position,
                            physicsExplosion.radius,
                            physicsExplosion.upwardsModifier);
                    }
                }
            }

            void DoSphereCollExplosion(ExplosionSettings.PhysicsExplosion physicsExplosion)
            {
                var sphere = new GameObject("Sphere Coll Explosion", typeof(SphereCollider));
                sphere.layer = physicsExplosion.sphereCollLayer;
                sphere.transform.position = position;
                var sphereColl = sphere.GetComponent<SphereCollider>();
                var sphereRadiusSetter = new DOSetter<float>((float value) => sphereColl.radius = value);
                DOTween
                    .To(sphereRadiusSetter,
                        .1f,
                        physicsExplosion.radius,
                        physicsExplosion.activateTime)
                    .SetUpdate(UpdateType.Fixed)
                    .onComplete = () =>
                {
                    Observable
                        .Timer(TimeSpan.FromSeconds(physicsExplosion.despawnSphereAfter))
                        .Subscribe(_ => { sphere.Despawn(); });
                };
            }

            #endregion
        }
    }
}