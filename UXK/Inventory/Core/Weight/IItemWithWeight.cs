namespace UXK.Inventory.Weight
{
    public interface IItemWithWeight : IItem
    {
        float Weight { get; }
    }
}