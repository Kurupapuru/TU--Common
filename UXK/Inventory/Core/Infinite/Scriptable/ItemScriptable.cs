using System.Collections.Generic;
using MessagePack;
using UnityEngine;

namespace UXK.Inventory
{
    [MessagePackFormatter(typeof(ItemScriptableFormatter))]
    [CreateAssetMenu(fileName = "Item", menuName = "Config/Inventory/Item", order = 0)]
    public class ItemScriptable : ScriptableObjectUniq<ItemScriptable>, IItem
    {
        [SerializeField] private Item _item = new Item();


        #region IItem
        public int        Id                => _item.Id;
        public string     Name              => _item.Name;
        public string     Description       => _item.Description;
        public ICategory  GetCategory       => _item.GetCategory;
        public uint       Cost              => _item.Cost;
        public GameObject GetVisualPrefab() => _item.GetVisualPrefab();
        public Sprite     GetIconSprite()   => _item.GetIconSprite();
        #endregion
    }

    public class ItemScriptableFormatter : ScriptableObjectFormatter<ItemScriptable>
    {
    }
}