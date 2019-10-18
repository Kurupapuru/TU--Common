namespace AnimationTools.CurveParticles {
    using System;
    using System.Collections.Generic;
    using AnimationTasks;
    using UnityEngine;
    using Random = UnityEngine.Random;

    [Serializable]
    public class CurveParticleSystemTasksManager {
        public CurveParticleSystemManagerSettings settings;

        private List<CurveParticleSystemContainerTask> psContainers = new List<CurveParticleSystemContainerTask>();


        public CurveParticleSystemTasksManager(CurveParticleSystemManagerSettings settings) {
            this.settings = settings;
        }

        private CurveParticleSystemContainerTask GetFreeContainer() {
            // Searching free
            foreach (var psContainer in this.psContainers) {
                if (psContainer.IsFree) return psContainer;
            }

            // Didn't found, creating new
            var result = new CurveParticleSystemContainerTask(this.settings.particleSystemPrefab);
            this.psContainers.Add(result);
            return result;
        }

        public IAnimationTasksContainer Emit(Vector3 startPosition, Vector3 endPosition) {
            var psContainer = this.GetFreeContainer();

            var tasks = new IAnimationTask[this.settings.count];

            for (int i = 0; i < tasks.Length; i++) {
                if (this.settings.randomStartPositionOffset > 0) {
                    startPosition += this.RandomRadiusVector(this.settings.randomStartPositionOffset);
                }

                if (this.settings.randomEndPositionOffset > 0) {
                    endPosition += this.RandomRadiusVector(this.settings.randomEndPositionOffset);
                }

                var length = this.settings.animationLength;
                if (this.settings.randomLengthOffset > 0) {
                    length += Random.Range(-this.settings.randomLengthOffset, this.settings.randomLengthOffset);
                }

                tasks[i] = new CurveVector3(this.settings.sharedInfo, startPosition, endPosition, length);
            }

            psContainer.Initialize(tasks);
            psContainer.Play();
            return psContainer;
        }

        private Vector3 RandomRadiusVector(float radius) {
            return this.RandomBoxVector(1).normalized * Random.Range(0, this.settings.randomEndPositionOffset);
        }

        private Vector3 RandomBoxVector(float boxSize) {
            return new Vector3(
                Random.Range(-boxSize, boxSize),
                Random.Range(-boxSize, boxSize),
                Random.Range(-boxSize, boxSize));
        }
    }
}