namespace AnimationTools.AnimationTasks {
    using System;
    using UniRx;

    public interface IAnimationTask {
        ReactiveCommand<IAnimationTask> onPlay           { get; }
        ReactiveCommand<IAnimationTask> onPause           { get; }
        ReactiveCommand<IAnimationTask> onUpdate          { get; }
        ReactiveCommand<IAnimationTask> onFinish          { get; }
        float                           progress          { get; }
        float                           progressInSeconds { get; }
        float                           length            { get; }


        void Play();
        void Pause();
        void Stop();

        void Dispose();
    }
}