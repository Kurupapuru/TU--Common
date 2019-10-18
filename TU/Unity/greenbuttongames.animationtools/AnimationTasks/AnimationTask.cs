using UnityEngine;

namespace AnimationTools.AnimationTasks {
    using System;
    using System.Collections;
    using UniRx;
    using UnityEngine;

    public class AnimationTask : IAnimationTask {
        private IDisposable updateTask;

        public ReactiveCommand<IAnimationTask> onPlay            { get; protected set; }
        public ReactiveCommand<IAnimationTask> onPause           { get; protected set; }
        public ReactiveCommand<IAnimationTask> onUpdate          { get; protected set; }
        public ReactiveCommand<IAnimationTask> onFinish          { get; protected set; }
        public float                           progress          { get; protected set; }
        public float                           progressInSeconds { get; protected set; }
        public float                           length            { get; protected set; }


        public AnimationTask(float length = 1) {
            onPlay   = new ReactiveCommand<IAnimationTask>();
            onPause  = new ReactiveCommand<IAnimationTask>();
            onUpdate = new ReactiveCommand<IAnimationTask>();
            onFinish = new ReactiveCommand<IAnimationTask>();
            this.length = length;
        }

        #region Public Methods

        public virtual void Play() {
            if (progress >= 1) progress = 0;
            this.updateTask = Observable.EveryUpdate().Subscribe(_ => this.EveryUpdate(Time.deltaTime));

            if (!onPlay.IsDisposed) this.onPlay.Execute(this);
        }

        public virtual void Pause() {
            this.updateTask?.Dispose();

            if (!onPause.IsDisposed) onPause.Execute(this);
        }

        public virtual void Stop() {
            this.Dispose();
        }

        public virtual void Dispose() {
            this.updateTask?.Dispose();
            if (!onPlay.IsDisposed) this.onPlay.Dispose();
            if (!onUpdate.IsDisposed) onUpdate.Dispose();
            if (!onFinish.IsDisposed) onFinish.Dispose();
        }

        #endregion


        private void EveryUpdate(float deltaTime) {
            var progressValues = ProgressUpdate(deltaTime);
            this.progressInSeconds = progressValues.Item1;
            this.progress          = progressValues.Item2;

            this.Update();

            if (!onUpdate.IsDisposed) this.onUpdate.Execute(this);

            // Finish?
            if (progress >= 1) {
                if (!onFinish.IsDisposed) this.onFinish.Execute(this);
                this.Stop();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <returns>(progressInSeconds, [from 0 to 1]progress)</returns>
        protected virtual ValueTuple<float, float> ProgressUpdate(float deltaTime) {
            var progressInSeconds = this.progressInSeconds + deltaTime;
            var progress          = Mathf.InverseLerp(0, length, progressInSeconds);

            return (progressInSeconds, progress);
        }

        protected virtual void Update()
        {
        }
    }
}