using UnityEngine;

namespace UXK.HealthSystem
{
    public abstract class HealthChangeType : ScriptableObject
    {
        public string LocalizedName { get; }

        public abstract float ApplyHealthChange(float currentHealth, float amount);
    }
}