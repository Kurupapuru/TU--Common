namespace BehavioursBasedCamera
{
    using System;
    using UnityEngine;

    [Serializable]
    public class RotateWithPointerBehaviour : AbstractSerializedIsEnabledBehavior
    {
        private Vector2 pointerPos;
        private Vector2 lastPointerPos;

        private CenterRotationLerpingBehavior lerping       => BehavioursBasedCameraController.CenterRotationLerpingBehavior;
        private Vector2                       rotationSpeed => BehavioursBasedCameraController.Settings.pointerRotationSpeed;
        private ValueTuple<float, float>      verticalClamp => BehavioursBasedCameraController.Settings.pointerRotationVerticalClamp;

        protected override void Update()
        {
            InputUpdate();
        }

        public override AbstractBehaviour Copy()
        {
            return new RotateWithPointerBehaviour() {
                BehavioursBasedCameraController = this.BehavioursBasedCameraController
            };
        }

        private void InputUpdate()
        {
            var pointerDown = false;
            var pointerHold = false;

#if UNITY_EDITOR || UNITY_STANDALONE
            // Standalone
            pointerDown = Input.GetMouseButtonDown(0);
            pointerHold = Input.GetMouseButton(0);
            pointerPos  = Input.mousePosition;
#elif UNITY_ANDROID || UNITY_IOS
            // Touch
            if (Input.touchCount > 0) {
                var firstTouch = Input.GetTouch(0);
                pointerDown = firstTouch.phase == TouchPhase.Began;
                pointerHold = true;
                pointerPos = firstTouch.position;
            }
#endif

            if (pointerDown) {
                lastPointerPos = pointerPos;
            }

            if (pointerHold) {
                Vector2 delta = pointerPos - lastPointerPos;

                if (delta.magnitude != 0) {
                    var lerpTo = BehavioursBasedCameraController.CenterRotationLerpingBehavior.lerpTo;

                    lerpTo.y += delta.x * rotationSpeed.x;
                    lerpTo.x += -delta.y * rotationSpeed.y;

                    if (lerpTo.x < verticalClamp.Item1) lerpTo.x = verticalClamp.Item1;
                    if (lerpTo.x > verticalClamp.Item2) lerpTo.x = verticalClamp.Item2;

                    lerping.lerpTo = lerpTo;
                    lerping.Enable();

                    lastPointerPos = pointerPos;
                }
            }
        }
    }
}