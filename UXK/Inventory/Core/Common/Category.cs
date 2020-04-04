using System;
using UnityEngine;

namespace UXK.Inventory
{
    [Serializable]
    public class Category : ICategory
    {
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        
        public string Name => _name;
        public string Description => _description;
    }
}