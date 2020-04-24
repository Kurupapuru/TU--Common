using UnityEngine;
using UXK.Inventory;
using UXK.Inventory.View;

namespace UXK.SaveSystem
{
    public class Player : MonoBehaviour
    {
        // Inspector
        [SerializeField] private BagConfigScriptable _bagConfig;
        
        public IBag Bag { get; set; }

        private void Awake()
        {
            Bag = new Bag(_bagConfig);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                UiManager.UiManager.Switch<InventoryViewController, IBag>(Bag);
            }
        }
    }
}