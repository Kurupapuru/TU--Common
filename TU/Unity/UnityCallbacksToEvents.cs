using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class UnityCallbacksToEvents : MonoBehaviour
{
    [FoldoutGroup("Awake",     false)] public UnityEvent OnAwakeEvent;
    [FoldoutGroup("Start",     false)] public UnityEvent OnStartEvent;
    [FoldoutGroup("OnEnable",  false)] public UnityEvent OnEnableEvent;
    [FoldoutGroup("OnDisable", false)] public UnityEvent OnDisableEvent;
    [FoldoutGroup("OnDestroy", false)] public UnityEvent OnDestroyEvent;

    private void Awake()     => OnAwakeEvent?.Invoke();
    private void Start()     => OnStartEvent?.Invoke();
    private void OnEnable()  => OnEnableEvent?.Invoke();
    private void OnDisable() => OnDisableEvent?.Invoke();
    private void OnDestroy() => OnDestroyEvent?.Invoke();
}
