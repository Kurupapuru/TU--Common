using MessagePack;
using UnityEngine;

namespace UXK.Inventory
{
    [Union(0, typeof(BagConfig))]
    [Union(1, typeof(BagConfigScriptable))]
    [Union(2, typeof(Item))]
    [Union(3, typeof(ItemScriptable))]
    [Union(4, typeof(Bag))]
    public interface IItem : IHasId
    {
        string    Name        { get; }
        string    Description { get; }
        ICategory GetCategory { get; }
        uint      Cost        { get; }

        GameObject GetVisualPrefab();
        Sprite     GetIconSprite();
    }
}