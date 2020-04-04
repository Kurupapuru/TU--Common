using System;
using UniRx;
using UnityEngine;

namespace UXK.HealthSystem.Testing
{
    public class HealthSystemTesting : UnityEngine.MonoBehaviour
    {
        public HealthControllerContainer hasHealth;
        public float healthChangeAmount = 10;
        public HealthChangeType healthChangeType;

        private CompositeDisposable _disposables = new CompositeDisposable();

        private void Start()
        {
            hasHealth
                .Health.Subscribe(x =>
                    Debug.Log($"Health: {x.ToString()}"))
                .AddTo(_disposables);
            hasHealth
                .IsAlive.Subscribe(x =>
                    Debug.Log($"IsAlive: {x.ToString()}"))
                .AddTo(_disposables);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                hasHealth.ApplyHealthChange(
                    new HealthChangeInfo(healthChangeAmount, healthChangeType));
            }
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}