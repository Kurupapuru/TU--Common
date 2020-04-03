using System;
using TU.Unity.Extensions;
using UnityEngine;

namespace GameDynamics.Character
{
    /// <summary>
    /// For proper working this script needs movementDirection setted from movement controller script
    /// </summary>
    public class RigidbodyImprover : MonoBehaviour
    {
        [Header("References")]
        public Rigidbody selfRb;
        public Collider selfCollider;

        [Header("Settings")]
        public LayerMask groundLayerMask;
        public LayerMask stairsLayerMask;
        public float slopeAngle = 35;
        public PhysicMaterial onGroundMaterial, onSlopeMaterial;
        public float stepHeight = .35f;
        
        
        // TODO: Show this values in "Debug" group
        [NonSerialized] public bool IsOnGround;
        [NonSerialized] public bool IsOnSlope;
        [NonSerialized] public Vector3 movementDelta;
        [NonSerialized] public Vector3 movementDirection;
        public bool IsCanWalk => IsOnGround && !IsOnSlope;
        
        
        private Vector3 previousPosition;

        private void Start() { previousPosition = selfRb.position; }

        private void FixedUpdate()
        {
            RaycastHit groundRaycastHit;
            Physics.Raycast(
                new Ray(selfRb.position + Vector3.up * .05f, Vector3.down),
                out groundRaycastHit,
            2,
                groundLayerMask);
            
            
            MovementDeltaUpdate();
            IsOnGroundCheck(groundRaycastHit);
            IsOnSlopeCheck(groundRaycastHit);
            MaterialByIsOnSlope();

        }

        private void MovementDeltaUpdate()
        {
            movementDelta = selfRb.position - previousPosition; 
            previousPosition = selfRb.position;
        }

        private void IsOnGroundCheck(RaycastHit hit)
        {
            IsOnGround = hit.collider != null && hit.distance < .2f;
        }

        private void IsOnSlopeCheck(RaycastHit hit)
        {
            var hitAngle = Vector3.Angle(Vector3.up, hit.normal);
            Debug.DrawRay(hit.point, hit.normal, Color.red, .1f, false);
            IsOnSlope = hit.collider != null 
                        && 
                        hitAngle >= slopeAngle;
            Debug.Log(hitAngle);
        }

        private void MaterialByIsOnSlope()
        {
            selfCollider.sharedMaterial = IsOnSlope ? onSlopeMaterial : onGroundMaterial;
        }
        
        
        
        
        private void OnCollisionEnter(Collision other)
        {
            ClimbOnStep(other);
        }

        private void ClimbOnStep(Collision other)
        {
            if (!stairsLayerMask.LayerCheck(other.gameObject.layer))
                return;
                
            var canClimbTo = selfRb.position.y + stepHeight;
            var isCanClimb = true;
            float highestPoint = selfRb.position.y;
            foreach (var contact in other.contacts)
            {
                if (contact.point.y > canClimbTo
                    ||
                    movementDirection == Vector3.zero 
                    || 
                    Vector3.Dot(movementDirection.normalized, (contact.point - selfRb.position).normalized) < .25f) // Слишком высокая ступенька или персонаж не идет в ее направлении
                {
                    isCanClimb = false;
                    break;
                }

                if (highestPoint < contact.point.y) highestPoint = contact.point.y;
            }

            if (!isCanClimb) return;
            
            selfRb.MovePosition(new Vector3(selfRb.position.x, highestPoint, selfRb.position.z));
        }
    }
}