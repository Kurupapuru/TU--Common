using System;
using UnityEngine;

namespace TU.Unity.Explosions
{
    //TODO: Добавить поддержку дамага

    [CreateAssetMenu(menuName = "Configations/Explosion Creator")]
    public class ExplosionCreator : ScriptableObject
    {
        [SerializeField] private int cacheLength = 25;

        [NonSerialized] private Collider[] collidersCache;
        
        public void Create(IExplosionParameters parameters)
        {
            if (collidersCache == null) collidersCache = new Collider[cacheLength];

            var collidersCount = Physics.OverlapSphereNonAlloc(parameters.Position, parameters.Radius, collidersCache, parameters.LayerMask);

            var forceOverDistance = parameters.ForceOverDistance;
            if (forceOverDistance == null) forceOverDistance = AnimationCurve.Linear(0, 1, 1, 0);
            
            for (int i = 0; i < collidersCount; i++)
            {
                var collider = collidersCache[i];

                if (collider.attachedRigidbody != null)
                {
                    var colliderRb = collider.attachedRigidbody;
                    var colliderPos = 
                        colliderRb.transform.TransformPoint(
                            colliderRb.centerOfMass);

                    var vector = colliderPos - parameters.Position;
                    var evaluateValue = Mathf.InverseLerp(0, parameters.Radius, vector.magnitude);
                    var force = parameters.Force * forceOverDistance.Evaluate(evaluateValue);
                    Debug.Log($"evaluateValue: {evaluateValue}, force: {force}");
                    colliderRb.AddForce(vector.normalized * force, parameters.ForceMode);
                }
            }
        }
    }
}