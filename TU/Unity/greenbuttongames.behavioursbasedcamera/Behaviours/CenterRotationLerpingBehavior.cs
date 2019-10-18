
namespace BehavioursBasedCamera
{
    using KurupapuruLab.KRobots;
    using System;
    using UnityEngine;

    [Serializable]
    public class CenterRotationLerpingBehavior : AbstractBehaviour
    {
        private float lerpSpeed => BehavioursBasedCameraController.Settings.centerRotationLerpSpeed;
        
        public  Vector3                   lerpTo;

        public override bool isEnabled { get; protected set; }

        public override AbstractBehaviour Initialize(IBehavioursBasedCameraController behavioursBasedCameraController)
        {
            base.Initialize(behavioursBasedCameraController);
            return this;
        }

        protected override void Update()
        {
            var rotation = BehavioursBasedCameraController.cameraCenter.eulerAngles;
            rotation.x = Mathf.Lerp(
                rotation.x,
                lerpTo.x,
                lerpSpeed);
            rotation.y = Mathf.LerpAngle(
                rotation.y,
                lerpTo.y,
                lerpSpeed);

            BehavioursBasedCameraController.cameraCenter.eulerAngles = rotation;

            if (MathGod.AreVectorsEqual(
                BehavioursBasedCameraController.cameraCenter.eulerAngles,
                lerpTo,
                .01f))
                Disable();
        }

        public override AbstractBehaviour Copy()
        {
            return new CenterRotationLerpingBehavior() {
                BehavioursBasedCameraController = this.BehavioursBasedCameraController,
                lerpTo                = this.lerpTo
            };
        }
    }
}