using TU.Unity.HealthSystem.Interfaces;
using UniRx;
using UnityEngine;

namespace TU.Unity.HealthSystem.HealthDIsplays
{
    public abstract class AbstractHealthDisplay : MonoBehaviour
    {
        protected CompositeDisposable tasks = new CompositeDisposable();

        public void Initialize(IHealthSystem healthSystemReference)
        {
            if (!tasks.IsDisposed || (healthSystemReference == null)) tasks.Dispose();
            healthSystemReference.Health.
                Subscribe(health => 
                              HealthUpdate(health, healthSystemReference.MaxHealth.Value));
            healthSystemReference.MaxHealth.
                Subscribe(maxHealth => HealthUpdate(healthSystemReference.Health.Value, maxHealth));
        }

        public abstract void HealthUpdate(float health, float maxHealth);
    }
}