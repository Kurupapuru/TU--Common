namespace BehavioursBasedCamera
{
    using System;
    using UnityEngine;

    [Serializable]
    public class BehavioursContainer : AbstractSerializedIsEnabledBehavior
    {
        public  AbstractBehaviour[]    behaviours;
        public override IBehavioursBasedCameraController BehavioursBasedCameraController { get; set; }


        public BehavioursContainer()
        {
        }

        public BehavioursContainer(int behavioursLength) =>
            behaviours = new AbstractBehaviour[behavioursLength];

        public BehavioursContainer(AbstractBehaviour[] behaviours) =>
            this.behaviours = behaviours;

        public override AbstractBehaviour Initialize(IBehavioursBasedCameraController behavioursBasedCameraController)
        {
            foreach (var b in behaviours) b.Initialize(behavioursBasedCameraController);

            base.Initialize(behavioursBasedCameraController);
            return this;
        }

        public override void Enable()
        {
            foreach (var b in behaviours) b.Enable();

            base.Enable();
        }

        protected override void Update()
        {
        }

        public override void Disable()
        {
            foreach (var b in behaviours) b.Disable();

            base.Disable();
        }

        public override AbstractBehaviour Copy()
        {
            var copy = new BehavioursContainer(behaviours.Length) {
                BehavioursBasedCameraController = this.BehavioursBasedCameraController
            };
            for (int i = 0; i < behaviours.Length; i++) {
                copy.behaviours[i] = behaviours[i].Copy();
            }

            return copy;
        }
    }
}