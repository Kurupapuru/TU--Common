using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UXK.CraftSystem.Interface;
using UXK.Inventory;
using UXK.UiManager;

public class OneItemCraftUi : MonoBehaviour, IUiWindowWithParam<CraftUiParam>
{
    // Inspector
    [SerializeField] private OneItemRecipeView _fullRecipeView;
    [SerializeField] private RecipesScroller   _recipesScroller;
    [SerializeField] private IBag              _userBag;

    private CompositeDisposable _disposables = new CompositeDisposable(1);

    public bool Enabled => gameObject.activeSelf;
    public void Hide()  => gameObject.SetActive(false);

    public void ShowFor(CraftUiParam param)
    {
        _fullRecipeView.SetupFor(null);

        this._userBag = param.Bag;
        _recipesScroller.Initialize(param.Recipes, OnViewSpawn);
        gameObject.SetActive(true);
        SubscribeUpdateCanCraft();
        UpdateCanCraft();
    }

    private void SubscribeUpdateCanCraft()
    {
        _disposables.Clear();
        if (_userBag == null) return;
        _userBag.Items.ObserveAdd()    .Subscribe(_ => UpdateCanCraft()).AddTo(_disposables);
        _userBag.Items.ObserveRemove() .Subscribe(_ => UpdateCanCraft()).AddTo(_disposables);
        _userBag.Items.ObserveReplace().Subscribe(_ => UpdateCanCraft()).AddTo(_disposables);
        _userBag.Items.ObserveReset()  .Subscribe(_ => UpdateCanCraft()).AddTo(_disposables);
    }
    
    private void OnEnable()
    {
        SubscribeUpdateCanCraft();
    }

    private void OnDisable()
    {
        _disposables.Clear();
    }

    private void OnViewSpawn(OneItemRecipeView view)
    {
        view.button.onClick.RemoveAllListeners();
        view.button.onClick.AddListener(() => SelectRecipe(view.Value));
    }

    private void SelectRecipe(IRecipe viewValue)
    {
        _fullRecipeView.SetupFor(viewValue);
        _fullRecipeView.button.onClick.RemoveAllListeners();
        _fullRecipeView.button.onClick.AddListener(Craft);
        UpdateCanCraft();
    }

    private void UpdateCanCraft()
    {
        if (_fullRecipeView.Value == null)
        {
            _fullRecipeView.button.interactable = false;
            return;
        }
        
        var bagCopy = _userBag.CreateCopy();
        var canCraft =
            bagCopy.RemoveItems(_fullRecipeView.Value.RequiredItems) &&
            bagCopy.CanAddItems(_fullRecipeView.Value.ResultItems);
        _fullRecipeView.button.interactable = canCraft;
    }

    private void Craft()
    {
        if (!_userBag.RemoveItems(_fullRecipeView.Value.RequiredItems))
        {
            Debug.LogError("Can't remove required for recipe items, this must not be possible, button for craft should be disabled");
            return;
        }

        if (!_userBag.AddItems(_fullRecipeView.Value.ResultItems))
        {
            Debug.LogError("Can't add recipe result items, this must not be possible, button for craft should be disabled. Trying to restore removed required items");
            if (!_userBag.AddItems(_fullRecipeView.Value.RequiredItems))
            {
                throw new Exception("CRITICAL ERROR: Required items was removed but can't be added back");
            }
        }
    }
}