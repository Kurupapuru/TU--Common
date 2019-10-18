namespace BehavioursBasedCamera
{
    using System;
    using Sirenix.Serialization;
    using UniRx;
    using UnityEngine;

    [Serializable]
    public class HardSnapBehaviour : AbstractSerializedIsEnabledBehavior, IHasTransformTargetForCenter
    {
        [SerializeField] private Transform _target;

        public Transform transformTargetForCenter {
            get => _target;
            set => _target = value;
        }

        public Vector3 offset;


        public override void Enable()
        {
            if (Application.isPlaying)
                updateTask = Observable.EveryLateUpdate().Subscribe(_ => Update());
            isEnabled = true;
        }

        protected override void Update()
        {
            BehavioursBasedCameraController.cameraCenter.position = transformTargetForCenter.position + offset;
        }

        public override AbstractBehaviour Copy()
        {
            return new HardSnapBehaviour {
                BehavioursBasedCameraController = this.BehavioursBasedCameraController,
                transformTargetForCenter                = this.transformTargetForCenter,
                offset                = this.offset
            };
        }
    }
}