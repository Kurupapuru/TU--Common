namespace AnimationTools.CurveParticles {
    using System;
    using AnimationTasks;
    using CurveParticles;
    using UnityEngine;

    public class CurveVector3 : AnimationTask {
        public Vector3 value => this.sharedInfo.Evaluate(this.beginPosition, this.endPosition, progress);

        protected AnimationCurveBasedVector3SharedInfo sharedInfo;
        protected Vector3                              beginPosition;
        protected Vector3                              endPosition;


        public CurveVector3(AnimationCurveBasedVector3SharedInfo sharedInfo, Vector3 beginPosition, Vector3 endPosition, float length) {
            this.sharedInfo    = sharedInfo;
            this.beginPosition = beginPosition;
            this.endPosition   = endPosition;
            this.length        = length;
        }

        protected override void Update() { }
    }
}