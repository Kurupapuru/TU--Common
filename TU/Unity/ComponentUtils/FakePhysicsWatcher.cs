using System;
using UniRx;
using UnityEngine;

namespace TU.Unity.ComponentUtils
{
    public class FakePhysicsWatcher : MonoBehaviour
    {
        [SerializeField] private UpdateType _updatePhysiscsType = UpdateType.LateUpdate;

        [NonSerialized] public Vector3 velocity;
        [NonSerialized] public Vector3 torque;

        private float timer;

        public UpdateType updatePhysiscsType {
            get => _updatePhysiscsType;
            set {
                _updatePhysiscsType = value;
                StartUpdateFakePhysics();
            }
        }

        private Transform cachedTransform;
        private Vector3 lastPos;
        private Vector3 lastRotation;

        private IDisposable updatePhysicsTask;

        private void Start()
        {
            cachedTransform = transform;
            lastPos = cachedTransform.position;
            lastRotation = cachedTransform.eulerAngles;
        }

        private void OnEnable()
        {
            StartUpdateFakePhysics();
        }

        private void OnDisable()
        {
            updatePhysicsTask?.Dispose();
        }

        private void StartUpdateFakePhysics()
        {
            updatePhysicsTask?.Dispose();

#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif

            switch (_updatePhysiscsType)
            {
                case UpdateType.Update:
                    updatePhysicsTask = Observable.EveryUpdate().Subscribe(_ => UpdateFakePhysics(Time.deltaTime));
                    break;
                case UpdateType.LateUpdate:
                    updatePhysicsTask = Observable.EveryLateUpdate().Subscribe(_ => UpdateFakePhysics(Time.deltaTime));
                    break;
                case UpdateType.FixedUpdate:
                    updatePhysicsTask = Observable.EveryFixedUpdate().Subscribe(_ => UpdateFakePhysics(Time.fixedDeltaTime));
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void UpdateFakePhysics(float deltaTime)
        {
            var currentPosition = cachedTransform.position;

            velocity = (currentPosition - lastPos) / deltaTime;
            var currentRotation = cachedTransform.eulerAngles;
            torque = (currentRotation - lastRotation) / deltaTime;

            lastPos      = currentPosition;
            lastRotation = currentRotation;
        }

        public enum UpdateType
        {
            Update,
            LateUpdate,
            FixedUpdate
        }
    }
}