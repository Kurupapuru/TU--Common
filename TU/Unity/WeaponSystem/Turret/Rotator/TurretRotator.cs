using System;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace WeaponSystem.Turret
{
    [Serializable]
    public class TurretRotator
    {
        [SerializeField] public ITurretRotatorSettings settings;
        [SerializeField] public Transform verticalRotator;
        [SerializeField] public Transform horizontalRotator;
        [FormerlySerializedAs("firePoint")] 
        [SerializeField] public Transform lookPoint;
        [SerializeField] public Transform turretForward;

        [NonSerialized] public ReactiveProperty<bool> isRotatedToTarget = new ReactiveProperty<bool>();
        [NonSerialized] public IDisposable rotationTask;
        
        
        // Constructors
        public TurretRotator(){}
        public TurretRotator(ITurretRotatorSettings settings, Transform verticalRotator, Transform horizontalRotator, Transform lookPoint, Transform turretForward)
        {
            this.settings = settings;
            this.verticalRotator = verticalRotator;
            this.horizontalRotator = horizontalRotator;
            this.lookPoint = lookPoint;
            this.turretForward = turretForward;
        }

        
        // Methods
        public void OnTargetChanged(Transform target)
        {
            isRotatedToTarget.Value = false;
            rotationTask?.Dispose();

            if (target != null)
            {
                var targetColl = target.GetComponent<Collider>();
                rotationTask = targetColl != null ? 
                    Observable.EveryFixedUpdate().Subscribe(_ =>
                    {
                        if (targetColl == null)
                        {
                            rotationTask.Dispose();
                            return;
                        }
                        RotateToTarget(targetColl.bounds.center);
                        var rName = Random.Range(-1000, 1000).ToString();
                        targetColl.gameObject.name = rName;
                    }) 
                    : 
                    Observable.EveryFixedUpdate().Subscribe(_ => RotateToTarget(target.position));
            }
        }
        
        private void RotateToTarget(Vector3 targetPos)
        {
            var direction = targetPos - lookPoint.position;
            if (Vector3.Angle(turretForward.forward, direction.normalized) <= settings.maxRotateAngle)
            {
                // Rotating
                Vector3 directionEuler = Quaternion.LookRotation(direction).eulerAngles;

                var horizontalRotatorEuler = horizontalRotator.eulerAngles;
                horizontalRotatorEuler.y =
                    Mathf.LerpAngle(
                        horizontalRotatorEuler.y,
                        directionEuler.y,
                        settings.horizontalRotateSpeed * Time.fixedDeltaTime);
                horizontalRotator.eulerAngles = horizontalRotatorEuler;

                var verticalRotatorEuler = verticalRotator.eulerAngles;
                verticalRotatorEuler.x =
                    Mathf.LerpAngle(
                        verticalRotatorEuler.x,
                        directionEuler.x,
                        settings.verticalRotateSpeed * Time.fixedDeltaTime);
                verticalRotator.eulerAngles = verticalRotatorEuler;
            }

            isRotatedToTarget.Value = Vector3.Angle(direction.normalized, lookPoint.forward) <= settings.isRotatedAngle;
        }
    }
}