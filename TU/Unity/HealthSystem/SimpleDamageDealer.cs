using TU.Unity.HealthSystem.Interfaces;
using UnityEngine;

namespace TU.Unity.HealthSystem
{
    public class SimpleDamageDealer : MonoBehaviour
    {
        public float damage = 1;


        protected DamageDealer damageDealer = new DamageDealer(damageType: DamageType.Normal);

        protected void OnTriggerEnter(Collider other)    => DealDamageToObject(other);
        protected void OnCollisionEnter(Collision other) => DealDamageToObject(other.collider);

        protected void DealDamageToObject(Collider coll)
        {
            IHealthSystem healthSystem;
            if (coll.attachedRigidbody != null)
                healthSystem = coll.attachedRigidbody.GetComponent<IHealthSystem>();
            else
                healthSystem = coll.GetComponent<IHealthSystem>();

            if (healthSystem == null) return;

            damageDealer.DealDamage(healthSystem, damage);
        }
    }
}