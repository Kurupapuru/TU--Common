namespace AnimationTools.AnimationTasks {
    using System;
    using UniRx;

    public interface IAnimationTasksContainer : IAnimationTask {
        IAnimationTask[]         Tasks { get; }
    }
}