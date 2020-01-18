using TU.Unity.HealthSystem.Interfaces;
using TU.Unity.Tools;
using UniRx;
using UnityEngine;

namespace TU.Unity.HealthSystem.ForPrototyping
{
    public class RestartOnDeath : MonoBehaviour
    {
        public HealthSystemMonoBehaviour healthSystemMonoBehaviour;
        public IHealthSystem healthSystem;

        private void Awake()
        {
            if (healthSystemMonoBehaviour != null)
                healthSystem = healthSystemMonoBehaviour;
            else
                healthSystem = GetComponent<IHealthSystem>();

            healthSystem.Alive.Where(x => !x).Subscribe(_ => OnDeath());
        }

        public void OnDeath()
        {
            UnityTools.Scene.RestartScene(s => true);
        }
    }
}
