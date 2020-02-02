using System;
using DefaultNamespace;
using TU.Unity.Identification;

namespace TU.Unity.Enabable
{
    [Serializable]
    public abstract class Behaviour : IEnabable
    {
        public AutoHashID identifier;
        
        
        public int Id => identifier.Id;

        private bool _enabled = false;
        public virtual bool enabled
        {
            get => _enabled;
            set
            {
                if (value && !_enabled)
                    OnEnable();
                else 
                if (!value && _enabled)
                    OnDisable();

                _enabled = value;
            }
        }

        protected virtual void OnEnable() {}
        protected virtual void OnDisable() {}
    }
}