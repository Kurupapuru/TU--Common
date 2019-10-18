using BehavioursManager.Interfaces;
using BehavioursManager;

namespace BehavioursManager.Interfaces
{
    public interface IBehavioursContainer : IBehaviour
    {
        void UpdateSharedVariables();
    }
}
