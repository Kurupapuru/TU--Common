using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchInfo
{
    // Non static
    /// <summary>
    /// Указывает на то сколько времени касание не двигалось с момента нажатия (-1 если касание перемещалось)
    /// </summary>
    // public float stationaryTime;
    public Component currentComponent;


    // Static
    static Dictionary<int, TouchInfo> all = new Dictionary<int, TouchInfo>();

    public static void Add(LeanFinger touch)
    {
        all.Remove(touch.Index);
        all.Add(touch.Index, new TouchInfo());
    }
    public static void Remove(LeanFinger touch)
    {
        all.Remove(touch.Index);
    }
    public static TouchInfo Get(LeanFinger touch)
    {
        if (!all.ContainsKey(touch.Index))
            Add(touch);
        return all[touch.Index];
    }
}
