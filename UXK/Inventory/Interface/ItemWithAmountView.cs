using UnityEngine;
using UnityEngine.UI;

namespace UXK.Inventory.View
{
    public class ItemWithAmountView : MonoBehaviour, IViewOfHasId<ItemWithAmount>
    {
        // Inspector
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RawImage _iconImage;
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _countText;
        
        public RectTransform RectTransform => _rectTransform;
        public ItemWithAmount Value { get; private set; }

        public void SetupFor(ItemWithAmount value)
        {
            Value = value;

            if (value.Item == null)
                Debug.LogError("Item is null");


            var sprite = value.Item.GetIconSprite();
            if (sprite != null)
                _iconImage.texture = sprite.texture;
            else
                _iconImage.texture = null;
            _nameText.text = value.Item.Name;
            _countText.text = value.Amount.ToString();
        }
    }
}