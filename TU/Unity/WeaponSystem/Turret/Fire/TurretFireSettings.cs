using System;
using UnityEngine;
using WeaponSystem.Ammo;

namespace WeaponSystem.Turret
{
    using UnityEngine.Serialization;

    [Serializable]
    public class TurretFireSettings : ITurretFireSettings
    {
        [SerializeField] private GameObject _ammoPrefab;
        [SerializeField] private int _ammoCount = 10;
        [SerializeField] private float _fireDelay = .5f;
        [SerializeField] private float _reloadTime = 1;
        [SerializeField] private AudioClip _fireSound;
        [SerializeField] private float _fireSoundVolume = 1;
        [SerializeField] private AudioClip _reloadSound;
        [SerializeField] private float _reloadSoundVolume = 1;
        [FormerlySerializedAs("_ammoSettings")]
        [SerializeField] private bool useHomingAmmoSettings = true;
        [SerializeField] private HomingAmmoSettings _homingAmmoSettings = new HomingAmmoSettings();
        [SerializeField] private AnimationTaskBasedAmmoSettings _animationAmmoSettings = new AnimationTaskBasedAmmoSettings();

        // ITurretFireSettings
        public AudioClip fireSound => _fireSound;
        public float fireSoundVolume => _fireSoundVolume;
        public AudioClip reloadSound => _reloadSound;
        public float reloadSoundVolume => _reloadSoundVolume;
        public GameObject ammoPrefab => _ammoPrefab;

        public IAmmoSettings ammoSettings {
            get {
                if (useHomingAmmoSettings) return _homingAmmoSettings;
                else return _animationAmmoSettings;
            }
        }
        public int ammoCount => _ammoCount;
        public float fireDelay => _fireDelay;
        public float reloadTime => _reloadTime;
    }
}