using System.Collections;
using System.Collections.Generic;
using BehavioursBasedCamera;
using Sirenix.OdinInspector;
using UnityEngine;

public class BBCameraDebug : SerializedMonoBehaviour
{
    [SerializeField] private BehavioursBasedCameraManager manager = new BehavioursBasedCameraManager(null);

    [Button]
    private void SnapCameraCenterTo(Transform transform) => manager.SnapCameraCenterTo(transform);

    [Button]
    private void LerpCameraTo(Transform transform) => manager.LerpCameraTo(transform);

    [Button]
    private void LerpCameraTo(Camera camera) => manager.LerpCameraTo(camera);
}