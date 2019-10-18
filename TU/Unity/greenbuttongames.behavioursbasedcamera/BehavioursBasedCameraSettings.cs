namespace BehavioursBasedCamera
{
    using System;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Serialization;

    [CreateAssetMenu(menuName = "Settings/Behaviours Based Camera")]
    public class BehavioursBasedCameraSettings : SerializedScriptableObject
    {
        [Header("Settings")] public float                    multipleTargetsZoomModifier = 1;
        public                      ValueTuple<float, float> maxFovOffset                = (20, 20);
        public                      float                    fovChangeSpeed              = 1;
        public                      float                    fovLerpSpeed                = .1f;
        public                      float                    centerPositionLerpSpeed     = .5f;
        public                      float                    centerRotationLerpSpeed     = .5f;

        public Vector2                  pointerRotationSpeed         = Vector2.one;
        public ValueTuple<float, float> pointerRotationVerticalClamp = (10, 80);

        public Vector3 defaultHardSnapOffset = Vector3.zero;

        [Header("Presets")] public AbstractBehaviour PresetSnapCameraCenterTo;
        public                     AbstractBehaviour PresetLerpCameraToTransform;
        public                     AbstractBehaviour PresetLerpCameraToCamera;
    }
}