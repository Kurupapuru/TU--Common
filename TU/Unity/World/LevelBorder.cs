using UnityEngine;

namespace TU.Unity.World
{
    public class LevelBorder : MonoBehaviour
    {
        [SerializeField] private float direction = 1;
    
        [Header("Force")]
        [SerializeField, Tooltip("time == distance")] private AnimationCurve forceCurve;
        [SerializeField] private float forceCurveMult = 1;
        [SerializeField] private ForceMode forceMode;
    
        [Header("Car Rotation")]
        [SerializeField, Tooltip("time == distance")] private AnimationCurve carRotationCurve;
        [SerializeField] private float carRotationCurveMult = 1;
        [SerializeField] private LayerMask carLayerMask;
    
    
        private void OnTriggerStay(Collider other)
        {
            var otherT = other.transform;
            var otherColl = other.GetComponent<Collider>();
            var otherRb = other.GetComponent<Rigidbody>();
        
            float distance;
            if (otherColl != null)
                distance = otherColl.ClosestPoint(otherT.position + Vector3.forward * direction * -10).z - transform.position.z;
            else
                distance = otherT.position.z - transform.position.z;
            distance = Mathf.Abs(distance);
        
            if (otherRb != null)
            {
                var addForce = forceCurve.Evaluate(distance) * forceCurveMult * direction * Vector3.forward; // Vector3.right
                otherRb.AddForce(addForce, forceMode);
            }
        }
    }
}
