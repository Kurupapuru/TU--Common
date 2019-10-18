using KurupapuruLab.SharedInterfaces;

namespace BehavioursManager.Interfaces
{
    public interface IBehaviour : IInstantiatable<IBehaviour>, IEnabable
    {
        void UpdateSharedVariables(BehavioursContainer container);
    }
}
