using UnityEngine;

namespace DefaultNamespace
{
    public class ColliderCallbacksToAnimator : MonoBehaviour
    {
        public Animator animator;
        public LayerMask layerMask;

        
        protected void OnCollisionEnter(Collision other)
        {
            if (!layerMask.LayerCheck(other.gameObject.layer)) return;
            
            animator.SetTrigger(1514430047);
            animator.SetBool(161259540, true);
        }

        protected void OnCollisionExit(Collision other)
        {
            if (!layerMask.LayerCheck(other.gameObject.layer)) return;

            animator.SetTrigger(-1013143674);
            animator.SetBool(161259540, false);
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (!layerMask.LayerCheck(other.gameObject.layer)) return;

            animator.SetTrigger(1167711205);
            animator.SetBool(245003463, true);
        }

        protected void OnTriggerExit(Collider other)
        {
            if (!layerMask.LayerCheck(other.gameObject.layer)) return;

            animator.SetTrigger(-996510891);
            animator.SetBool(245003463, false);
        }
    }
}