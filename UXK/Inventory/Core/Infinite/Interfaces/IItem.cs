using UnityEngine;

namespace UXK.Inventory
{
    public interface IItem
    {
        string    Name        { get; }
        string    Description { get; }
        ICategory GetCategory { get; }
        uint      Cost        { get; }

        GameObject GetVisualPrefab();
        Sprite     GetIconSprite();
    }
}