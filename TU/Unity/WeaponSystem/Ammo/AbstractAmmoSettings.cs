namespace WeaponSystem.Ammo
{
    using System;
    using Sirenix.OdinInspector;
    using TU.Unity.World;
    using UnityEngine;
    
    [Serializable]
    public abstract class AbstractAmmoSettings : IAmmoSettings
    {
        [SerializeField] private float _damage;
        [SerializeField] public bool _calculateSpeedByPerSecondValue;
        [ShowIf("calculateSpeedByPerSecondValue", false)]
        [SerializeField] private float _speedPerSec;
        [ShowIf("calculateSpeedByFixedValue",     false)]
        [SerializeField] private float _fixedFlyTime;
        [SerializeField] private float _disapearAfter = 10;
        [SerializeField] private LayerMask _attackMask = new LayerMask();
        [SerializeField] private GameObject _muzzleEffectPrefab;
        [SerializeField] private LayerPrefab[] _hitEffects = new LayerPrefab[1];
        [SerializeField] private ExplosionSettings _explosionSettings;

        public bool calculateSpeedByPerSecondValue => _calculateSpeedByPerSecondValue;
        public bool calculateSpeedByFixedValue => !calculateSpeedByPerSecondValue;
        
        
        // IAmmoSettings
        public float damage => _damage;
        public float speedPerSec => _speedPerSec;
        public float fixedFlyTime => _fixedFlyTime;
        public float disapearAfter => _disapearAfter;
        public LayerMask attackMask => _attackMask;
        public GameObject muzzleEffectPrefab => _muzzleEffectPrefab;
        public LayerPrefab[] hitEffects => _hitEffects;
        public bool explosion => _explosionSettings != null;
        public ExplosionSettings explosionSettings => _explosionSettings;
    }
}