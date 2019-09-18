namespace TU.Unity.HealthSystem.Interfaces
{
    public interface IHealDealer
    {
        void Heal(IHealthSystem target, HealPack healpack);
    }
}