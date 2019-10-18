namespace BehavioursBasedCamera
{
    using System;
    using System.Collections.Generic;
    using UniRx;
    using UnityEngine;

    public class MultipleTargetsWatcher : AbstractSerializedIsEnabledBehavior
    {
        public  List<Transform> targets = new List<Transform>();
        private float           zoomModifier => BehavioursBasedCameraController.Settings.multipleTargetsZoomModifier;
        private IDisposable     lerpTask;


        private Camera camera       => BehavioursBasedCameraController.camera;
        private float  scrollMult   => BehavioursBasedCameraController.Settings.fovChangeSpeed;
        private float  startFov     => BehavioursBasedCameraController.startFov;
        private float  fovLerpSpeed => BehavioursBasedCameraController.Settings.fovLerpSpeed;

        protected override void Update()
        {
            var bounds = new Bounds(targets[0].position, Vector3.one);
            for (int i = 1; i < targets.Count; i++) bounds.Encapsulate(targets[i].position);

            BehavioursBasedCameraController.cameraCenter.position = bounds.center;
            
            BehavioursBasedCameraController.hardSettedFov = bounds.size.magnitude * zoomModifier;
        }

        public override AbstractBehaviour Copy() => throw new System.NotImplementedException();
    }
}