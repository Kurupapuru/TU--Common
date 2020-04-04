using UXK.Inventory;

namespace TU.Common.Generatable.Inventory.Abstract
{
    public struct ItemWithAmount
    {
        public IItem Item { get; }
        public uint Amount { get; internal set; }
    }
}