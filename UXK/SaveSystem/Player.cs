using UniRx;
using UnityEngine;
using UXK.CraftSystem.Interface;
using UXK.Inventory;
using UXK.Inventory.View;

namespace UXK.SaveSystem
{
    public class Player : MonoBehaviour
    {
        // Inspector
        [SerializeField] private BagConfigScriptable _bagConfig;
        [SerializeField] private RecipeScriptable[] _defaultRecipes;

        public ReactiveCollection<IRecipe> Recipes;
        public IBag Bag { get; set; }

        private void Awake()
        {
            Bag = new Bag(_bagConfig);
            Recipes = new ReactiveCollection<IRecipe>(_defaultRecipes);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                UiManager.UiManager.Switch<InventoryViewController, IBag>(Bag);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                UiManager.UiManager.Switch<OneItemCraftUi, CraftUiParam>(new CraftUiParam(Recipes, Bag));
            }
        }
    }
}