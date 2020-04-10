using System;
using System.Linq;
using MessagePack;
using TU.Sharp.Extensions;
using UniRx;
using UnityEngine;

namespace UXK.Inventory
{
    [MessagePackObject(), Serializable]
    public class Bag : IBag, IItem
    {
        #region IItem
        [IgnoreMember] public string Name        => Config.Name;
        [IgnoreMember] public string Description => Config.Description;
        [IgnoreMember] public ICategory GetCategory => Config.GetCategory;
        [IgnoreMember] public uint Cost
        {
            get
            {
                var result = Config.Cost;
                Items.ForEach(x => result += x.Key.Cost * x.Value);
                return result;
            }
        }

        public GameObject GetVisualPrefab() => Config.GetVisualPrefab();

        public Sprite GetIconSprite() => Config.GetIconSprite();
        #endregion
        
        #region IBag
        [IgnoreMember] public IBag                          ParentBag { get => _parentBag; set => _parentBag = value; }
        [IgnoreMember] public IBagConfig                    Config    => _config;
        [IgnoreMember] public IReadOnlyReactiveDictionary<IItem, uint> Items     => _items;

        public virtual bool AddItems(params IItemWithAmount[] items)
        {
            var canAddItems = CanAddItems(items); 
            
            if (canAddItems)
                foreach (var itemWithAmount in items)
                    AddItemForce(itemWithAmount);
            
            return canAddItems;
        }


        #region Add
        public virtual bool AddItem(IItemWithAmount item)
        {
            var canAddItem = CanAddItem(item); 
            
            if (canAddItem)
                AddItemForce(item);
            
            return canAddItem;
        }

        private void AddItemForce(IItemWithAmount addItem)
        {
            if (_items.TryGetValue(addItem.Item, out var amount))
                _items[addItem.Item] = amount + addItem.Amount;
            else
                _items[addItem.Item] = addItem.Amount;
        }

        public virtual bool CanAddItems(params IItemWithAmount[] items)
        {
            return true;
        }

        public virtual bool CanAddItem(IItemWithAmount item)
        {
            return true;
        }
        #endregion
        
        #region Remove
        public bool RemoveItem(IItemWithAmount item)
        {
            var canRemove = CanRemoveItem(item);
            if (canRemove)
                RemoveItemForce(item);
            return canRemove;
        }

        public bool RemoveItems(params IItemWithAmount[] items)
        {
            var canRemove = CanRemoveItems(items);
            if (canRemove)
                items.ForEach(RemoveItemForce);
            return canRemove;
        }
        private void RemoveItemForce(IItemWithAmount item)
        {
            var newAmount = _items[item.Item] - item.Amount;
            if (newAmount == 0)
                _items.Remove(item.Item);
            else
                _items[item.Item] = newAmount;
        }

        public bool CanRemoveItem(IItemWithAmount item)
        {
            if (_items.TryGetValue(item.Item, out var existedAmount))
                return item.Amount <= existedAmount;
            else
                return item.Amount == 0;
        }

        public bool CanRemoveItems(params IItemWithAmount[] items) => items.All(CanRemoveItem);
        #endregion
        #endregion
        
        #region IHasId
        private int _nameHashCode = -1;
        public int Id
        {
            get
            {
                if (_nameHashCode == -1)
                    _nameHashCode = Name.GetHashCode();
                return _nameHashCode;
            }
        }
        #endregion
        
        [IgnoreMember] private IBag                 _parentBag;
        [Key(0), SerializeField] private IBagConfig           _config;
        [Key(1), SerializeField] private ReactiveDictionary<IItem, uint> _items;

        public Bag(IBagConfig config, IBag parentBag = null, ReactiveDictionary<IItem, uint> items = null)
        {
            _config = config;
            
            _parentBag = parentBag;
            
            if (items == null)
                _items = new ReactiveDictionary<IItem, uint>();
            else
                _items = items;
        }

        // For MessagePack
        public Bag(IBagConfig config, ReactiveDictionary<IItem, uint> items)
        {
            this._config = config;
            this._items = items;
        }

        public Bag()
        {
        }
    }
}

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

