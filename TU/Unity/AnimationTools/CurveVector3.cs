namespace AnimationTools.CurveParticles
{
    using System;
    using AnimationTasks;
    using CurveParticles;
    using UnityEngine;

    public class CurveVector3 : AnimationTask
    {
        public Vector3 value => this.curveSettings.Evaluate(this.beginPosition, this.endPosition, progress);

        protected IVector3Curve curveSettings;
        protected Vector3      beginPosition;
        protected Vector3      endPosition;


        public CurveVector3(IVector3Curve curveSettings, Vector3 beginPosition, Vector3 endPosition, float length)
        {
            this.curveSettings = curveSettings;
            this.beginPosition = beginPosition;
            this.endPosition   = endPosition;
            this.length        = length;
        }
    }
}