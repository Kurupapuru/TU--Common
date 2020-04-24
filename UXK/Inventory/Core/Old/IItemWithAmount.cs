using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UXK.Inventory
{
    [Serializable]
    public struct ItemWithAmount : IItemWithAmount
    {
        [SerializeField] private IItem _item;
        [SerializeField] private uint  _amount;

        public ItemWithAmount(IItem item, uint amount)
        {
            _item = item;
            _amount = amount;
        }

        public IItem Item => _item;
        public uint Amount
        {
            get => _amount;
            internal set => _amount = value;
        }
        public int Id => _item.Id;

        public static IEnumerable<ItemWithAmount> ConcatSameItems(IEnumerable<IItemWithAmount> items)
        {
            var dict = new Dictionary<IItem, uint>(items.Count());
            foreach (var itemWithAmount in items)
            {
                if (dict.TryGetValue(itemWithAmount.Item, out var existed))
                    dict[itemWithAmount.Item] = existed + itemWithAmount.Amount;
                else
                    dict[itemWithAmount.Item] = itemWithAmount.Amount;
            }

            return dict.Select(x => new ItemWithAmount(x.Key, x.Value));
        }
    }

    [Serializable]
    public struct ItemScriptableWithAmount : IItemWithAmount
    {
        [SerializeField] private ItemScriptable _item;
        [SerializeField] private uint  _amount;

        public IItem Item => _item;
        public uint Amount
        {
            get => _amount;
            internal set => _amount = value;
        }
        public int Id => _item.Id;
    }

    public interface IItemWithAmount : IHasId
    {
        IItem Item   { get; }
        uint  Amount { get; }
    }
}