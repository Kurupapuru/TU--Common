using TU.Unity.CameraRelated;
using UnityEngine;

namespace UnicornLib.Input
{
    public static class RaycastHelper
    {
        public static Ray GetRay(Vector2 screenPoint)
        {
            return MainCamera.camera.Value.ScreenPointToRay(screenPoint);
        }
        
        public static RaycastHit GetRayHit(Vector2 screenPoint, int layerMask, float maxDistance = 100)
        {
            Ray InputRay = GetRay(screenPoint); 
            Physics.Raycast(InputRay, out var InputRayInfo, maxDistance, layerMask);
            return InputRayInfo;
        }

        public static RaycastHit[] GetRayHitAll(Vector2 screenPoint, int layerMask, float maxDistance = 100)
        {
            Ray InputRay = GetRay(screenPoint); 
            return Physics.RaycastAll(InputRay, maxDistance, layerMask);
        }

        public static Vector3? DirectionToHit(Vector3 from, Vector2 screenPoint, int layerMask, float maxDistance = 100)
        {
            var rayHit = GetRayHit(screenPoint, layerMask, maxDistance);

            if (rayHit.collider == null)
                return null;

            return rayHit.point - from;
        }
    }
}