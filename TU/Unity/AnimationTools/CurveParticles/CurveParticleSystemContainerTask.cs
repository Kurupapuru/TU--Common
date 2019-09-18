namespace AnimationTools.CurveParticles {
    using System;
    using AnimationTasks;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public class CurveParticleSystemContainerTask : AnimationTasksContainer {
        public ParticleSystem ps;

        private ParticleSystem.Particle[] particlesCacheArray;

        public CurveParticleSystemContainerTask(ParticleSystem psSettingsComponent) {
            this.ps = Object.Instantiate(psSettingsComponent.gameObject).GetComponent<ParticleSystem>();
        }


        public bool IsFree => this.ps.particleCount == 0;


        public override void Play() {
            this.particlesCacheArray = new ParticleSystem.Particle[this.Tasks.Length];

            if (!this.IsFree) throw new NotSupportedException("One Animation Curve Particle System can process only one group of particles at the same time");

            this.ps.Emit(this.Tasks.Length);
            base.Play();
        }

        protected override void Update() {
            base.Update();

            this.ps.GetParticles(particlesCacheArray);

            for (int i = 0; i < particlesCacheArray.Length; i++) {
                particlesCacheArray[i].position = ((CurveVector3) this.Tasks[i]).value;
            }

            this.ps.SetParticles(particlesCacheArray);
        }
    }
}