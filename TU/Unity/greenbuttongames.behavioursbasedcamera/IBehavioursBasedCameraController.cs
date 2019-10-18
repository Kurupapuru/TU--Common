namespace BehavioursBasedCamera
{
    using System;
    using UnityEngine;

    public interface IBehavioursBasedCameraController
    {
        Camera    camera                { get; }
        Camera    defaultCameraSettings { get; }
        Transform cameraParent          { get; }
        Transform cameraCenter          { get; }
        float     startFov              { get; }
        float     hardSettedFov         { get; set; }


        CenterPositionLerpingBehavior CenterPositionLerpingBehavior { get; }
        CenterRotationLerpingBehavior CenterRotationLerpingBehavior { get; }

        BehavioursBasedCameraSettings Settings { get; set; }
    }
}