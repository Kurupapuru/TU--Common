namespace BehavioursBasedCamera
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class BehavioursBasedCameraManager
    {
        [SerializeField] private BehavioursBasedCameraController controller;

        private bool isInitialized;

        private List<AbstractBehaviour> switchableBehaviours;
        private List<AbstractBehaviour> optionalBehaviours;

        public BehavioursBasedCameraManager(BehavioursBasedCameraController controller)
        {
            this.controller = controller;
        }

        private void Initialize()
        {
            this.switchableBehaviours = controller.switchableBehaviours;
            this.optionalBehaviours   = controller.optionalBehaviours;
            isInitialized             = true;
        }


        public AbstractBehaviour SnapCameraCenterTo(Transform transformTargetForCenter)
        {
            if (!isInitialized) Initialize();

            AbstractBehaviour result = null;
            int switchTo = -1;

            for (int i = 0; i < switchableBehaviours.Count; i++) {
                result = IsContainOrEqualBehaviour<HardSnapBehaviour>(
                    switchableBehaviours[i], behaviour => { return ((HardSnapBehaviour) behaviour).transformTargetForCenter == transformTargetForCenter; });
                if (result != null) {
                    switchTo = i;
                    break;
                }
            }
            foreach (var switchableBehaviour in switchableBehaviours) {
                result = IsContainOrEqualBehaviour<HardSnapBehaviour>(
                    switchableBehaviour, behaviour => { return ((HardSnapBehaviour) behaviour).transformTargetForCenter == transformTargetForCenter; });
                if (result != null) break;
            }

            if (result == null) {
                result = controller.Settings.PresetSnapCameraCenterTo.Copy();

                SetTargets(result,
                    transformTargetForCenter: transformTargetForCenter,
                    transformTargetForCamera: null,
                    cameraTargetForCamera: controller.defaultCameraSettings);

                switchableBehaviours.Add(result);
                switchTo = switchableBehaviours.Count - 1;
                result.Initialize(controller);
            }

            if (!result.isEnabled) result.Enable();

            controller.SwitchToBehaviour(switchTo);
            return result;
        }

        public AbstractBehaviour LerpCameraTo(Transform transformTargetForCamera)
        {
            if (!isInitialized) Initialize();

            AbstractBehaviour result = null;
            int switchTo = -1;

            for (int i = 0; i < switchableBehaviours.Count; i++) {
                result = IsContainOrEqualBehaviour<LerpCameraToTransform>(
                    switchableBehaviours[i], behaviour => { return ((LerpCameraToTransform) behaviour).transformTargetForCamera == transformTargetForCamera; });
                if (result != null) {
                    switchTo = i;
                    break;
                }
            }

            if (result == null) {
                result = controller.Settings.PresetLerpCameraToTransform.Copy();

                SetTargets(result,
                    transformTargetForCenter: null,
                    transformTargetForCamera: transformTargetForCamera,
                    cameraTargetForCamera: null);

                switchableBehaviours.Add(result);
                switchTo = switchableBehaviours.Count - 1;
                result.Initialize(controller);
            }

            if (!result.isEnabled) result.Enable();

            controller.SwitchToBehaviour(switchTo);
            return result;
        }

        public AbstractBehaviour LerpCameraTo(Camera cameraTargetForCamera)
        {
            if (!isInitialized) Initialize();

            AbstractBehaviour result   = null;
            int               switchTo = -1;

            for (int i = 0; i < switchableBehaviours.Count; i++) {
                result = IsContainOrEqualBehaviour<LerpCameraToCamera>(
                    switchableBehaviours[i], behaviour => { return ((LerpCameraToCamera) behaviour).cameraTargetForCamera == cameraTargetForCamera; });
                if (result != null) {
                    switchTo = i;
                    break;
                }
            }

            if (result == null) {
                result = controller.Settings.PresetLerpCameraToCamera.Copy();

                SetTargets(result,
                    transformTargetForCenter: null,
                    transformTargetForCamera: null,
                    cameraTargetForCamera: cameraTargetForCamera);

                switchableBehaviours.Add(result);
                switchTo = switchableBehaviours.Count - 1;
                result.Initialize(controller);
            }

            controller.SwitchToBehaviour(switchTo);
            return result;
        }

        /// <param name="behaviour">Behaviour to check</param>
        /// <typeparam name="T">Trying to find behaviour which type is equal to this, or contains behaviour with this type</typeparam>
        private static AbstractBehaviour IsContainOrEqualBehaviour<T>(AbstractBehaviour behaviour, Func<AbstractBehaviour, bool> additionalChecking)
        {
            if (behaviour is T && additionalChecking.Invoke(behaviour)) return behaviour;
            if (behaviour is BehavioursContainer container) {
                foreach (var b in container.behaviours) {
                    var isB = IsContainOrEqualBehaviour<T>(b, additionalChecking);
                    if (isB != null) return container;
                }
            }

            return null;
        }

        private static void SetTargets(AbstractBehaviour b, Transform transformTargetForCenter, Transform transformTargetForCamera, Camera cameraTargetForCamera)
        {
            if (transformTargetForCenter != null) {
                if (b is IHasTransformTargetForCenter hasTarget)
                    hasTarget.transformTargetForCenter = transformTargetForCenter;
            }

            if (transformTargetForCamera != null) {
                if (b is IHasTransformTargetForCamera hasTarget)
                    hasTarget.transformTargetForCamera = transformTargetForCamera;
            }

            if (cameraTargetForCamera != null) {
                if (b is IHasCameraTargetForCamera hasTarget)
                    hasTarget.cameraTargetForCamera = cameraTargetForCamera;
            }

            if (b is BehavioursContainer container) {
                foreach (var subB in container.behaviours) {
                    SetTargets(subB, transformTargetForCenter, transformTargetForCamera, cameraTargetForCamera);
                }
            }
        }
    }
}