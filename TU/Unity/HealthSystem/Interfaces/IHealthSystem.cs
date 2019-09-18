using UniRx;

namespace TU.Unity.HealthSystem.Interfaces
{
    public interface IHealthSystem{
        IReadOnlyReactiveProperty<float> MaxHealth { get; }
        IReadOnlyReactiveProperty<float> Health{ get; }
        IReadOnlyReactiveProperty<bool> Alive{ get; }
        IReadOnlyReactiveProperty<int> ImmortalityBuffsCount { get; }


        void ReceiveDamage(DamagePack damagePack);
        void ReceiveHeal(HealPack healPack);
        void InitializeHealth(float health, float maxHealth);
    }
}