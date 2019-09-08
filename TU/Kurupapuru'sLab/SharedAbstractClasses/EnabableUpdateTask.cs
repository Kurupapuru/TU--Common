using System;
using KurupapuruLab.KRobots;
using KurupapuruLab.SharedInterfaces;
using UniRx;

namespace KurupapuruLab.SharedAbstractClasses
{
    public abstract class EnabableUpdateTask : IEnabable
    {
        private IDisposable updateTask;
        public bool enabled
        {
            get { return updateTask != null; }
            set
            {
                if (!enabled && value)
                {
                    updateTask = Observable.EveryUpdate().Subscribe(_ => UpdateTaskMethod());
                }
                else if (enabled && !value)
                {
                    updateTask.Dispose();
                }
            }
        }

        protected abstract void UpdateTaskMethod();
    }
}