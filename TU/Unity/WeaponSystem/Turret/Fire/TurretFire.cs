using System;
using Plugins.Lean.Pool;
using UniRx;
using UnityEngine;
using WeaponSystem.Ammo;

namespace WeaponSystem.Turret
{
    [Serializable]
    public class TurretFire
    {
        public bool enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;

                if (value && !_isBusy)
                    StartFire();
            }
        }

        [SerializeField] public AudioSource audioSource;
        [SerializeField] public Transform[] firePoints;

        [NonSerialized] public Transform target;
        [NonSerialized] public ITurretFireSettings settings;
        [NonSerialized] public IDisposable fireTask;
        [NonSerialized] private int _currentAmmoCount;
        [NonSerialized] private bool _enabled;
        [NonSerialized] private bool _isBusy;
        [NonSerialized] private int currentFirePointId;
        protected IAmmoSettings ammoSettings => settings.ammoSettings;

        // Methods
        public virtual void Initialize()
        {
            _currentAmmoCount = settings.ammoCount;
        }

        private void StartFire(long l = default)
        {
            if (!_enabled) return;

            FireOneShot();
        }

        public void FireOneShot()
        {
            #region Shot

            if (settings.fireSound != null)
                audioSource.PlayOneShot(settings.fireSound, settings.fireSoundVolume);

            // Getting FirePoint
            var firePoint = firePoints[currentFirePointId];
            currentFirePointId++;
            if (currentFirePointId >= firePoints.Length)
                currentFirePointId = 0;

            var ammoTransform = AmmoSpawn(firePoint);
            var ammo = AmmoInitialize(ammoTransform);
            _currentAmmoCount--;

            #endregion

            fireTask?.Dispose();

            if (_currentAmmoCount == 0)
                Reload();
            else
                FireDelay();
        }

        protected virtual Transform AmmoSpawn(Transform firePoint)
        {
            var ammoTransform = settings.ammoPrefab.Spawn<Transform>();
            ammoTransform.position = firePoint.position;
            ammoTransform.rotation = firePoint.rotation;
            
            ammoTransform.gameObject.SetActive(true);
            
            return ammoTransform;
        }

        protected virtual IInitializableAmmo AmmoInitialize(Transform ammoTransform)
        {
            var ammo = ammoTransform.GetComponent<IInitializableAmmo>();
            if (ammo != null)
                ammo.Initialize(target, ammoSettings);
            else
                Debug.Log("Cant find IInitializableAmmo component on used ammo");

            return ammo;
        }

        protected virtual void Reload()
        {
            if (settings.reloadSound != null)
                audioSource.PlayOneShot(settings.reloadSound, settings.reloadSoundVolume);
            _isBusy = true;
            fireTask = Observable
                .Timer(TimeSpan.FromSeconds(settings.reloadTime))
                .Subscribe(_ =>
                {
                    _currentAmmoCount = settings.ammoCount;
                    _isBusy = false;
                    StartFire();
                });
        }

        protected virtual void FireDelay()
        {
            _isBusy = true;
            fireTask = Observable
                .Timer(TimeSpan.FromSeconds(settings.fireDelay))
                .Subscribe(_ =>
                {
                    _isBusy = false;
                    StartFire();
                });
        }
    }
}