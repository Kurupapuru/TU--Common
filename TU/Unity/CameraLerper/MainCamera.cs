using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public new static ReactiveProperty<Camera> camera = new ReactiveProperty<Camera>();

    public Camera mainCamera;
    
    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = GetComponent<Camera>();

        if (mainCamera == null)
            mainCamera = Camera.main;
        
        camera.Value = mainCamera;
    }
}