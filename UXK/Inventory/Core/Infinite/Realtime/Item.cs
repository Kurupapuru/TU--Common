using System;
using MessagePack;
using UnityEngine;

namespace UXK.Inventory
{
    [Serializable, MessagePackObject(true)]
    public class Item : IItem
    {
        [SerializeField] [Key("name")]         private string     _name         = "Unnamed";
        [SerializeField] [Key("description")]  private string     _description  = "";
        [SerializeField] [Key("getCategory")]  private ICategory  _getCategory  = null;
        [SerializeField] [Key("cost")]         private uint       _cost         = 100;
        [SerializeField] [Key("icon")]         private Sprite     _icon         = null;
        [SerializeField] [Key("visualPrefab")] private GameObject _visualPrefab = null;


        #region IItem
        [IgnoreMember] public string    Name        => _name;
        [IgnoreMember] public string    Description => _description;
        [IgnoreMember] public ICategory GetCategory => _getCategory;
        [IgnoreMember] public uint      Cost        => _cost;

        public GameObject GetVisualPrefab() => _visualPrefab;
        public Sprite     GetIconSprite()   => _icon;
        #endregion


        #region IHasId
        private int _nameHashCode = -1;
        public int Id
        {
            get
            {
                if (_nameHashCode == -1)
                    _nameHashCode = Name.GetHashCode();
                return _nameHashCode;
            }
        }
        #endregion
    }
}