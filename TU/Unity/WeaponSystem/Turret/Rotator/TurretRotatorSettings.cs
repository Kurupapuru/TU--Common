using System;
using UnityEngine;

namespace WeaponSystem.Turret
{
    [Serializable]
    public class TurretRotatorSettings : ITurretRotatorSettings
    {
        public float _verticalRotateSpeed;
        public float _horizontalRotateSpeed;
        public float _isRotatedAngle;
        public float _maxRotateAngle;
        
        // ITurretRotatorSettings
        public float verticalRotateSpeed => _verticalRotateSpeed;
        public float horizontalRotateSpeed => _horizontalRotateSpeed;
        public float isRotatedAngle => _isRotatedAngle;
        public float maxRotateAngle => _maxRotateAngle;
    }
}