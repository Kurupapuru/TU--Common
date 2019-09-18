namespace AnimationTools.AnimationTasks {
    using System;
    using UniRx;
    using UnityEngine;

    public class AnimationTasksContainer : AnimationTask, IAnimationTasksContainer {
        private readonly Action<IAnimationTask>           onProgressChanged;
        protected        IAnimationTask                   longestTask;
        public           IAnimationTask[]                 Tasks           { get; protected set; }


        // Constructor
        public AnimationTasksContainer() {
            this.Tasks = new IAnimationTask[0];
        }

        public AnimationTasksContainer(IAnimationTask[] tasks) {
            Initialize(tasks);
        }

        public void Initialize(IAnimationTask[] tasks) {
            this.Stop();

            this.Tasks = tasks;

            this.longestTask = tasks[0];
            for (int i = 1; i < tasks.Length; i++)
                if (this.Tasks[i].length > this.longestTask.length)
                    this.longestTask = tasks[i];
            length = this.longestTask.length;
        }


        #region Public Methods

        public override void Play() {
            base.Play();

            for (int i = 0; i < this.Tasks.Length; i++) {
                this.Tasks[i].Play();
            }
        }

        public override void Pause() {
            for (int i = 0; i < this.Tasks.Length; i++) {
                this.Tasks[i].Pause();
            }

            base.Pause();
        }

        public override void Stop() {
            for (int i = 0; i < this.Tasks.Length; i++) {
                this.Tasks[i].Stop();
            }

            base.Stop();
        }

        public override void Dispose() {
            for (int i = 0; i < this.Tasks.Length; i++) {
                this.Tasks[i].Dispose();
            }

            base.Dispose();
        }

        #endregion

        protected override ValueTuple<float, float> ProgressUpdate(float deltaTime) =>
            (this.longestTask.progressInSeconds, this.longestTask.progress);

        protected override void Update() {
        }
    }
}