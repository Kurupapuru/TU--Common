using UnityEngine;

namespace UXK.Inventory
{
    [CreateAssetMenu(fileName = "Bag Config", menuName = "Config/Inventory/Bag Config", order = 0)]
    public class BagConfigScriptable : ScriptableObject, IBagConfig
    {
        [SerializeField] private BagConfig _bagConfig;
        
        #region IBagConfig
        public string    Name        => _bagConfig.Name;
        public string    Description => _bagConfig.Description;
        public ICategory GetCategory => _bagConfig.GetCategory;
        public uint      Cost        => _bagConfig.Cost;

        public GameObject GetVisualPrefab() => _bagConfig.GetVisualPrefab();
        public Sprite     GetIconSprite()   => _bagConfig.GetIconSprite();
        #endregion
    }
}