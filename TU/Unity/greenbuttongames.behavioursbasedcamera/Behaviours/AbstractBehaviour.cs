namespace BehavioursBasedCamera
{
    using System;
    using UniRx;
    using UnityEngine;

    [Serializable]
    public abstract class AbstractBehaviour
    {
        public abstract bool             isEnabled        { get; protected set; }
        public virtual  IBehavioursBasedCameraController BehavioursBasedCameraController { get; set; }

        protected IDisposable updateTask;

        public virtual AbstractBehaviour Initialize(IBehavioursBasedCameraController behavioursBasedCameraController)
        {
            this.BehavioursBasedCameraController = behavioursBasedCameraController;
            return this;
        }
        
        public virtual void Enable()
        {
            if (Application.isPlaying)
                updateTask = Observable.EveryUpdate().Subscribe(_ => Update());
            isEnabled = true;
        }

        protected virtual void Update()
        {
            
        }

        public virtual void Disable()
        {
            isEnabled = false;
            updateTask?.Dispose();
        }

        public abstract AbstractBehaviour Copy();
    }
}