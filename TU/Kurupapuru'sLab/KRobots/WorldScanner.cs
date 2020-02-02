using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KurupapuruLab.KRobots
{
    public static class WorldScanner
    {
        public static Vector3? GetPointFromScreenRay(Camera camera, Vector2 screenPos, LayerMask layerMask) =>
            GetHitFromScreenRay(camera, screenPos, layerMask)?.point;

        public static RaycastHit? GetHitFromScreenRay(Camera camera, Vector2 screenPos, LayerMask layerMask)
        {
            var ray = camera.ScreenPointToRay(screenPos);
            RaycastHit hit;

            bool raycastResult = Physics.Raycast(ray, out hit, 100f, layerMask);

            if (!raycastResult) return null;
            else return hit;
        }

        public static IEnumerable<Collider> GetAllColliders(Vector3 center, float radius, LayerMask layerMask,
            Func<Collider, bool> additionalPredicate)
        {
            return Physics.OverlapSphere(center, radius, layerMask).Where(additionalPredicate);
        }

        public static Collider GetClosestCollider(Vector3 center, float radius, LayerMask layerMask,
            Func<Collider, bool> additionalPredicate)
        {
            var colls = Physics.OverlapSphere(center, radius, layerMask);

            Collider closestColl = null;
            float closestDistance = float.PositiveInfinity;
            for (int i = 0; i < colls.Length; i++)
            {
                var coll = colls[i];
                var distance = (coll.transform.position - center).magnitude;
                if (distance < closestDistance && additionalPredicate.Invoke(coll))
                {
                    closestColl = coll;
                    closestDistance = distance;
                }
            }

            return closestColl;
        }
    }
}