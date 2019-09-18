using Plugins.Lean.Pool;
using TU.Unity.HealthSystem.Interfaces;
using UniRx;
using UnityEngine;

namespace TU.Unity.World
{
    public class ExplosionBarrel : MonoBehaviour
    {
        public ExplosionSettings settings;

        // PRIVATE
        private bool calculate = false;
        private float timerNow;
        private Vector3 explosionPos;
        private Collider[] colliders;

        private void Start()
        {
            GetComponent<IHealthSystem>().Alive.Subscribe(alive =>
            {
                if (!alive)
                {
                    ExplosionCreator.Explosion(transform.position, settings);
                    gameObject.Despawn();
                }
            });
        }

        void OnDrawGizmosSelected()
        {
            float radius;
            if (settings == null)
                return;
            if (settings.damageExplosionsLayers != null && settings.damageExplosionsLayers.Length > 0)
            {
                radius = settings.damageExplosionsLayers[0].radius;
            }
            else if (settings.physicsExplosionsLayers != null && settings.physicsExplosionsLayers.Length > 0)
            {
                radius = settings.physicsExplosionsLayers[0].radius;
            }
            else
                return;

            Vector3 zeroPoint = transform.position;

            float r = radius + 0.05f;
            float a = 1.1f;

            for (int i = 0; i < 10; i++)
            {
                r -= 0.05f;
                a -= 0.1f;
                float theta = 0;
                float x = r * Mathf.Cos(theta);
                float y = r * Mathf.Sin(theta);
                Vector3 pos = zeroPoint + new Vector3(x, 0, y);
                Vector3 newPos = pos;
                Vector3 lastPos = pos;
                for (theta = 0.15f; theta < Mathf.PI * 2; theta += 0.15f)
                {
                    x = r * Mathf.Cos(theta);
                    y = r * Mathf.Sin(theta);
                    newPos = zeroPoint + new Vector3(x, 0, y);
                    Gizmos.color = new Color(1, 0, 0, a);
                    Gizmos.DrawLine(pos, newPos);
                    pos = newPos;
                }

                Gizmos.color = new Color(1, 0, 0, a);
                Gizmos.DrawLine(pos, lastPos);
            }
        }
    }
}