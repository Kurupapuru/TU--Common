using System;
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