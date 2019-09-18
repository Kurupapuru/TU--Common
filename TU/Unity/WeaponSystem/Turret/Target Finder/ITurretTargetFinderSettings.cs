using UnityEngine;

namespace WeaponSystem.Turret
{
    public interface ITurretTargetFinderSettings
    {
        LayerMask[] targetMasks { get; }
        float maxRadius { get; }
        float minRadius { get; }
        float targetAllowedAngle { get; }
        float targetScanDelay { get; }
        bool closestTarget { get; }
        bool targetLock { get; }
    }
}