using TU.Unity.HealthSystem.Interfaces;
using UniRx;
using UnityEngine;

namespace WeaponSystem.Turret
{
    public class TurretController : MonoBehaviour
    {
        public TurretSettings settings;

        public TurretTargetFinder targetFinder = new TurretTargetFinder();
        public TurretRotator      rotator      = new TurretRotator();
        public TurretFire         fire         = new TurretFire();

        private CompositeDisposable disposables = new CompositeDisposable();

        protected void Awake()
        {
            targetFinder.settings = settings.targetFinderSettings;
            rotator.settings      = settings.rotatorSettings;
            fire.settings         = settings.fireSettings;
            fire.Initialize();

            targetFinder.additionalPredicate = coll => {
                var collHealthSystem = coll.GetComponent<IHealthSystem>();
                return collHealthSystem != null && collHealthSystem.Alive.Value;
            };
        }

        void OnEnable()
        {
            targetFinder.enabled = true;

            targetFinder.currentTarget
                .Subscribe(_ => { rotator.OnTargetChanged(_); })
                .AddTo(disposables);

            rotator.isRotatedToTarget
                .Subscribe(isRotated => { fire.enabled = isRotated; })
                .AddTo(disposables);
            targetFinder.currentTarget
                .Subscribe(newTarget => { fire.target = newTarget; })
                .AddTo(disposables);
        }

        public void OnDestroy()
        {
            targetFinder.enabled             = false;
            targetFinder.currentTarget.Value = null;
            disposables?.Dispose();
        }
    }
}