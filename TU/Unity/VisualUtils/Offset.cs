using System;
using UnityEngine;

public class Offset : MonoBehaviour
{
    public Transform sourceTransform;
    public Vector3 simpleOffset;
    public Transform rotateTo;
    public Vector3 rotateOffset;


    [Space]
    public Transform cameraTransform;
    public Vector3 offsetFromCamera;

    [Space]
    public bool useCameraMain;
    public bool rotateToCamera;
    
    
    private bool isRotateToNotNull;
    private bool isCameraTransformNotNull;
    private bool isSourceTransformNotNull;
    private IDisposable cameraUpdateTask;


    private void Start()
    {
        CameraUpdate();
        UpdateBools();
    }

    private void UpdateBools()
    {
        isSourceTransformNotNull = sourceTransform != null;
        isCameraTransformNotNull = cameraTransform != null;
        isRotateToNotNull        = rotateTo != null;
    }

    private void Update()
    {
        if (isSourceTransformNotNull) {
            transform.position = sourceTransform.position + simpleOffset;

            if (!isRotateToNotNull)
                transform.rotation = Quaternion.Euler(sourceTransform.rotation.eulerAngles + rotateOffset);
        }

        if (isRotateToNotNull) {
            transform.LookAt(rotateTo);
            transform.rotation = Quaternion.Euler(transform.eulerAngles + rotateOffset);
        }

        if (isCameraTransformNotNull) {
            transform.position += cameraTransform.rotation * offsetFromCamera;
        }
    }

    private void CameraUpdate()
    {
        if (useCameraMain && cameraTransform == null) {
            var cameraMain = Camera.main;
            if (cameraMain != null) 
                cameraTransform = cameraMain.transform;
        }
        if (rotateToCamera && rotateTo == null)
            rotateTo = cameraTransform;
    }
}