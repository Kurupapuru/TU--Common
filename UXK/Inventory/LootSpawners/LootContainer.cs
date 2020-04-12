using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using UXK.HealthSystem;
using UXK.Inventory;


public class LootContainer : MonoBehaviour
{
    [Required]
    [SerializeField] private HealthControllerContainer _health;
    [SerializeField] private Vector3 spawnOffset = new Vector3(0,1,0);
    [SerializeField] private float horizontalImpulse = 2f, verticalImpulse = 1f;
    [Space]
    [SerializeField] private ItemScriptableWithAmount[] _additionalItems;
    
    private IItemWithAmount[] _items;
    private IDisposable _sub;

    public void Setup(IItemWithAmount[] items)
    {
        _items = new IItemWithAmount[items.Length + _additionalItems.Length];
        for (int i = 0; i < items.Length; i++)
            _items[i] = items[i];
        for (int i = items.Length; i < _additionalItems.Length; i++)
            _items[i] = _additionalItems[i];
    }

    private void Start()
    {
        if (_items == null)
            _items = Array.ConvertAll(_additionalItems, x => (IItemWithAmount)x);
    }

    private void OnEnable()
    {
        _sub = _health.IsAlive.Subscribe(AliveChanged);
    }

    private void OnDisable()
    {
        _sub.Dispose();
    }

    private void AliveChanged(bool isAlive)
    {
        if (isAlive) return;

        var spawnPos = transform.TransformPoint(spawnOffset);
        Debug.DrawRay(spawnPos, Vector3.up, Color.red, 1f);
        foreach (var itemWithAmount in _items)
        {
            PickableItemCreator.Create(itemWithAmount, spawnPos, horizontalImpulse, verticalImpulse);
        }
        
        Destroy(gameObject);
    }
}
