using System;
using BehavioursBasedCamera;
using KurupapuruLab.KRobots;
using Sirenix.OdinInspector;
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
        private Camera _camera;
        private IDisposable behaviourTask;


        private void Start()
        {
            _camera = BehavioursBasedCameraController.instance.camera;

            UpdateMovementSpeed();

            EnableWASDMovement();
        }

        public void UpdateMovementSpeed()
        {
            animator.SetFloat("Movement Speed", movementSpeedMultiplier);
        }

        public void EnableWASDMovement()
        {
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
                    Quaternion.AngleAxis(_camera.transform.eulerAngles.y - transform.eulerAngles.y, Vector3.up) *
                    input3D;

            animator.SetFloat(-1348911080, input3D.z); // Movement Vertical
            animator.SetFloat(760785499, input3D.x); // Movement Horizontal
        }

        private void RotateToScreenPosUpdate(Vector2 screenPos)
        {
            var lookTo = WorldScanner.GetPointFromScreenRay(BehavioursBasedCameraController.instance.camera, screenPos,
                invisiblePlaneMask);
            if (lookTo == null) return;

            var lookDirection = lookTo.Value - transform.position;
            if (lookDirection == Vector3.zero) return;

            var rotationEuler = transform.eulerAngles;
            rotationEuler.y = Quaternion.LookRotation(lookTo.Value - transform.position).eulerAngles.y;
            transform.eulerAngles =
                MathGod.LerpEulerAngle(transform.eulerAngles, rotationEuler, rotationLerpSpeed * Time.deltaTime);
        }
    }
}