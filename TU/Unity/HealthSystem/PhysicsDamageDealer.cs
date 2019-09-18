using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace TU.Unity.HealthSystem
{
    public class PhysicsDamageDealer : MonoBehaviour
    {
        private List<Component> damagedComponenct = new List<Component>(10);

        [SerializeField] protected AnimationCurve damagePerVelocity = AnimationCurve.Linear(0, 0, 1000, 1000);
        [SerializeField] protected LayerMask damageLayers;
        [SerializeField] protected DamageDealer damageDealer = new DamageDealer(damageType: DamageType.Physics);
        [SerializeField] protected Rigidbody selfRb;
        [SerializeField] protected float minSelfVelocity = .5f;

        [FormerlySerializedAs("fakePhysiscsWatcher")] [SerializeField]
        protected FakePhysicsWatcher fakePhysicsWatcher;

        protected Vector3 selfVelocity => fakePhysicsWatcher != null ? fakePhysicsWatcher.velocity : selfRb.velocity;


        private void Awake()
        {
            if (fakePhysicsWatcher == null && selfRb == null)
                fakePhysicsWatcher = GetComponent<FakePhysicsWatcher>();
            if (fakePhysicsWatcher == null && selfRb == null)
                selfRb = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision other)
        {
            damagedComponenct.Clear();
        
            // Velocity Check
            if (selfVelocity.magnitude <= minSelfVelocity) return;

            // Layer Check
            if (!LayerCheck(other.gameObject.layer)) return;

            other.gameObject.GetComponents(damagedComponenct);

            var healthSystem = damagedComponenct.GetComponent<HealthSystemMonoBehaviour>();
            if (healthSystem == null) return;

            var otherFakePhysicsWatcher = damagedComponenct.GetComponent<FakePhysicsWatcher>();
            if (otherFakePhysicsWatcher != null)
            {
                DealPhysicsDamage(healthSystem, damagedComponenct, selfVelocity - otherFakePhysicsWatcher.velocity);
                return;
            }

            DealPhysicsDamage(healthSystem, damagedComponenct, other.relativeVelocity);
        }

        private void OnTriggerEnter(Collider other) => OnTrigger(other);
        private void OnTriggerStay(Collider other) => OnTrigger(other);

        private void OnTrigger(Collider other)
        {
            damagedComponenct.Clear();
        
            // Velocity Check
            if (selfVelocity.magnitude <= minSelfVelocity) return;

            // Layer Check
            if (!LayerCheck(other.gameObject.layer)) return;

            other.gameObject.GetComponents<Component>(damagedComponenct);

            var healthSystem = damagedComponenct.GetComponent<HealthSystemMonoBehaviour>();
            if (healthSystem == null) return;

            var otherFakePhysicsWatcher = damagedComponenct.GetComponent<FakePhysicsWatcher>();
            if (otherFakePhysicsWatcher != null)
            {
                DealPhysicsDamage(healthSystem, damagedComponenct, selfVelocity - otherFakePhysicsWatcher.velocity);
                return;
            }

            var otherRb = damagedComponenct.GetComponent<Rigidbody>();
            if (otherRb != null && !otherRb.isKinematic)
            {
                DealPhysicsDamage(healthSystem, damagedComponenct, selfVelocity - otherRb.velocity);
                return;
            }

            DealPhysicsDamage(healthSystem, damagedComponenct, selfVelocity);
            // hell know what to do with this
        }
    

        protected void DealPhysicsDamage(HealthSystemMonoBehaviour healthSystem, IList<Component> otherComponenets, Vector3 relativeVelocity)
        {
        
            var damage = damagePerVelocity.Evaluate(relativeVelocity.magnitude);
            damageDealer.DealDamage(healthSystem, damage);

            var callBackReceiver = otherComponenets.GetComponent<IPhysicsDamageReceivedCallback>();
            callBackReceiver?.PhysicsDamageReceived(relativeVelocity, damage);
        }

        protected bool LayerCheck(int layer) => damageLayers == (damageLayers | (1 << layer));
    }

    public interface IPhysicsDamageReceivedCallback
    {
        void PhysicsDamageReceived(Vector3 relativeForce, float damagePack);
    }
}