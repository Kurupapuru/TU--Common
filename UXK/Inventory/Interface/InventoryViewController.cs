using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UXK.UiManager;

namespace UXK.Inventory.View
{
    public class InventoryViewController : MonoBehaviour, IUiWindowWithParam<IBag>
    {
        [SerializeField] private ItemWithAmountScroller _infScroll;
        [SerializeField] private Button _closeButton;
        
        public bool Enabled => gameObject.activeSelf;
        
        private IBag _bag;
        private CompositeDisposable _disposables = new CompositeDisposable();

        private List<ItemWithAmount> _items;
        

        public void Hide()  => gameObject.SetActive(false);
        
        public void ShowFor(IBag bag)
        {
            _bag = null;
            gameObject.SetActive(true);
            _bag = bag;
            
            _items = bag.Items.Select(x => new ItemWithAmount(x.Key, x.Value)).ToList();
            _infScroll.Initialize(_items);
            
            AddSubscriptions(bag);
        }

        private void AddSubscriptions(IBag bag)
        {
            _disposables.Clear();
            _bag.Items.ObserveAdd().Subscribe(x =>
            {
                _infScroll.OnBeforeListChange();
                _items.Add(new ItemWithAmount(x.Key, x.Value));
            }).AddTo(_disposables);
            _bag.Items.ObserveRemove().Subscribe(x =>
            {
                _infScroll.OnBeforeListChange();
                _items.Remove(new ItemWithAmount(x.Key, x.Value));
            }).AddTo(_disposables);
            _bag.Items.ObserveReplace().Subscribe(x =>
            {
                _infScroll.OnBeforeListChange();
                var index = _items.FindIndex(v => v.Id == x.Key.Id);
                _items[index] = new ItemWithAmount(x.Key, x.NewValue);
            }).AddTo(_disposables);
        }

        private void Start()
        {
            _closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        }

        private void OnEnable()
        {
            if (_bag != null)
                AddSubscriptions(_bag);
        }

        private void OnDisable()
        {
            _disposables.Clear();
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}