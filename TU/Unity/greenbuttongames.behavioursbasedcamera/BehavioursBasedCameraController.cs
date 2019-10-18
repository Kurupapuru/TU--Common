namespace BehavioursBasedCamera
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Sirenix.OdinInspector;
    using Sirenix.Serialization;
    using UniRx;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class BehavioursBasedCameraController : SerializedMonoBehaviour, IBehavioursBasedCameraController
    {
        #region IBehavioursBasedCameraController

        [SerializeField] private Camera                        _camera;
        [SerializeField] private Camera                        _defaultCameraSettings;
        [SerializeField] private Transform                     _cameraParent;
        [SerializeField] private Transform                     _cameraCenter;
        [SerializeField] private BehavioursBasedCameraSettings _settings;

        public new Camera    camera                => _camera;
        public     Camera    defaultCameraSettings => _defaultCameraSettings;
        public     Transform cameraParent          => _cameraParent;
        public     Transform cameraCenter          => _cameraCenter;

        public CenterPositionLerpingBehavior CenterPositionLerpingBehavior { get; protected set; }
        public CenterRotationLerpingBehavior CenterRotationLerpingBehavior { get; protected set; }

        public BehavioursBasedCameraSettings Settings {
            get => _settings;
            set => _settings = value;
        }

        #endregion

        public static BehavioursBasedCameraController instance;
        
        public List<AbstractBehaviour> switchableBehaviours = new List<AbstractBehaviour>();
        public List<AbstractBehaviour> optionalBehaviours   = new List<AbstractBehaviour>();

        [NonSerialized] public Transform CameraT;
        public                 float     startFov { get; protected set; }
        public                 float     hardSettedFov { get; set; }

        public BehavioursBasedCameraController()
        {
            CenterPositionLerpingBehavior = new CenterPositionLerpingBehavior();
            CenterRotationLerpingBehavior = new CenterRotationLerpingBehavior();
        }

        private void Awake()
        {
            instance = this;
            
            CameraT  = camera.transform;
            startFov = camera.fieldOfView;
            hardSettedFov = startFov;

            CenterPositionLerpingBehavior.lerpTo = cameraCenter.position;
            CenterRotationLerpingBehavior.lerpTo = cameraCenter.eulerAngles;

            CenterPositionLerpingBehavior.Initialize(this);
            CenterRotationLerpingBehavior.Initialize(this);

            foreach (var b in switchableBehaviours) {
                b.Initialize(this);
                if (b.isEnabled) b.Enable();
            }

            foreach (var b in optionalBehaviours) {
                b.Initialize(this);
                if (b.isEnabled) b.Enable();
            }
        }

        [Button]
        public void SwitchBehavior()
        {
            for (int i = 0; i < switchableBehaviours.Count; i++) {
                if (switchableBehaviours[i].isEnabled) {
                    switchableBehaviours[i].Disable();
                    var nextBehavior                                             = i + 1;
                    if (nextBehavior >= switchableBehaviours.Count) nextBehavior = 0;
                    switchableBehaviours[nextBehavior].Enable();
                    break;
                }
            }
        }

        /// <returns>-1 if not found, else id</returns>
        public int SwitchToBehaviour(Func<AbstractBehaviour, bool> predicate)
        {
            int foundedBehaviour = -1;
            for (int i = 0; i < switchableBehaviours.Count; i++) {
                var b = switchableBehaviours[i];
                if (foundedBehaviour == -1 && predicate.Invoke(b)) {
                    if (!b.isEnabled) b.Enable();
                    foundedBehaviour = i;
                }
                else {
                    if (b.isEnabled) b.Disable();
                }
            }

            return foundedBehaviour;
        }

        public void SwitchToBehaviour(int id)
        {
            for (int i = 0; i < switchableBehaviours.Count; i++) {
                var b = switchableBehaviours[i];
                if (i == id) {
                    if (!b.isEnabled) b.Enable();
                }
                else {
                    if (b.isEnabled) b.Disable();
                }
            }
        }
    }
}