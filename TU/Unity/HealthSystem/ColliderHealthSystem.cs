using System;
using UniRx;
using UnityEngine;

namespace TU.Unity.HealthSystem
{
    public class ColliderHealthSystem : HealthSystemMonoBehaviour
    {
        [Space] [SerializeField] public AnimationCurve DamagePerVelocityScale = AnimationCurve.Linear(0, 0, 5000, 5000);
        [SerializeField] public LayerMask physicsDamageOnlyFrom;

        protected Vector3 selfVelocity, lastSelfPos;
        protected DamageDealer selfDamageDealer = new DamageDealer();

        [NonSerialized] public ReactiveCommand<(Collider, Vector3?)> OnColliderEnteredTrigger = new ReactiveCommand<(Collider, Vector3?)>();

        [NonSerialized] public ReactiveCommand<(Collider, Vector3?)> OnColliderStayInTrigger = new ReactiveCommand<(Collider, Vector3?)>();
        [NonSerialized] public ReactiveCommand<(Collider, Vector3?)> OnColliderExitedTrigger = new ReactiveCommand<(Collider, Vector3?)>();

        [NonSerialized] public ReactiveCommand<Collision> OnColliderEnteredCollider = new ReactiveCommand<Collision>();
        [NonSerialized] public ReactiveCommand<Collision> OnColliderStayCollider = new ReactiveCommand<Collision>();
        [NonSerialized] public ReactiveCommand<Collision> OnColliderExitedCollider = new ReactiveCommand<Collision>();

        
        private void OnTriggerEnter(Collider collider) => DamageBasedOnCollider(collider, OnColliderEnteredTrigger);
        private void OnTriggerStay(Collider collider) => DamageBasedOnCollider(collider, OnColliderStayInTrigger);

        
        private void OnCollisionEnter(Collision collision)
        {
            OnColliderEnteredCollider.ForceExecute(collision);
            if (!LayerCheck(collision.gameObject.layer)) return;

            
            
            DamageBasedOnVelocity(collision.relativeVelocity.magnitude);
        }

        private void OnCollisionStay(Collision collision)
        {
            OnColliderStayCollider.ForceExecute(collision);
            if (!LayerCheck(collision.gameObject.layer)) return;
            DamageBasedOnVelocity(collision.relativeVelocity.magnitude);
        }


        protected virtual void Start()
        {
            lastSelfPos = transform.position;
            if (physicsDamageOnlyFrom == LayerMask.GetMask())
            {
                Debug.Log("<color=red>physicsDamageOnlyFrom was null, setted to \"Everything\" </color>", this);
            }
        }

        protected virtual void FixedUpdate()
        {
            var selfPosition = transform.position;
            selfVelocity = (selfPosition - lastSelfPos) / Time.fixedDeltaTime;
            lastSelfPos = selfPosition;
        }


        /// <returns>Relative velocity if object have rb, if hasn't returns null</returns>
        protected void DamageBasedOnCollider(Collider coll, ReactiveCommand<(Collider, Vector3?)> inTriggerCommand)
        {
            Vector3? relativeVelocity = null;

            if (coll.attachedRigidbody != null)
            {
                relativeVelocity = coll.attachedRigidbody.velocity - selfVelocity;
            }
            
            inTriggerCommand.ForceExecute((coll, relativeVelocity));

            if (relativeVelocity.HasValue && LayerCheck(coll.gameObject.layer)) 
                DamageBasedOnVelocity(relativeVelocity.Value.magnitude);
        }

        protected void DamageBasedOnVelocity(float velocityDifferenceMagnitude)
        {
            selfDamageDealer.DealDamage(this, DamagePerVelocityScale.Evaluate(velocityDifferenceMagnitude));
        }

        protected bool LayerCheck(int layer) => physicsDamageOnlyFrom == (physicsDamageOnlyFrom | (1 << layer));
    }
}