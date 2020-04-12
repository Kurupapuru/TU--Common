using System;
using DG.Tweening;
using TU.Sharp.Extensions;
using UnityEngine;
using UXK.IconGoCreator;
using UXK.Inventory;
using UXK.SaveSystem;

public class PickableItem : MonoBehaviour
{
    [SerializeField] private ItemScriptableWithAmount _defaultItem;
    [Space]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Collider _selfTrigger;
    [SerializeField] private IItemWithAmount _item;
    [SerializeField] private IconGo _itemIcon;
    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.Linear(1,1,0,0);
    [SerializeField] private AnimationCurve globalCurve = AnimationCurve.Linear(0,0,1,1);
    [SerializeField] private float _duration = 1;
    [SerializeField] private Vector3 endPointOffset = new Vector3(0, 1, 0);

    private Tweener[] _tweeners;
    
    public PickableItem Setup(IItemWithAmount item, Vector3 position, Vector3 velocity = default)
    {
        // Reset
        _tweeners?.ForEach(x =>
        {
            x.Rewind();
            x.Kill();
        });
        _selfTrigger.enabled = true;
        _rb.angularVelocity = Vector3.zero;
        
        // Setup
        _rb.position = position;
        _item = item;
        _itemIcon.Setup(_item.Item);
        _rb.velocity = velocity;
        return this;
    }
    
    private void OnTriggerEnter(Collider c)
    {
        var player = c.GetComponent<Player>();
        if (player == null) return;


        _selfTrigger.enabled = false;

        if (_tweeners == null)
            _tweeners = new Tweener[2];

        _tweeners[0] = DOTween.To(x => { _itemIcon.transform.localScale = Vector3.one * x; },
                0, 1, _duration)
            .SetEase(scaleCurve);

        _tweeners[1] = DOTween.To(x => { _itemIcon.transform.position = Vector3.Lerp(transform.position, player.transform.position + endPointOffset, x); },
                0, 1, _duration)
            .SetEase(globalCurve)
            .OnComplete(() =>
            {
                player.Bag.AddItem(_item);
                Destroy(_rb.gameObject);
            });
    }

    private void Start()
    {
        if (_item == null)
            Setup(_defaultItem, transform.position);
    }
}
