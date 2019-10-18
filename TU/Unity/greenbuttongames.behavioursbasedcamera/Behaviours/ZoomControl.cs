namespace BehavioursBasedCamera
{
    using System;
    using UniRx;
    using UnityEngine;

    public class ZoomControl : AbstractSerializedIsEnabledBehavior
    {
        private float additionalFov;

        private float  targetFov     => hardSettedFov + additionalFov;
        private Camera camera        => BehavioursBasedCameraController.camera;
        private float  scrollMult    => BehavioursBasedCameraController.Settings.fovChangeSpeed;
        private float  hardSettedFov => BehavioursBasedCameraController.hardSettedFov;
        private float  fovLerpSpeed  => BehavioursBasedCameraController.Settings.fovLerpSpeed;

        private ValueTuple<float, float> additionalFovClamp => BehavioursBasedCameraController.Settings.maxFovOffset;


        protected override void Update()
        {
            //TODO: touch zoom control
            var scroll = Input.mouseScrollDelta.y;
            if (scroll != 0) {
                scroll        *= scrollMult;
                additionalFov -= scroll;

                if (additionalFov < -additionalFovClamp.Item1) additionalFov = -additionalFovClamp.Item1;
                if (additionalFov > additionalFovClamp.Item2) additionalFov = additionalFovClamp.Item2;
            }

            Debug.Log(targetFov);
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, targetFov, fovLerpSpeed * Time.deltaTime);
        }

        public override AbstractBehaviour Copy()
        {
            return new ZoomControl() {BehavioursBasedCameraController = this.BehavioursBasedCameraController};
        }
    }
}