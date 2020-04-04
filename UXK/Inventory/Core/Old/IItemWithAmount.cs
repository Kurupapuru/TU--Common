using System;
using UnityEngine;

namespace UXK.Inventory
{
    public struct ItemWithAmount : IItemWithAmount
    {
        [SerializeField] private IItem _item;
        [SerializeField] private uint  _amount;


        public IItem Item => _item;
        public uint Amount
        {
            get => _amount;
            internal set => _amount = value;
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
    }

    public interface IItemWithAmount
    {
        IItem Item   { get; }
        uint  Amount { get; }
    }
}