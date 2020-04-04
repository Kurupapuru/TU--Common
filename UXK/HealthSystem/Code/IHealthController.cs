using UniRx;

namespace UXK.HealthSystem
{
    public interface IHealthController
    {
        IReadOnlyReactiveProperty<float> Health { get; }
        IReadOnlyReactiveProperty<float> MaxHealth { get; }
        IReadOnlyReactiveProperty<bool> IsAlive { get; }
        
        void ApplyHealthChange(HealthChangeInfo healthChangeInfo);
    }
}