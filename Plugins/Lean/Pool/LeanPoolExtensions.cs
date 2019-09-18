
using Lean.Pool;

namespace Plugins.Lean.Pool
{
    using UnityEngine;
    public static class LeanPoolExtensions
    {
        // Spawn via GameObject, and get clone
        /// <summary>
        /// This allows you to spawn a prefab via GameObject.
        /// </summary>
        public static GameObject Spawn(this GameObject prefab)
            => LeanPool.Spawn(prefab);
        /// <summary>
        /// This allows you to spawn a prefab via GameObject.
        /// </summary>
        public static GameObject Spawn(this GameObject prefab, Vector3 position, Quaternion rotation)
            => LeanPool.Spawn(prefab, position, rotation);
        /// <summary>
        /// This allows you to spawn a prefab via GameObject.
        /// </summary>
        public static GameObject Spawn(this GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
            => LeanPool.Spawn(prefab, position, rotation, parent);
        // Spawn via GameObject, and get Component of clone
        /// <summary>
        /// This allows you to spawn a prefab via GameObject.
        /// </summary>
        public static T Spawn<T>(this GameObject prefab)
            => LeanPool.Spawn(prefab).GetComponent<T>();
        /// <summary>
        /// This allows you to spawn a prefab via GameObject.
        /// </summary>
        public static T Spawn<T>(this GameObject prefab, Vector3 position, Quaternion rotation)
            => LeanPool.Spawn(prefab, position, rotation).GetComponent<T>();
        /// <summary>
        /// This allows you to spawn a prefab via GameObject.
        /// </summary>
        public static T Spawn<T>(this GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
            => LeanPool.Spawn(prefab, position, rotation, parent).GetComponent<T>();

        // Spawn via Component, and get Component of clone
        /// <summary>
        /// This allows you to spawn a prefab via Component.
        /// </summary>
        public static T Spawn<T>(this T prefabComponent) where T : Component
            => LeanPool.Spawn(prefabComponent);
        /// <summary>
        /// This allows you to spawn a prefab via Component.
        /// </summary>
        public static T Spawn<T>(this T prefabComponent, Vector3 position, Quaternion rotation) where T : Component
            => LeanPool.Spawn(prefabComponent, position, rotation);
        /// <summary>
        /// This allows you to spawn a prefab via Component.
        /// </summary>
        public static T Spawn<T>(this T prefabComponent, Vector3 position, Quaternion rotation, Transform parent) where T : Component
            => LeanPool.Spawn(prefabComponent, position, rotation, parent);
        
        // Despawn via GameObject
        /// <summary>
        /// This allows you to despawn a clone via GameObject, with optional delay.
        /// </summary>
        public static void Despawn(this GameObject gameObject, float delay = 0f)
            => LeanPool.Despawn(gameObject, delay);
        
        // Despawn via Component
        /// <summary>
        /// This allows tou to despawn a clone via Component, with optional delay.
        /// </summary>
        public static void Despawn(this Component clone, float delay = 0f)
            => LeanPool.Despawn(clone, delay);
    }
}