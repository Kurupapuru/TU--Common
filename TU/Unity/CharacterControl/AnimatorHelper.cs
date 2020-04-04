using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class AnimatorHelper : MonoBehaviour
{
    public Animator animator;
    public bool pullRootMotion   = false;
    public bool motionToAnimator = false;

    public ReactiveCommand<Vector3> rootMotionCommand = new ReactiveCommand<Vector3>();
    [NonSerialized] public Vector3 originalLocalPos;


    private float timeDelta
    {
        get
        {
            switch (animator.updateMode)
            {
                case AnimatorUpdateMode.Normal:
                    return Time.deltaTime;
                case AnimatorUpdateMode.AnimatePhysics:
                    return Time.fixedDeltaTime;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void Start()
    {
        originalLocalPos = transform.localPosition;
        previousPosition = transform.position;
    }

    private void Update()
    {
        if (animator.updateMode!=AnimatorUpdateMode.Normal)
            return;
        
        MotionToAnimator();
        PullRootMotion();
    }

    private void FixedUpdate()
    {
        if (animator.updateMode!=AnimatorUpdateMode.AnimatePhysics)
            return;
        
        MotionToAnimator();
        PullRootMotion();
    }

    private void PullRootMotion()
    {
        if (!pullRootMotion)
            return;

        var rootMotionDelta = transform.localPosition - originalLocalPos;
        transform.localPosition = originalLocalPos;
        
        if (rootMotionDelta == Vector3.zero) return;
        rootMotionDelta = transform.TransformVector(rootMotionDelta);
        if (rootMotionCommand.CanExecute.Value) rootMotionCommand.Execute(rootMotionDelta);
    }


    private Vector3 previousPosition;
    private void MotionToAnimator()
    {
        if (!motionToAnimator)
            return;

        var delta = Quaternion.AngleAxis(-transform.eulerAngles.y, Vector3.up) * (transform.position - previousPosition) / timeDelta;
        animator.SetFloat("Movement Vertical", delta.z);
        animator.SetFloat("Movement Horizontal", delta.x);
        
        previousPosition = transform.position;
    }
}
