using System;
using TU.Unity.World;
using UnityEngine;

namespace WeaponSystem.Ammo
{
    public interface IAmmoSettings
    {
        float damage { get; }
        float speedPerSec { get; }
        float fixedFlyTime { get; }
        float disapearAfter { get; }
        LayerMask attackMask { get; }
        GameObject muzzleEffectPrefab { get; }
        LayerPrefab[] hitEffects { get; }
        bool explosion { get; }
        ExplosionSettings explosionSettings { get; }

        bool calculateSpeedByPerSecondValue { get; }
        bool calculateSpeedByFixedValue { get; }
    }

    [Serializable]
    public struct LayerPrefab
    {
        public string name;
        public LayerMask layerMask;
        public GameObject prefab;
    }
}