using System;
using KurupapuruLab.KRobots;
using Lean.Touch;
using NaughtyAttributes;
using TU.Unity.CameraRelated;
using UnicornLib.Input;
using UniRx;
using UnityEngine;

namespace Shared.Code.CharacterControl
{
    public class LocomotionCharacterController : MonoBehaviour
    {
        [Header("References")] 
        public Rigidbody rb;

        [Header("Settings")]
        [OnValueChanged("UpdateMovementSpeed")]
        public float movementSpeedMultiplier = 1;
        public float rotationLerpSpeed = 1;
        public LayerMask lookAtMask;
        [Space]
        public Camera topDownCamera;
        public Camera firstPersonCamera;
        private Transform firstPersonCameraT;
        [Space]
        public float minVerticalRotation = 10;
        public float maxVerticalRotation = 80;
        public float verticalRotationSpeed   = 1;
        public float horizontalRotationSpeed = 1;

        private Vector3 movementVector;
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

        private void FixedUpdate()
        {
            rb.MovePosition(rb.position + movementVector);
        }
        


        #region TopDown WASD

        public void EnableTopDownWASDMovement()
        {
            _cameraLerper?.LerpTo(topDownCamera, 1, true);
            
            behaviourTask?.Dispose();
            behaviourTask = Observable.EveryUpdate().Subscribe(_ =>
            {
                MovementUpdate(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
                RotateToScreenPosUpdate(Input.mousePosition);
            });
        }

        private void MovementUpdate(float verticalMovement, float horizontalMovement)
        {
            movementVector = new Vector3(horizontalMovement, 0, verticalMovement) * movementSpeedMultiplier;
        }

        private void RotateToScreenPosUpdate(Vector2 screenPos)
        {
            Vector3 lookAt;
            var screenPosRay = RaycastHelper.GetRay(screenPos);
            Physics.Raycast(screenPosRay, out var hit, 100, lookAtMask);
            if (hit.collider != null)
                lookAt = hit.point;
            else
            {
                var playerPlane = new Plane(Vector3.up, transform.position);
                playerPlane.Raycast(screenPosRay, out var enterDistance);
                lookAt = screenPosRay.GetPoint(enterDistance);
            }
                
            var lookDirection = lookAt - transform.position;
            if (lookDirection == Vector3.zero) return;

            var rotationEuler = transform.eulerAngles;
            rotationEuler.y = Quaternion.LookRotation(lookDirection).eulerAngles.y;
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
                    Input.GetAxis("Horizontal"));

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