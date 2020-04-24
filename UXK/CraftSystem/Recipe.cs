using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXK.Inventory;
using Random = UnityEngine.Random;

public interface IRecipe : IHasId
{
    bool AdditionalRequirementCheck();
    IEnumerable<IItemWithAmount> RequiredItems { get; }
    IEnumerable<IItemWithAmount> ResultItems { get; }
}

[Serializable]
public class Recipe : IRecipe
{
    [SerializeField] private IItemWithAmount[] bf_requiredItems;
    [SerializeField] private IItemWithAmount[] bf_resultItems;
    [SerializeField] private int bf_id = Random.Range(int.MinValue, int.MaxValue);

    public Recipe(IItemWithAmount[] bfRequiredItems, IItemWithAmount[] bfResultItems)
    {
        bf_requiredItems = bfRequiredItems;
        bf_resultItems = bfResultItems;
    }
    
    // Interface
    public bool AdditionalRequirementCheck() => true;
    public IEnumerable<IItemWithAmount> RequiredItems { get; }
    public IEnumerable<IItemWithAmount> ResultItems { get; }

    public int Id => bf_id;
}
