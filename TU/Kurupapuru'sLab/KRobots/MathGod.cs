using UnityEngine;

namespace KurupapuruLab.KRobots
{
    public static class MathGod
    {
        public static Vector3 LerpEulerAngle(Vector3 lerpFrom, Vector3 lerpTo, float t)
        {
            return new Vector3(
                Mathf.LerpAngle(lerpFrom.x, lerpTo.x, t),
                Mathf.LerpAngle(lerpFrom.y, lerpTo.y, t),
                Mathf.LerpAngle(lerpFrom.z, lerpTo.z, t));
        }

        public static bool AreVectorsEqual(Vector3 v1, Vector3 v2, float maxDistance)
        {
            return (v1 - v2).magnitude <= maxDistance;
        }

        public static bool AreQuaternionsEqual(Quaternion q1, Quaternion q2, float maxAngle)
        {
            return Quaternion.Angle(q1, q2) <= maxAngle;
        }
    }
}