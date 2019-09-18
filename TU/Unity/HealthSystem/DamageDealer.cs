using TU.Unity.HealthSystem.Interfaces;

namespace TU.Unity.HealthSystem
{
    public class DamageDealer : IDamageDealer
    {
        public DamageType currentDamageType = DamageType.Normal;

        public DamageDealer(DamageType damageType = DamageType.Normal)
        {
            this.currentDamageType = damageType;
        }
        
        
        public void DealDamage(IHealthSystem target, float damage)
        {
            target.ReceiveDamage(
                HandleDamage(damage));
        }
        
        protected virtual DamagePack HandleDamage(float damage)
        {
            return new DamagePack(damage, DamageType.Normal);
        }
    }
}