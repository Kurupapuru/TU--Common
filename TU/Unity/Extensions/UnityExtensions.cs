using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public static class UnityExtensions
{
    /// <summary>
    /// Component of type T from Components IEnumerable (array/list/etc.)
    /// </summary>
    public static T GetComponent<T>(this IList<Component> componentsList) where T : class
    {
        for (var i = 0; i < componentsList.Count; i++) {
            var component = componentsList[i];
            if (component is T TComponent) return TComponent;
        }

        return null;
    }

    public static bool LayerCheck(this LayerMask layerMask, int layer) => layerMask == (layerMask | (1 << layer));

    public static void DoForAllChilds(this Transform transform, Action<Transform> action, bool includeThis = true)
    {
        if (includeThis) action.Invoke(transform);
        foreach (var child in transform)
            DoForAllChilds((Transform) child, action, true);
    }

    public static void ForceDo<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key, Func<TValue, TValue> doFunc)
    {
        source.TryGetValue(key, out var currentValue);
        source[key] = doFunc.Invoke(currentValue);
    }

    public static void DestroySmart(this Object obj)
    {
#if UNITY_EDITOR
        if (Application.isPlaying)    
            Object.Destroy(obj);
        else
            Object.DestroyImmediate(obj);
#else
        Object.Destroy(obj);
#endif
    }
}