using UnityEngine;

namespace TU.Unity.ComponentUtils
{
    public class ParentOnStart : MonoBehaviour
    {
        public Transform parentTo;
    
        private void Start()
        {
            transform.parent = parentTo;
        }
    }
}
