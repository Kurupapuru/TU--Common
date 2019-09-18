namespace AnimationTools.CurveParticles {
    using UnityEngine;

    [CreateAssetMenu(menuName = "Settings/AnimationTools/CurveParticleSystemManagerSettings")]
    public class CurveParticleSystemManagerSettings : ScriptableObject {
        public ParticleSystem                       particleSystemPrefab;
        public Vector3Curve sharedInfo;

        public int   count           = 1;
        public float animationLength = 3;

        public float randomLengthOffset        = 0;
        public float randomStartPositionOffset = 0;
        public float randomEndPositionOffset   = 0;
    }
}