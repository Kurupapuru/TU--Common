using UnityEngine;
using UnityEngine.UI;
using UXK.UnityUtils;

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
            {
                _iconImage.texture = null;
                _nameText.text = "Null Item";
                _countText.text = value.Amount.ToString();
                Debug.LogError("Item is null");
                return;
            }

            var sprite = value.Item.GetIconSprite();
            if (sprite != null)
                _iconImage.SetSprite(sprite);
            else
                _iconImage.texture = null;
            _nameText.text = value.Item.Name;
            _countText.text = value.Amount.ToString();
        }
    }
}