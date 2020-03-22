using System;
using KurupapuruLab.KRobots;
using Lean.Touch;
using NaughtyAttributes;
using TU.Unity.CameraRelated;
using UniRx;
using UnityEngine;

namespace Shared.Code.CharacterControl
{
    public class LocomotionCharacterController : MonoBehaviour
    {
        [Header("References")] public Animator animator;

        [Header("Settings")]
        [OnValueChanged("UpdateMovementSpeed")]
        public float movementSpeedMultiplier = 1;
        public float rotationLerpSpeed = 1;
        public LayerMask invisiblePlaneMask;

        [Header("FirstPersonMouseCamera")]
        public Camera topDownCamera;
        public Camera firstPersonCamera;
        private Transform firstPersonCameraT;
        [Space]
        public float minVerticalRotation = 10;
        public float maxVerticalRotation = 80;
        public float verticalRotationSpeed   = 1;
        public float horizontalRotationSpeed = 1;
        
        private CameraLerper _cameraLerper;
        private IDisposable behaviourTask;


        private void Start()
        {
            firstPersonCameraT = firstPersonCamera.transform;
            
            MainCamera.camera.Subscribe(x =>
            {
                if (x != null)
                    _cameraLerper = x.GetComponent<CameraLerper>();
            }).AddTo(this);

            UpdateMovementSpeed();

            EnableTopDownWASDMovement();
            //EnableFirstPersonWASD();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F3))
                EnableTopDownWASDMovement();
            if (Input.GetKeyDown(KeyCode.F1))
                EnableFirstPersonWASD();
        }

        public void UpdateMovementSpeed()
        {
            animator.SetFloat("Movement Speed", movementSpeedMultiplier);
        }


        #region TopDown WASD

        public void EnableTopDownWASDMovement()
        {
            _cameraLerper.LerpTo(topDownCamera, 1, true);
            
            behaviourTask?.Dispose();
            behaviourTask = Observable.EveryUpdate().Subscribe(_ =>
            {
                MovementUpdate(
                    Input.GetAxis("Vertical"),
                    Input.GetAxis("Horizontal"),
                    true);
                RotateToScreenPosUpdate(Input.mousePosition);
            });
        }

        private void MovementUpdate(float verticalMovement, float horizontalMovement, bool rotateInput = false)
        {
            var input3D = new Vector3(horizontalMovement, 0, verticalMovement);

            if (rotateInput)
                input3D =
                    Quaternion.AngleAxis(_cameraLerper.transform.eulerAngles.y - transform.eulerAngles.y, Vector3.up) *
                    input3D;

            animator.SetFloat(-1348911080, input3D.z); // Movement Vertical
            animator.SetFloat(760785499, input3D.x); // Movement Horizontal
        }

        private void RotateToScreenPosUpdate(Vector2 screenPos)
        {
            //
            var lookTo = WorldScanner.GetPointFromScreenRay(MainCamera.camera.Value, screenPos,
                invisiblePlaneMask);
            if (lookTo == null) return;

            var lookDirection = lookTo.Value - transform.position;
            if (lookDirection == Vector3.zero) return;

            var rotationEuler = transform.eulerAngles;
            rotationEuler.y = Quaternion.LookRotation(lookTo.Value - transform.position).eulerAngles.y;
            transform.eulerAngles =
                MathGod.LerpEulerAngle(transform.eulerAngles, rotationEuler, rotationLerpSpeed * Time.deltaTime);
        }

        #endregion

        
        #region First Person WASD

        public void EnableFirstPersonWASD()
        {
            _cameraLerper.LerpTo(firstPersonCamera, 1, true);

            behaviourTask?.Dispose();
            behaviourTask = Observable.EveryUpdate().Subscribe(_ =>
            {
                MovementUpdate(
                    Input.GetAxis("Vertical"),
                    Input.GetAxis("Horizontal"),
                    false);

                FirstPersonMouseCameraUpdate();
            });
        }

        private void FirstPersonMouseCameraUpdate()
        {
            var fingers = LeanTouch.Fingers;
            if (fingers.Count == 0) return;
            var firstFinger = fingers[0];
            
            transform.Rotate(0, firstFinger.ScaledDelta.x * horizontalRotationSpeed, 0);

            var newCameraRot = firstPersonCameraT.eulerAngles;
            if (newCameraRot.x > 180) newCameraRot.x -= 360;
            newCameraRot.x += firstFinger.ScaledDelta.y * verticalRotationSpeed;
            newCameraRot.x = Mathf.Clamp(newCameraRot.x, minVerticalRotation, maxVerticalRotation);
            firstPersonCameraT.eulerAngles = newCameraRot;
        }

        #endregion
    }
}