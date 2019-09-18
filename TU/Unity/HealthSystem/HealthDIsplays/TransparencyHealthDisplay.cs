using System;
using UnityEngine;
using UnityEngine.UI;

namespace TU.Unity.HealthSystem.HealthDIsplays
{
    public class TransparencyHealthDisplay : AbstractHealthDisplay
    {
        [Header("References")] 
        public HealthSystemMonoBehaviour healthReference;
        
        public Image image;

        [Header("Settings")] public bool invert = true;

        private IDisposable connectToPlayerTask;

        private void OnEnable()
        {
            if (healthReference != null)
                Initialize(healthReference);
            else
                Debug.LogError("HEALTH REFERENCE NOT SET");
        }

        private void OnDisable()
        {
            if (!tasks.IsDisposed) tasks.Dispose();
            connectToPlayerTask?.Dispose();
        }

        public override void HealthUpdate(float health, float maxHealth)
        {
            var healthRange = health / maxHealth;

            var color = image.color;
            if (invert)
                color.a = 1 - healthRange;
            else
                color.a = healthRange;

            image.color = color;
        }
    }
}