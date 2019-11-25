using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using AnimationTools.AnimationTasks;
using UnityEngine;

public class CameraLerper : MonoBehaviour
{
    public Camera camera;
    private Transform cameraT;

    private void Awake()
    {
        cameraT = camera.transform;
    }

    public void LerpTo(Camera lerpTo, float timeLength, bool parentAfterLerp)
    {
        var aTask = new AnimationTask(timeLength); 
        var lerpToT = lerpTo.transform;
        var startPos = cameraT.position;
        var startRotation = cameraT.rotation;
        var startFOV = camera.fieldOfView;
        aTask.onUpdate.Subscribe(a =>
        {
            cameraT.position = Vector3.Lerp   (startPos,      lerpToT.position, a.progress);
            cameraT.rotation = Quaternion.Lerp(startRotation, lerpToT.rotation, a.progress);
            camera.fieldOfView = Mathf.Lerp(startFOV, lerpTo.fieldOfView, a.progress);
        });
        
        if (parentAfterLerp)
            aTask.onFinish.Subscribe(a => cameraT.parent = lerpToT);
        
        aTask.Play();
    }
}
