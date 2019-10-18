namespace BehavioursBasedCamera
{
    using System;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [Serializable]
    public abstract class AbstractSerializedIsEnabledBehavior : AbstractBehaviour
    {
        [SerializeField, OnValueChanged("CheckEnable")]
        private bool _isEnabled;

        public override bool isEnabled {
            get => _isEnabled;
            protected set => _isEnabled = value;
        }

        public void CheckEnable()
        {
            if (!Application.isPlaying) return;

            if (_isEnabled) {
                Enable();
            }
            else
                Disable();
        }
    }
}