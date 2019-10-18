namespace BehavioursBasedCamera
{
    using UnityEngine;

    public class LerpFovToAdditional : AbstractSerializedIsEnabledBehavior
    {
        public float additionalFov = 0;

        private float targetFov;
        
        private Camera camera       => BehavioursBasedCameraController.camera;
        private float startFov => BehavioursBasedCameraController.startFov;
        private float  fovLerpSpeed => BehavioursBasedCameraController.Settings.fovLerpSpeed;

        public override AbstractBehaviour Initialize(IBehavioursBasedCameraController behavioursBasedCameraController)
        {
            base.Initialize(behavioursBasedCameraController);
            targetFov = startFov + additionalFov;
            return this;
        }

        protected override void Update()
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, targetFov, fovLerpSpeed * Time.deltaTime);
            if (camera.fieldOfView == targetFov) updateTask.Dispose();
        }

        public override AbstractBehaviour Copy()
        {
            return new LerpFovTo(){BehavioursBasedCameraController = this.BehavioursBasedCameraController};
        }
    }
}