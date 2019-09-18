using Plugins.Lean.Pool;

namespace WeaponSystem.Ammo
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using System.Collections;

    /// <summary>
    /// + You must call Hit() at some time
    /// + You must override OnHit(), do damage here or something like that
    /// + You can add something to Initialize, but call base.Initialize(_, _)
    /// </summary>
    public abstract class AbstractAmmo : MonoBehaviour, IInitializableAmmo
    {
        [SerializeField] public float        despawnAfter = 1f;
        [SerializeField] public GameObject[] enableOnHit;
        [SerializeField] public GameObject[] disableOnHit;

        [NonSerialized] public Transform     target;
        [NonSerialized] public Collider      targetColl;
        [NonSerialized] public IAmmoSettings settings;

        [NonSerialized] private List<bool> savedActiveList;

        protected Vector3 targetPos => targetColl != null ? targetColl.bounds.center : target.position;

        #region ActiveList

        private void FillActiveList(Transform t)
        {
            savedActiveList.Add(t.gameObject.activeSelf);
            foreach (var tChild in t)
                FillActiveList((Transform) tChild);
        }

        private void ResetActive(Transform t, ref int idInList)
        {
            t.gameObject.SetActive(savedActiveList[idInList]);
            idInList++;
            foreach (var tChild in t)
                ResetActive((Transform) tChild, ref idInList);
        }

        #endregion


        public virtual void Initialize(Transform target, IAmmoSettings ammoSettings, bool createMuzzle = true)
        {
            if (target == null) {
                gameObject.Despawn();
                return;
            }

            this.target   = target;
            targetColl    = target.GetComponent<Collider>();
            this.settings = ammoSettings;

            // Muzzle Effect
            if (createMuzzle && ammoSettings.muzzleEffectPrefab != null) {
                var muzzleT = ammoSettings.muzzleEffectPrefab.Spawn<Transform>();
                muzzleT.gameObject.SetActive(true);
                muzzleT.position = transform.position;
                muzzleT.rotation = transform.rotation;
            }

            if (savedActiveList == null) {
                savedActiveList = new List<bool>();
                FillActiveList(transform);
            }
            else {
                int idInList = 0;
                ResetActive(transform, ref idInList);
            }
        }

        protected IEnumerator Hit(Transform hittedObjectT)
        {
            foreach (var obj in disableOnHit)
                obj.SetActive(false);

            foreach (var obj in enableOnHit)
                obj.SetActive(true);

            CreateHitEffect();

            OnHit(hittedObjectT);

            // Despawn
            yield return new WaitForSeconds(despawnAfter);
            gameObject.Despawn();

            // Sub Methods
            void CreateHitEffect()
            {
                foreach (var hitEffect in settings.hitEffects) {
                    if (!hitEffect.layerMask.LayerCheck(hittedObjectT.gameObject.layer)) continue;
                    var hitEffectT = hitEffect.prefab.Spawn<Transform>();
                    hitEffectT.position = transform.position;
                    hitEffectT.rotation = transform.rotation;
                    return;
                }
            }
        }

        protected abstract void OnHit(Transform hittedObject);

        // Helpers
        protected RaycastHit RaycastCheck(Vector3 prevPos, Vector3 currentPos)
        {
            var raycastDir = currentPos - prevPos;
            Physics.Raycast(
                ray: new Ray(prevPos, raycastDir),
                hitInfo: out var hit,
                maxDistance: raycastDir.magnitude,
                layerMask: settings.attackMask);
            return hit;
        }
    }
}