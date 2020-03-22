using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace TU.Unity
{
    public class UnityCallbacksToEvents : MonoBehaviour
    {
        [BoxGroup("Awake")] public UnityEvent OnAwakeEvent;
        [BoxGroup("Start")] public UnityEvent OnStartEvent;
        [BoxGroup("OnEnable")] public UnityEvent OnEnableEvent;
        [BoxGroup("OnDisable")] public UnityEvent OnDisableEvent;
        [BoxGroup("OnDestroy")] public UnityEvent OnDestroyEvent;

        private void Awake()     => OnAwakeEvent?.Invoke();
        private void Start()     => OnStartEvent?.Invoke();
        private void OnEnable()  => OnEnableEvent?.Invoke();
        private void OnDisable() => OnDisableEvent?.Invoke();
        private void OnDestroy() => OnDestroyEvent?.Invoke();
    }
}
