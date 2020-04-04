using UnityEngine;

namespace UXK.HealthSystem
{
    [CreateAssetMenu(menuName = "Config/Normal Health Change Type")]
    public class NormalHealthChangeType : HealthChangeType
    {
        public float multiplier = 1;

        public override float ApplyHealthChange(float currentHealth, float amount)
            => currentHealth + amount * multiplier;
    }
}