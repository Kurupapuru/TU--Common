using System;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    
    
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerUp += EndedOrCanceled;
    }

    void OnFingerDown(LeanFinger touch)
    {
        TouchInfo.Add(touch);
    }
    void EndedOrCanceled(LeanFinger touch)
    {
        TouchInfo.Remove(touch);
    }


    //====================================================//
    //=======================EVENTS=======================//
    //====================================================//


}