using System;
using UnityEngine;

namespace WeaponSystem.Turret
{
    [Serializable]
    public class TurretTargerFinderSettings : ITurretTargetFinderSettings
    {
        [SerializeField] private float _maxRadius = 10;
        [SerializeField] private float _minRadius = 5;
        [SerializeField] private float _targetScanDelay = 0.02f;
        [SerializeField] private LayerMask[] _targetMasks = {new LayerMask()};
        [SerializeField] private float _targetAllowedAngle;
        [SerializeField] private bool _closestTarget = true;
        [SerializeField] private bool _targetLock;

        // ITurretTargetFinderSettings
        public LayerMask[] targetMasks => _targetMasks;
        public float maxRadius => _maxRadius;
        public float targetScanDelay => _targetScanDelay;
        public bool  closestTarget => _closestTarget;
        public bool  targetLock => _targetLock;
        public float minRadius => _minRadius;
        public float targetAllowedAngle => _targetAllowedAngle;
    }
}