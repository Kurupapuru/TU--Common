namespace BehavioursBasedCamera
{
    using System;
    using AnimationTools.AnimationTasks;
    using Sirenix.OdinInspector;
    using UniRx;
    using UnityEngine;

    [Serializable]
    public class LerpCameraToTransform : AbstractSerializedIsEnabledBehavior, IHasTransformTargetForCamera
    {
        [InfoBox("This behavior works only one time after enable")]
        private IAnimationTask aTask;
        private Vector3 lerpFromPos;
        private Vector3 lerpFromRotEuler;

        protected Transform CameraTransform;

        public Transform transformTargetForCamera {
            get => lerpToTransform;
            set => lerpToTransform = value;
        }

        public Transform      lerpToTransform;
        public bool           parentAfterLerp;
        public float          lerpLength = 1f;
        public AnimationCurve lerpCurve  = AnimationCurve.Linear(0, 0, 1, 1);


        public override AbstractBehaviour Initialize(IBehavioursBasedCameraController behavioursBasedCameraController)
        {
            CameraTransform = behavioursBasedCameraController.camera.transform;
            base.Initialize(behavioursBasedCameraController);
            return this;
        }

        public override void Enable()
        {
            base.Enable();
            lerpFromPos      = CameraTransform.position;
            lerpFromRotEuler = CameraTransform.eulerAngles;

            aTask?.Dispose();
            aTask = new AnimationTask(length: lerpCurve.keys[lerpCurve.keys.Length - 1].time);
            aTask.onUpdate.Subscribe(UpdateCameraPos);
            if (parentAfterLerp)
                aTask.onFinish.Subscribe(_ => CameraTransform.parent = lerpToTransform);
            aTask.Play();
        }

        public override AbstractBehaviour Copy()
        {
            return new LerpCameraToTransform() {
                BehavioursBasedCameraController = this.BehavioursBasedCameraController,
                CameraTransform       = this.CameraTransform,
                parentAfterLerp       = this.parentAfterLerp,
                lerpLength            = this.lerpLength,
                lerpCurve             = this.lerpCurve,
                lerpToTransform      = this.lerpToTransform
            };
        }

        protected virtual void UpdateCameraPos(IAnimationTask animationTask)
        {
            var lerpValue = lerpCurve.Evaluate(aTask.progressInSeconds);
            CameraTransform.position = Vector3.Lerp(lerpFromPos, lerpToTransform.position, lerpValue);
            CameraTransform.eulerAngles = new Vector3(
                Mathf.LerpAngle(lerpFromRotEuler.x, lerpToTransform.eulerAngles.x, lerpValue),
                Mathf.LerpAngle(lerpFromRotEuler.y, lerpToTransform.eulerAngles.y, lerpValue),
                0);
        }
    }
}