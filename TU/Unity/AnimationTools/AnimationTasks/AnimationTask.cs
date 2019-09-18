namespace AnimationTools.AnimationTasks {
    using UniRx;
    using System;
    using UnityEngine;

    public class AnimationTask : IAnimationTask {
        private IDisposable updateTask;

        public virtual bool executeOnUpdateOnFinish { get; set; } = true;
        public virtual ReactiveCommand<IAnimationTask> onPlay            { get;  set; } = new ReactiveCommand<IAnimationTask>();
        public virtual ReactiveCommand<IAnimationTask> onPause           { get;  set; } = new ReactiveCommand<IAnimationTask>(); 
        public virtual ReactiveCommand<IAnimationTask> onUpdate          { get;  set; } = new ReactiveCommand<IAnimationTask>();
        public virtual ReactiveCommand<IAnimationTask> onFinish          { get;  set; } = new ReactiveCommand<IAnimationTask>();
        public virtual float                           progress          { get; protected set; }
        public virtual float                           progressInSeconds { get; protected set; }
        public virtual float                           length            { get; protected set; }


        public AnimationTask(float length = 1) {
            this.length = length;
            Initialize();
        }

        #region Public Methods

        public virtual void Initialize()
        {
            if (onPlay == null)   onPlay   = new ReactiveCommand<IAnimationTask>();
            if (onPause == null)  onPause  = new ReactiveCommand<IAnimationTask>();
            if (onUpdate == null) onUpdate = new ReactiveCommand<IAnimationTask>();
            if (onFinish == null) onFinish = new ReactiveCommand<IAnimationTask>();
        }
        
        public virtual void Play() {
            if (progress >= 1) progress = 0;
            this.updateTask = Observable.EveryUpdate().Subscribe(_ => this.EveryUpdate(Time.deltaTime));

            if (!onPlay.IsDisposed) this.onPlay.Execute(this);
        }

        public virtual void Pause() {
            this.updateTask?.Dispose();

            if (!onPause.IsDisposed) onPause.Execute(this);
        }

        public virtual void Stop()
        {
            progressInSeconds = 0;
            this.Dispose();
        }

        public virtual void Dispose() {
            this.updateTask?.Dispose();
            if (!onPlay.IsDisposed) {
                onPlay.Dispose();
                onPlay = new ReactiveCommand<IAnimationTask>();
            }
            if (!onUpdate.IsDisposed) {
                onUpdate.Dispose();
                onUpdate = new ReactiveCommand<IAnimationTask>();
            }
            if (!onFinish.IsDisposed) {
                onFinish.Dispose();
                onFinish = new ReactiveCommand<IAnimationTask>();
            }
        }

        #endregion


        private void EveryUpdate(float deltaTime) {
            var progressValues     = ProgressUpdate(deltaTime);
            this.progressInSeconds = progressValues.Item1;
            this.progress          = progressValues.Item2;

            this.Update();

            if (!onUpdate.IsDisposed) this.onUpdate.Execute(this);

            // Finish?
            if (progress >= 1) {
                if (executeOnUpdateOnFinish && !onUpdate.IsDisposed) this.onUpdate.Execute(this);
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