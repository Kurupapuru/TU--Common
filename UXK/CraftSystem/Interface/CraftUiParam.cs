using UniRx;
using UXK.Inventory;

namespace UXK.CraftSystem.Interface
{
    public struct CraftUiParam
    {
        public IReactiveCollection<IRecipe> Recipes;
        public IBag Bag;

        public CraftUiParam(IReactiveCollection<IRecipe> recipes, IBag bag)
        {
            Recipes = recipes;
            Bag = bag;
        }
    }
}