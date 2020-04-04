using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace UXK.HealthSystem
{
    [Serializable]
    public class HealthController : IHealthController
    {
        [SerializeField] private FloatReactiveProperty _health = new FloatReactiveProperty(100);
        [SerializeField] private FloatReactiveProperty _maxHealth = new FloatReactiveProperty(100);
        private ReactiveProperty<bool> _isAlive = new ReactiveProperty<bool>(true);
        
        public IReadOnlyReactiveProperty<float> Health => _health;
        public IReadOnlyReactiveProperty<float> MaxHealth => _maxHealth;
        public IReadOnlyReactiveProperty<bool> IsAlive => _isAlive;

        private CompositeDisposable _disposables = new CompositeDisposable();
        
        public void OnAwake()
        {
            Health.Subscribe(health => _isAlive.Value = health > 0)
                .AddTo(_disposables);
        }

        public void OnDestroy()
        {
            _disposables.Dispose();
        }

        public void ApplyHealthChange(HealthChangeInfo healthChangeInfo)
        {
            var newHealth = healthChangeInfo.type.ApplyHealthChange(Health.Value, healthChangeInfo.amount);

            if (IsAlive.Value)
            {
                SetHealth(newHealth);
            } else if (newHealth > 0)
            {
                if (!HealthSystemSettings.Instance.canResurrectTypes.Contains(healthChangeInfo.type))
                    return;
                
                SetHealth(newHealth);
            }
        }

        private void AddHealth(float addHealth) => SetHealth(Health.Value + addHealth);
        private void SetHealth(float newHealth) => _health.Value = Mathf.Clamp(newHealth, 0, MaxHealth.Value);
    }
}