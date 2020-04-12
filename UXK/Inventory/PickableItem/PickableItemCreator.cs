using UnityEngine;

namespace UXK.Inventory
{
    public static class PickableItemCreator
    {
        private static PickableItem GetInstance()
        {
            if (bf_prefab == null)
                bf_prefab = Resources.Load<GameObject>("PickableItemPrefab");
            return GameObject.Instantiate(bf_prefab).GetComponentInChildren<PickableItem>();
        }
        private static GameObject bf_prefab;
    
        public static PickableItem Create(IItemWithAmount item, Vector3 position, float horizontalImpulse, float verticalImpulse)
        {
            var angle   = Random.Range(0f, 360f);
            var impulse = horizontalImpulse * (Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward);
            impulse.y = verticalImpulse;
            return Create(item, position, impulse);
        }
    
        public static PickableItem Create(IItemWithAmount item, Vector3 position, Vector3 impulse = default)
        {
            return GetInstance().Setup(item, position,impulse);
        }
    }
}