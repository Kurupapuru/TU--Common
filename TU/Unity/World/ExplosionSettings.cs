using System;
using UnityEngine;

namespace TU.Unity.World
{
    [CreateAssetMenu(menuName = "Settings/Explosion Settings")]
    public class ExplosionSettings : ScriptableObject
    {
        public ParticleSystem explPS;
        public DamageExplosion[] damageExplosionsLayers;
        public PhysicsExplosion[] physicsExplosionsLayers;

        [Serializable]
        public struct PhysicsExplosion
        {
            [Header("Common")] public string name;
            public float radius;
            public float activateTime;

            [Header("For AddExplosionForce")] public LayerMask layerMask;
            public float power;
            public float upwardsModifier;
            public ForceMode forceMode;

            [Header("For SphereCollExplosion")] public int sphereCollLayer;
            public float despawnSphereAfter;
        }

        [Serializable]
        public struct DamageExplosion
        {
            public string name;
            public LayerMask layerMask;
            public float damage;
            public float radius;
            public float activateAfter;
        }
    }
}