using TU.Unity.HealthSystem.Interfaces;

namespace TU.Unity.HealthSystem
{
    public class HealDealer : IHealDealer
    {
        public void Heal(IHealthSystem target, HealPack healpack) => target.ReceiveHeal(healpack);

        public void Heal(IHealthSystem target, float value) => Heal(target, new HealPack(value));
    }
}