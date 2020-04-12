using UXK.IconGoCreator;

namespace UXK.Inventory
{
    public static class ItemExtensions
    {
        public const float DEFAULT_ICON_SCALE = .5f;
        
        public static IconGo CreateIconGo(this IItem item, float scale = DEFAULT_ICON_SCALE)
        {
            return global::IconGoCreator.Create(item.GetIconSprite(), scale);
        }

        public static IconGo Setup(this IconGo iconGo, IItem item, float scale = DEFAULT_ICON_SCALE)
        {
            return iconGo.Setup(item.GetIconSprite(), scale);
        }
    }
}