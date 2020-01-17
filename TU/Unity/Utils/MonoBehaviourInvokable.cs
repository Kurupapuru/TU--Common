using TU.Sharp.Utils;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class MonoBehaviourInvokable : MonoBehaviour, IInvokable
    {
        public abstract void Invoke();
    }
}