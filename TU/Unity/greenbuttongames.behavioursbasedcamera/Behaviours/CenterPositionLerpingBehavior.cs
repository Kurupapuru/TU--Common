namespace BehavioursBasedCamera
{
    using System;
    using UnityEngine;
    using KurupapuruLab.KRobots;

    [Serializable]
    public class CenterPositionLerpingBehavior : AbstractBehaviour
    {
        private Vector3 _lerpTo;

        private float lerpSpeed => BehavioursBasedCameraController.Settings.centerPositionLerpSpeed;

        public Vector3 lerpTo {
            get => _lerpTo;
            set {
                _lerpTo = value;
                if (lerpTo.y < 0) _lerpTo.y   += 360;
                if (lerpTo.y > 360) _lerpTo.y -= 360;
            }
        }


        public override bool isEnabled { get; protected set; }

        protected override void Update()
        {
            BehavioursBasedCameraController.cameraCenter.position = Vector3.Lerp(BehavioursBasedCameraController.cameraCenter.position, lerpTo, lerpSpeed * Time.deltaTime);

            if (MathGod.AreVectorsEqual(BehavioursBasedCameraController.cameraCenter.position, lerpTo, .01f))
                Disable();
        }

        public override AbstractBehaviour Copy()
        {
            return new CenterPositionLerpingBehavior() {
                BehavioursBasedCameraController = this.BehavioursBasedCameraController,
                _lerpTo = this._lerpTo
            };
        }
    }
}