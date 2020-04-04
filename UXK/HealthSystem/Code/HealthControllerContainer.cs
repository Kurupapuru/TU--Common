using System;
using UniRx;
using UnityEngine;

namespace UXK.HealthSystem
{
    public class HealthControllerContainer : MonoBehaviour, IHealthController
    {
        [SerializeField] private HealthController _healthController;
        
        private void Awake()
        {
            _healthController.OnAwake();
        }

        private void OnDestroy()
        {
            _healthController.OnDestroy();
        }

        private void OnEnable() { }

        public IReadOnlyReactiveProperty<float> Health => _healthController.Health;
        public IReadOnlyReactiveProperty<float> MaxHealth => _healthController.MaxHealth;
        public IReadOnlyReactiveProperty<bool> IsAlive => _healthController.IsAlive;

        public void ApplyHealthChange(HealthChangeInfo healthChangeInfo)
        {
            if (!enabled)
                return;
            
            _healthController.ApplyHealthChange(healthChangeInfo);
        }
    }
}