using System;
using UnityEngine;

namespace UXK.Inventory
{
    [CreateAssetMenu(fileName = "ItemCategory", menuName = "Config/Inventory/Item Category", order = 0)]
    public class CategoryScriptable : ScriptableObject, ICategory
    {
        [SerializeField] private Category _category;
        
        public string Name => _category.Name;
        public string Description => _category.Description;
    }
}