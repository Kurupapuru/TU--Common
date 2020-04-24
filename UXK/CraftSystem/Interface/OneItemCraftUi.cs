using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UXK.CraftSystem.Interface;
using UXK.UiManager;

public class OneItemCraftUi : MonoBehaviour, IUiWindowWithParam<IReactiveCollection<IRecipe>>
{
    // Inspector
    [SerializeField] private OneItemRecipeView _recipeLineViewPrefab;
    [SerializeField] private OneItemRecipeView _fullRecipeView;
    [SerializeField] private RecipesScroller _recipesScroller;
    
    public bool Enabled => gameObject.activeSelf;
    public void Hide() => gameObject.SetActive(false);
    public void ShowFor(IReactiveCollection<IRecipe> param)
    {
        _recipesScroller.Initialize(param);
        gameObject.SetActive(true);
    }
}
