namespace TU.Unity.HealthSystem.Interfaces
{
    public interface IDamageDealer
    {
        void DealDamage(IHealthSystem target, float damage);
    }
}