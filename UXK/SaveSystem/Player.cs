using UnityEngine;
using UXK.Inventory;

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
    }
}