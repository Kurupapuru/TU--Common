namespace DefaultNamespace
{
    using System;
    using AnimationTools.CurveParticles;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using Random = UnityEngine.Random;

    [Serializable]
    public class Vector3CurveRandomized : IVector3Curve
    {
        public Vector3Curve curve1 = new Vector3Curve();
        public Vector3Curve curve2 = new Vector3Curve();
        
        [Range(0f, 1f)] public float globalCurveRValue = .5f;
        [Range(0f, 1f)] public float xCurveRValue      = .5f;
        [Range(0f, 1f)] public float yCurveRValue      = .5f;
        [Range(0f, 1f)] public float zCurveRValue      = .5f;
        

        public Vector3CurveRandomized GetRandomizedCopy()
        {
            var randomizedCopy = new Vector3CurveRandomized() {
                curve1 = this.curve1,
                curve2 = this.curve2
            };
            randomizedCopy.RandomizeRValues();
            return randomizedCopy;
        }

        public Vector3 Evaluate(Vector3 beginPosition, Vector3 endPosition, float progress)
        {
            var lerpValue = Mathf.Lerp(curve1.globalCurve.Evaluate(progress), curve2.globalCurve.Evaluate(progress), globalCurveRValue);
            var xValue = Mathf.Lerp(curve1.XCurve.Evaluate(lerpValue), curve2.XCurve.Evaluate(lerpValue), xCurveRValue);
            var yValue = Mathf.Lerp(curve1.YCurve.Evaluate(lerpValue), curve2.YCurve.Evaluate(lerpValue), yCurveRValue);
            var zValue = Mathf.Lerp(curve1.ZCurve.Evaluate(lerpValue), curve2.ZCurve.Evaluate(lerpValue), zCurveRValue);
            
            return new Vector3(
                Mathf.LerpUnclamped(beginPosition.x, endPosition.x, xValue),
                Mathf.LerpUnclamped(beginPosition.y, endPosition.y, yValue),
                Mathf.LerpUnclamped(beginPosition.z, endPosition.z, zValue));
        }
        
        
        [Button]
        public void RandomizeRValues(bool global = true, bool x = true, bool y = true, bool z = true)
        {
            if (global) globalCurveRValue = Random.Range(0f, 1f);
            if (x)      xCurveRValue      = Random.Range(0f, 1f);
            if (y)      yCurveRValue      = Random.Range(0f, 1f);
            if (z)      zCurveRValue      = Random.Range(0f, 1f);
        }
    }
}