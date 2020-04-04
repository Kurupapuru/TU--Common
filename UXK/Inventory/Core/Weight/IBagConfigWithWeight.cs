namespace UXK.Inventory
{
    public interface IBagConfigWithWeight : IBagConfig
    {
        float MaxWeight        { get; }
        float WeightMultiplier { get; }
    }
}