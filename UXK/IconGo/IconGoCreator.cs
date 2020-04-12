using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXK.IconGoCreator;

public static class IconGoCreator
{
    /// <summary>
    /// Возвращает копию объекта с иконкой постоянно смотрящей в камеру 
    /// </summary>
    /// <param name="sprite">Иконка</param>
    /// <param name="scale">Длина самой длинной стороны спрайта в игровом пространстве</param>
    /// <returns></returns>
    public static IconGo Create(Sprite sprite, float scale) =>
        GetEmptyIconGo().Setup(sprite, scale);

    
    private static GameObject Prefab
    {
        get
        {
            if (bf_prefab == null)
                bf_prefab = Resources.Load<GameObject>("IconGoPrefab");
            return bf_prefab;
        }
    }
    private static GameObject bf_prefab;

    private static IconGo GetEmptyIconGo() => 
        GameObject.Instantiate(Prefab).GetComponent<IconGo>();
        
}
