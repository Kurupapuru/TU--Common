using MessagePack;

namespace UXK.Inventory
{
    [Union(0, typeof(BagConfig))]
    [Union(1, typeof(BagConfigScriptable))]
    public interface IBagConfig : IItem
    {
    }
}