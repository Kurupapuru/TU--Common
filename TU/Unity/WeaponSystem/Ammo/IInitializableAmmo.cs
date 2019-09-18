using UnityEngine;

namespace WeaponSystem.Ammo
{
    public interface IInitializableAmmo
    {
        void Initialize(Transform target, IAmmoSettings ammoSettings, bool createMuzzle = true);
    }
}