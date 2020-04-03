using System;
using Lean.Touch;
using UnicornLib.Input;
using UnityEngine;

// ReSharper disable DelegateSubtraction

public class RayEngine : MonoBehaviour
{
    private const int MaxDistance = 30;


    public static RayEngine instance;

    private void Awake()
    {
        instance = this;
    }


    #region RayHandle

    public void OnEnable()
    {
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerUp   += OnFingerUp;
        LeanTouch.OnFingerTap  += OnFingerTap;
    }

    public void OnDisable()
    {
        LeanTouch.OnFingerDown -= OnFingerDown;
        LeanTouch.OnFingerUp   -= OnFingerUp;
        LeanTouch.OnFingerTap  -= OnFingerTap;
    }

    private void OnFingerDown(LeanFinger finger) =>
        FingerActionHandle<ITouchDownObserver>(finger, (observer, touch) => observer.OnTouchDown(touch));

    private void OnFingerUp(LeanFinger finger) =>
        FingerActionHandle<ITouchUpObserver>(finger, (observer, touch) => observer.OnTouchUp(touch));

    private void OnFingerTap(LeanFinger finger) =>
        FingerActionHandle<ITapObserver>(finger, (observer, touch) => observer.OnTap(touch));

    private void FingerActionHandle<T>(LeanFinger touch, Action<T, LeanFinger> doForEachObserver)
    {
        RaycastHit[] hits = RaycastHelper.GetRayHitAll(touch.ScreenPosition, StaticValues.Interactable.layerMask);

        foreach (RaycastHit hit in hits)
        {
            T[] observers;
            if (hit.rigidbody != null)
                observers = hit.rigidbody.GetComponents<T>();
            else
                observers = hit.collider.GetComponents<T>();

            foreach (var observer in observers)
                doForEachObserver.Invoke(observer, touch);
        }
    }

    #endregion RayHandle

    #region Static / GetRay

    

    #endregion Static / GetRay
}

//Ray Interfaces
public interface ITouchDownObserver
{
    void OnTouchDown(LeanFinger touch);
}

public interface ITouchUpObserver
{
    void OnTouchUp(LeanFinger touch);
}

public interface ITapObserver
{
    void OnTap(LeanFinger touch);
}