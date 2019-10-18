namespace AnimationTools.CurveParticles {
    using System;
    using UnityEngine;

    [Serializable]
    public class AnimationCurveBasedVector3SharedInfo {
        public AnimationCurve globalCurve;
        public AnimationCurve XCurve;
        public AnimationCurve YCurve;
        public AnimationCurve ZCurve;

        public Vector3 Evaluate(Vector3 beginPosition, Vector3 endPosition,  float progress) {
            var lerpValue = globalCurve.Evaluate(progress);
            return new Vector3(
                Mathf.Lerp(beginPosition.x, endPosition.x, XCurve.Evaluate(lerpValue)),
                Mathf.Lerp(beginPosition.y, endPosition.y, YCurve.Evaluate(lerpValue)),
                Mathf.Lerp(beginPosition.z, endPosition.z, ZCurve.Evaluate(lerpValue)));
        }

        public AnimationCurveBasedVector3SharedInfo() {
            this.globalCurve = AnimationCurve.Linear(0, 0, 1, 1);
            this.XCurve      = AnimationCurve.Linear(0, 0, 1, 1);
            this.YCurve      = AnimationCurve.Linear(0, 0, 1, 1);
            this.ZCurve      = AnimationCurve.Linear(0, 0, 1, 1);
        }
    }
}