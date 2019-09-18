using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace WeaponSystem.Turret
{
    [Serializable]
    public class TurretTargetFinder
    {
        // Settings
        [SerializeField] public Transform scanCenter;
        [NonSerialized] public ITurretTargetFinderSettings settings;
        [NonSerialized] public Func<Collider, bool> additionalPredicate = null;

        // Public Logic
        [NonSerialized] public ReactiveProperty<Transform> currentTarget = new ReactiveProperty<Transform>();
        [NonSerialized] public ReactiveProperty<Collider> currentTargetCollider = new ReactiveProperty<Collider>();
        public bool enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                
                targetUpdateTask?.Dispose();
                
                if (value)
                    targetUpdateTask = Observable
                        .Timer(TimeSpan.FromSeconds(settings.targetScanDelay))
                        .Repeat()
                        .Subscribe(_ => TargetUpdate());
            }
        }
        
        // Private
        private IDisposable targetUpdateTask;
        private bool _enabled;

        
        // Constructors
        public TurretTargetFinder() {}
        public TurretTargetFinder(ITurretTargetFinderSettings settings, Transform scanCenter, Func<Collider, bool> additionalPredicate = null)
        {
            this.settings = settings;
            this.additionalPredicate = additionalPredicate;
        }

        private void TargetUpdate()
        {
            var scanCenterPos = scanCenter.position;
            
            if (settings.targetLock && currentTarget.Value!=null && IsAcceptableTarget(currentTarget.Value, currentTargetCollider.Value, (currentTarget.Value.position - scanCenter.position)))
                return;
            else
            {
                Collider[] possibleTargets = new Collider[0];
                Transform priorTarget = null;
                Collider priorCollider = null;
                foreach (var targetMask in settings.targetMasks)
                {
                    possibleTargets = Physics.OverlapSphere(scanCenterPos, settings.maxRadius, targetMask);
                    
                    if (additionalPredicate != null)
                        possibleTargets = possibleTargets.Where(additionalPredicate).ToArray();

                    float priorTargetDistance;
                    priorTargetDistance = settings.closestTarget ? float.MaxValue : float.MinValue;
                    
                    for (int i = 0; i < possibleTargets.Length; i++)
                    {
                        var targetCollider = possibleTargets[i];
                        var targetTransform = possibleTargets[i].transform;
                        var direction = (targetTransform.position - scanCenterPos);
                        var distance = direction.magnitude;
                    
                        
                        if ((distance < priorTargetDistance && settings.closestTarget) || (distance > priorTargetDistance && !settings.closestTarget) 
                            && IsAcceptableTarget(targetTransform, targetCollider, direction))
                        {
                            priorTarget = targetTransform;
                            priorCollider = targetCollider;
                            priorTargetDistance = distance;
                        }
                    }
                    
                    if (priorTarget!=null) break;
                }
                
                currentTarget.Value = priorTarget;
                currentTargetCollider.Value = priorCollider;
            }
        }

        public bool IsAcceptableTarget(Transform target, Collider targetColl, Vector3 direction)
        {
            if (!(additionalPredicate != null && additionalPredicate.Invoke(targetColl))) return false;
            if (!(Vector3.Angle(direction.normalized, scanCenter.forward) <= settings.targetAllowedAngle)) return false;
            if (!(direction.magnitude <= settings.maxRadius)) return false;
            if (!(direction.magnitude >= settings.minRadius)) return false;

            return true;
        }
    }
}