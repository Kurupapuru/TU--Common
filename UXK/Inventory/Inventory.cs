using System;
using UniRx;
using UnityEngine;

namespace UXK.Inventory
{
    public class Inventory : MonoBehaviour
    {
        // Inspector
        [SerializeField] private BagConfigScriptable _bagConfig;

        public Bag Bag
        {
            get
            {
                if (bf_bag == null)
                    bf_bag = new Bag(_bagConfig);
                return bf_bag;
            }
        }
        private Bag bf_bag;
        
#if UNITY_EDITOR
        [Header("Debugging")]
        [SerializeField] private bool debug;
        [ContextMenuItem("Add", "AddTestItem")]
        [ContextMenuItem("Remove", "RemoveTestItem")]
        [SerializeField] private ItemScriptableWithAmount _testItem;

        private CompositeDisposable _debugDisposables = new CompositeDisposable();
        
        private void Start()
        {
            if (debug)
            {
                Bag.Items.ObserveAdd()
                    .Subscribe(x => Debug.Log($"Added \"{x.Key.Name}:{x.Value}\""))
                    .AddTo(_debugDisposables);
                Bag.Items.ObserveRemove()
                    .Subscribe(x => Debug.Log($"Removed \"{x.Key.Name}:{x.Value}\""))
                    .AddTo(_debugDisposables);
                Bag.Items.ObserveReplace()
                    .Subscribe(x => Debug.Log($"\"{x.Key.Name}\" changed from {x.OldValue} to {x.NewValue}"))
                    .AddTo(_debugDisposables);
            }
        }

        private void OnDestroy()
        {
            _debugDisposables.Dispose();
        }

        // Invoked from [ContextMenuAttribute] of _testItem
        private void AddTestItem() => Bag.AddItem(_testItem);

        // Invoked from [ContextMenuAttribute] of _testItem
        private void RemoveTestItem() => Bag.RemoveItem(_testItem);
#endif
    }
}