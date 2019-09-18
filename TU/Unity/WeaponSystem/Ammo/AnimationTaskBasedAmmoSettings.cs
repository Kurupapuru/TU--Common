using System;
using UnityEngine;

namespace WeaponSystem.Ammo
{
    [Serializable]
    public class AnimationTaskBasedAmmoSettings : AbstractAmmoSettings
    {
        [SerializeField] public AnimationCurve flyCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] public AnimationCurve yAddCurve = AnimationCurve.Constant(0, 1, 0);
        [SerializeField] public float yAddCurveMult = 1;
    }
}