using System.Collections.Generic;
using GameSystems.Inventory;
using GameSystems.Inventory.Abstract;
using TU.Common.Generatable.Inventory.Abstract;
using TU.Sharp.Extensions;
using UnityEngine;

namespace UXK.Inventory
{
    public class Bag : IBag, IItem
    {
        private IBag _parentBag;
        private IBagConfig _config;
        private List<ItemWithAmount> _items;

        //         [SerializeField] private BagConfig  config;
//         public                   IBagConfig Config => config;
//
//         public IBag ParentBag                   { get; private set; }
//         public bool SubWeightMultipliersAllowed => config.subWeightMultipliers;
//
//         public Bag(IBag parentBag)
//         {
//             ParentBag = parentBag;
//         }
//
//
//         private readonly List<ItemWithCount>          _items = new List<ItemWithCount>();
//         public           IReadOnlyList<ItemWithCount> Items => _items;
//
//
//         public bool AddItems(params ItemWithCount[] items)
//         {
//             var canAdd = CanAddItems(items);
//             if (canAdd) items.ForEach(AddItemForce);
//             return canAdd;
//         }
//
//         public bool AddItem(ItemWithCount addItem)
//         {
//             var canAdd = CanAddItem(addItem);
//             if (canAdd) AddItemForce(addItem);
//             return canAdd;
//         }
//
//         private void AddItemForce(ItemWithCount addItem)
//         {
//             if (!_items.TryModifyElement(
//                 x => x.Item == addItem.Item,
//                 (ref ItemWithCount x) => x.Count += addItem.Count))
//             {
//                 _items.Add(addItem);
//             }
//         }
//
//
//         public bool CanAddItem(ItemWithCount item)
//         {
//             bool result = true;
// #if INV_WEIGHT
//             if (result)
//             {
//                 var needEmptyWeight = item.Item.Weight * item.Count;
//                 var canAddWeight = GetCanAddWeightRecursive();
//
//                 result = needEmptyWeight <= canAddWeight;
//             }
// #endif
//             return result;
//         }
//
//         public bool CanAddItems(params ItemWithCount[] items)
//         {
//             bool result = true;
//             
// #if INV_WEIGHT
//             if (result)
//             {
//                 float needEmptyWeight = 0;
//                 foreach (var itemWithCount in items)
//                 {
//                     needEmptyWeight += itemWithCount.Item.Weight * itemWithCount.Count;
//                 }
//                 var canAddWeight = GetCanAddWeightRecursive();
//
//                 result = needEmptyWeight <= canAddWeight;
//             }
// #endif
//             return result;
//         }
//
//
// #if INV_WEIGHT
//         
//         
// #endif

public string Name => Config.Name;
public string Description => Config.Description;
public ICategory GetCategory => Config.GetCategory;
public uint Cost
{
    get
    {
        var result = Config.Cost;
        Items.ForEach(x => result += x.Item.Cost * x.Amount);
        return result;
    }
}

public GameObject GetVisualPrefab() => Config.GetVisualPrefab();

public Sprite GetIconSprite() => Config.GetIconSprite();

public IBag ParentBag => _parentBag;
public IBagConfig Config => _config;
public IReadOnlyList<ItemWithAmount> Items => _items;

public virtual bool AddItems(params ItemWithAmount[] items)
{
    _items.AddRange(items);
    return true;
}

public virtual bool AddItem(ItemWithAmount items)
{
    _items.
    _items.first
    _items.Add(items);
    return true;
}

public virtual bool CanAddItems(params ItemWithAmount[] items)
{
    return true;
}

public virtual bool CanAddItem(ItemWithAmount item)
{
    return true;
}