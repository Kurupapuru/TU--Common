using System;
using UnityEngine;

namespace TU.Unity.Explosions
{
    [Serializable]
    public struct ExplosionParametersStruct : IExplosionParameters
    {
        [SerializeField] private float force;
        [SerializeField] private float radius;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private AnimationCurve forceOverDistance;
        [SerializeField] private ForceMode forceMode;


        public Vector3 Position { get; set; }
        public float Force => force;
        public float Radius => radius;
        public LayerMask LayerMask => layerMask;
        public AnimationCurve ForceOverDistance => forceOverDistance;
        public ForceMode ForceMode => forceMode;
    }
}