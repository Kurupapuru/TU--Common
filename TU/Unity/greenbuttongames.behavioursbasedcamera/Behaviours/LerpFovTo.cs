namespace BehavioursBasedCamera
{
    using UnityEngine;

    public class LerpFovTo : AbstractSerializedIsEnabledBehavior
    {
        public float targetFov = 75;
        
        private Camera camera => BehavioursBasedCameraController.camera;
        private float fovLerpSpeed => BehavioursBasedCameraController.Settings.fovLerpSpeed;

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