# Animation Tools
## AnimationTasks
Abstract package. Содержит в себе AnimationTask и AnimationTasksContainer, от которых можно наследоваться или использовать в качестве отдельного объекта подписываясь на его изменения.

*Пример подписи:*

    aTask = new AnimationTask(length: animationLength);
    aTask.onUpdate.Subscribe(UpdateCameraPos);
    aTask.onFinish.Subscribe(_ => CameraTransform.parent = lerpToTransform);
    aTask.Play();
Пример наследования:

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
    }

## CurveParticles
Реализация ***AnimationTasks***. Для более подробного просмотра примера использования есть демо сцена.
Пример использования менеджера:

    public class ExampleCurveParticleSystemEmit : MonoBehaviour {  
        [SerializeField] private CurveParticleSystemTasksManager manager = new CurveParticleSystemTasksManager(null);  
        [SerializeField] private Transform startPoint, endPoint;  
      
      
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

