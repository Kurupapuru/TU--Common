using System;
using System.Linq;
using TU.Sharp.Extensions;
using UniRx;
using UnityEngine;
using UXK.Inventory;

namespace UXK.InfiniteScroller
{
    public class ReactiveLinkedInfiniteScroller<TItem, TView> : InfiniteScroller<TItem, TView>
        where TItem : IHasId where TView : Component, IViewOfHasId<TItem>
    {
        private IReactiveCollection<TItem> itemsReactive;
        private CompositeDisposable _disposables = new CompositeDisposable();

        public void Initialize(IReactiveCollection<TItem> items, Action<TView> onViewSpawn = null)
        {
            Initialize(items.ToList(), onViewSpawn);
            AddSubscriptions(items);
        }

        private void OnEnable()
        {
            if (itemsReactive!=null)
                AddSubscriptions(itemsReactive);
        }

        private void OnDisable()
        {
            _disposables.Clear();
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }


        public void AddSubscriptions(IReactiveCollection<TItem> items)
        {
            this.itemsReactive = items;
            _disposables.Clear();
            items.ObserveAdd().Subscribe(x =>
            {
                OnBeforeListChange();
                itemsList.Add(x.Value);
            }).AddTo(_disposables);
            items.ObserveRemove().Subscribe(x =>
            {
                OnBeforeListChange();
                itemsList.Remove(x.Value);
            }).AddTo(_disposables);
            items.ObserveReplace().Subscribe(replaceEvent =>
            {
                OnBeforeListChange();
                var index = itemsList.FindIndex(x => x.Id == replaceEvent.OldValue.Id);
                itemsList[index] = replaceEvent.NewValue;
            }).AddTo(_disposables);
        }
    }
}