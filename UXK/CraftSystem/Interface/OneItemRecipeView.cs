using System;
using System.Linq;
using TU.Sharp.Extensions;
using UnityEngine;
using UnityEngine.UI;
using UXK.Inventory;
using UXK.UnityUtils;

namespace UXK.CraftSystem.Interface
{
    public class OneItemRecipeView : MonoBehaviour, IViewOfHasId<IRecipe>
    {
        [SerializeField] private RawImage resultItemIcon;
        [SerializeField] private Text     resultItemName;
        [SerializeField] private Text     resultItemDescription;
        [SerializeField] private Button   craftButton;

        public RectTransform RectTransform { get; private set; }
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

            if (craftButton!=null)
                craftButton.interactable = isResultItemNotNull;
            resultItemName?.gameObject.SetActive(isResultItemNotNull);
            resultItemDescription?.gameObject.SetActive(isResultItemNotNull);
            resultItemIcon?.gameObject.SetActive(isResultItemNotNull);
            
            if (isResultItemNotNull)
            {
                if (resultItemName != null)
                    resultItemName.text = recipeResult.Item.Name;

                if (resultItemDescription != null)
                    resultItemDescription.text = recipeResult.Item.Description;

                if (resultItemIcon != null)
                    resultItemIcon.SetSprite(recipeResult.Item.GetIconSprite());
            }
        }

        private void Awake()
        {
            RectTransform = (RectTransform) transform;
        }
    }
}