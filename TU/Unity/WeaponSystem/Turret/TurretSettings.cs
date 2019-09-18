using UnityEngine;
using UnityEngine.Serialization;
using WeaponSystem.Ammo;

namespace WeaponSystem.Turret
{
    [CreateAssetMenu(menuName = "Settings/Turret")]
    public class TurretSettings : ScriptableObject
    {
        [Space, SerializeField] public TurretFireSettings fireSettings = new TurretFireSettings();
        [Space, SerializeField] public TurretRotatorSettings rotatorSettings = new TurretRotatorSettings();
        [Space, SerializeField] public TurretTargerFinderSettings targetFinderSettings = new TurretTargerFinderSettings();
    }
}