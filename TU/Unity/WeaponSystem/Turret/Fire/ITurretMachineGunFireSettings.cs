using UnityEngine;
using WeaponSystem.Ammo;

namespace WeaponSystem.Turret
{
    public interface ITurretFireSettings
    {
        AudioClip fireSound { get; }
        float fireSoundVolume { get; }
        AudioClip reloadSound { get; }
        float reloadSoundVolume { get; }
        GameObject ammoPrefab { get; }
        IAmmoSettings ammoSettings { get; }

        /// <summary>
        /// < 0 == infinite (no reloading)
        /// </summary>
        int ammoCount { get; }
        float fireDelay { get; }
        float reloadTime { get; }
    }
}