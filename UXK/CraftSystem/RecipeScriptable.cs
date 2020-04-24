using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UXK.Inventory;

[CreateAssetMenu(fileName = "Recipe", menuName = "Config/Recipe")]
public class RecipeScriptable : ScriptableObject, IRecipe
{
    [SerializeField] private ItemScriptableWithAmount[] _requiredItems = new ItemScriptableWithAmount[1];
    [SerializeField] private ItemScriptableWithAmount[] _resultItems = new ItemScriptableWithAmount[1];

    public bool AdditionalRequirementCheck() => true;
    public IEnumerable<IItemWithAmount> RequiredItems => _requiredItems.Cast<IItemWithAmount>();
    public IEnumerable<IItemWithAmount> ResultItems => _resultItems.Cast<IItemWithAmount>();
    public int Id { get; private set; }

    private void Awake()
    {
        Id = GetInstanceID();
    }

}
