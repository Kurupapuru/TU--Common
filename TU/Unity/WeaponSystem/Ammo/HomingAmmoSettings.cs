using System;
using UnityEngine;

namespace WeaponSystem.Ammo
{
    [Serializable]
    public class HomingAmmoSettings : AbstractAmmoSettings
    {
        [Space] 
        [SerializeField] public float homingAllowedAngle;
        [SerializeField] public float rotateLerpSpeed;
    }
}