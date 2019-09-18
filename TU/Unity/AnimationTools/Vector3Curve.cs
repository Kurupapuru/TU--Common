namespace AnimationTools.CurveParticles {
    using System;
    using UnityEngine;

    [Serializable]
    public class Vector3Curve : IVector3Curve {
        public AnimationCurve globalCurve;
        public AnimationCurve XCurve;
        public AnimationCurve YCurve;
        public AnimationCurve ZCurve;

        public virtual Vector3 Evaluate(Vector3 beginPosition, Vector3 endPosition,  float progress) {
            var lerpValue = globalCurve.Evaluate(progress);
            return new Vector3(
                Mathf.LerpUnclamped(beginPosition.x, endPosition.x, XCurve.Evaluate(lerpValue)),
                Mathf.LerpUnclamped(beginPosition.y, endPosition.y, YCurve.Evaluate(lerpValue)),
                Mathf.LerpUnclamped(beginPosition.z, endPosition.z, ZCurve.Evaluate(lerpValue)));
        }

        public Vector3Curve() {
            this.globalCurve = AnimationCurve.Linear(0, 0, 1, 1);
            this.XCurve      = AnimationCurve.Linear(0, 0, 1, 1);
            this.YCurve      = AnimationCurve.Linear(0, 0, 1, 1);
            this.ZCurve      = AnimationCurve.Linear(0, 0, 1, 1);
        }

        public Vector3Curve(AnimationCurve globalCurve = null, AnimationCurve xCurve = null, AnimationCurve yCurve = null, AnimationCurve zCurve = null)
        {
            if (this.globalCurve == null) 
                this.globalCurve = AnimationCurve.Linear(0, 0, 1, 1);
            if (xCurve == null)
                xCurve      = AnimationCurve.Linear(0, 0, 1, 1);
            if (yCurve == null) 
                yCurve      = AnimationCurve.Linear(0, 0, 1, 1);
            if (zCurve == null) 
                zCurve      = AnimationCurve.Linear(0, 0, 1, 1);
            
            this.globalCurve = globalCurve;
            XCurve = xCurve;
            YCurve = yCurve;
            ZCurve = zCurve;
        }
    }

    public interface IVector3Curve
    {
        Vector3 Evaluate(Vector3 beginPosition, Vector3 endPosition, float progress);
    }
}