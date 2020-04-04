using UXK.Inventory;

namespace GameSystems.Inventory
{
    public interface IBagConfig : IItem
    {
        float MaxWeight { get; }
        float WeightMultiplier { get; }
    }
}