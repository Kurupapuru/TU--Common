using System;
using UnityEngine;

namespace UXK.Inventory
{
    [Serializable]
    public class Item : IItem
    {
        [SerializeField] private string     _name         = "Unnamed";
        [SerializeField] private string     _description  = "";
        [SerializeField] private ICategory  _getCategory  = null;
        [SerializeField] private uint       _cost         = 100;
        [SerializeField] private Sprite     _icon         = null;
        [SerializeField] private GameObject _visualPrefab = null;


        #region IItem
        public string    Name        => _name;
        public string    Description => _description;
        public ICategory GetCategory => _getCategory;
        public uint      Cost        => _cost;

        public GameObject GetVisualPrefab() => _visualPrefab;
        public Sprite     GetIconSprite()   => _icon;
        #endregion
    }
}