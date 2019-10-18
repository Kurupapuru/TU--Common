namespace BehavioursBasedCamera
{
    using System;
    using AnimationTools.AnimationTasks;
    using UnityEngine;

    [Serializable]
    public class LerpCameraToCamera : LerpCameraToTransform, IHasCameraTargetForCamera
    {
        private Camera camera;
        private float  lerpFromFOV;

        public Camera cameraTargetForCamera {
            get => lerpToCamera;
            set => lerpToCamera = value;
        }

        [SerializeField] private Camera lerpToCamera;

        public override AbstractBehaviour Initialize(IBehavioursBasedCameraController behavioursBasedCameraController)
        {
            base.Initialize(behavioursBasedCameraController);
            camera           = behavioursBasedCameraController.camera;
            lerpToTransform = lerpToCamera.transform;
            return this;
        }

        public override void Enable()
        {
            lerpFromFOV      = camera.fieldOfView;
            base.Enable();
        }

        protected override void UpdateCameraPos(IAnimationTask animationTask)
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, lerpToCamera.fieldOfView, animationTask.progress);
            base.UpdateCameraPos(animationTask);
        }

        public override AbstractBehaviour Copy()
        {
            return new LerpCameraToCamera() {
                BehavioursBasedCameraController = this.BehavioursBasedCameraController,
                CameraTransform       = this.CameraTransform,
                parentAfterLerp       = this.parentAfterLerp,
                lerpLength            = this.lerpLength,
                lerpCurve             = this.lerpCurve,
                lerpToCamera          = this.lerpToCamera
            };
        }
    }
}