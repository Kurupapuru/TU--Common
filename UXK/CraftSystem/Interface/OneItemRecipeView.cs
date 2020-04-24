using System;
using System.Linq;
using TU.Sharp.Extensions;
using UnityEngine;
using UnityEngine.UI;
using UXK.Inventory;
using UXK.Inventory.View;
using UXK.UnityUtils;

namespace UXK.CraftSystem.Interface
{
    public class OneItemRecipeView : MonoBehaviour, IViewOfHasId<IRecipe>
    {
        [Header("Result Item")]
        [SerializeField] private RawImage resultItemIcon;
        [SerializeField] private Text     resultItemName;
        [SerializeField] private Text     resultItemDescription;
        [SerializeField] private Text     resultItemAmount;
        [Header("Required Items")]
        [SerializeField] private IItemWithAmountView _requiredItemPrefab;
        [SerializeField] private RectTransform _requiredItemsParent;
        [Space]
        [SerializeField] public  Button   button;

        public RectTransform RectTransform
        {
            get
            {
                if (bf_RectTransform == null)
                    bf_RectTransform = (RectTransform) transform;
                return bf_RectTransform;
            }
        }
        private RectTransform bf_RectTransform;
        public IRecipe       Value         { get; private set; }

        public void SetupFor(IRecipe value)
        {
            Value = value;
            IItemWithAmount recipeResult;
            if (value == null || value.ResultItems == null || !value.ResultItems.IsHaveCount(1))
                recipeResult = null;
            else
                recipeResult = value.ResultItems.First();

            var isResultItemNotNull = recipeResult != null;

            resultItemName?.gameObject.SetActive(isResultItemNotNull);
            resultItemDescription?.gameObject.SetActive(isResultItemNotNull);
            resultItemIcon?.gameObject.SetActive(isResultItemNotNull);
            resultItemAmount?.gameObject.SetActive(isResultItemNotNull);

            if (_requiredItemsParent != null)
            {
                var requiredItemChildsCount = _requiredItemsParent.childCount;
                for (int i = requiredItemChildsCount - 1; i >= 0; i--)
                {
                    Destroy(_requiredItemsParent.GetChild(i).gameObject);
                }

                if (isResultItemNotNull)
                {
                    foreach (var requiredItem in Value.RequiredItems)
                        Instantiate(_requiredItemPrefab.gameObject, _requiredItemsParent).GetComponent<IItemWithAmountView>().SetupFor(requiredItem);
                }
            }
            
            if (isResultItemNotNull)
            {
                if (resultItemName != null)
                    resultItemName.text = recipeResult.Item.Name;

                if (resultItemDescription != null)
                    resultItemDescription.text = recipeResult.Item.Description;

                if (resultItemIcon != null)
                    resultItemIcon.SetSprite(recipeResult.Item.GetIconSprite());

                if (resultItemAmount != null)
                    resultItemAmount.text = recipeResult.Amount.ToString();
            }
        }
    }
}