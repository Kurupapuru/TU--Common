namespace AnimationTools.CurveParticles {
    using AnimationTasks;
    using UniRx;
    using UnityEngine;

    public class ExampleCurveParticleSystemEmit : MonoBehaviour {
        [SerializeField] private CurveParticleSystemTasksManager manager = new CurveParticleSystemTasksManager(null);
        [SerializeField] private Transform                       startPoint, endPoint;


        private float lastEmitTime;

        private void Update() {
            if (Time.time - this.lastEmitTime > this.manager.settings.animationLength) {
                this.lastEmitTime = Time.time;

                var animationTasksContainer = this.manager
                    .Emit(this.startPoint.position, this.endPoint.position);

                foreach (var task in animationTasksContainer.Tasks) {
                    task.onFinish.Subscribe(this.OnParticleFinishedMovement);
                }
                
            }
        }

        private void OnParticleFinishedMovement(IAnimationTask task) {
            Debug.Log("Particle Finished Movement");
        }
    }
}