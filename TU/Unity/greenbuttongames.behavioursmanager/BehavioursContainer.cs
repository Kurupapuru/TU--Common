using System;
using System.Collections.Generic;
using BehavioursManager;
using BehavioursManager.Interfaces;
using KurupapuruLab.SharedInterfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BehavioursManager
{
    [Serializable, CreateAssetMenu(menuName = "Settings/BehavioursContainer")]
    public class BehavioursContainer : SerializedScriptableObject, IBehavioursContainer
    {
        #region SharedVariables

        [SerializeField]
        private Dictionary<string, IValueHolder> _sharedVariables = new Dictionary<string, IValueHolder>();

        public IValueHolder FindOrAddSharedVariable(string variableName, Type valueType)
        {
            var result = FindSharedVariable(variableName);
            if (result != null) return result;

            var newHolder = Activator.CreateInstance(typeof(ValueHolder<>).MakeGenericType(valueType));
            _sharedVariables.Add(variableName, (IValueHolder) newHolder);
            return (IValueHolder) newHolder;
        }

        public ValueHolder<T> FindOrAddSharedVariable<T>(string variableName, T setValueIfNotFound = default)
        {
            var result = FindSharedVariable(variableName);
            if (result != null) return (ValueHolder<T>) result;

            var newHolder = new ValueHolder<T>(setValueIfNotFound);
            _sharedVariables.Add(variableName, newHolder);
            return newHolder;
        }

        public IValueHolder FindSharedVariable(string variableName)
        {
            var isFound = _sharedVariables.TryGetValue(variableName, out var result);
            return result;
        }

        #endregion

        public List<IBehaviour> behaviours;

        public bool enabled
        {
            get
            {
                foreach (var b in behaviours)
                    if (!b.enabled)
                        return false;

                return true;
            }
            set
            {
                foreach (var b in behaviours) b.enabled = value;
            }
        }

        public void UpdateSharedVariables(BehavioursContainer container)
        {
            //TODO:
            //_sharedVariables = container;
            UpdateSharedVariables();
        }

        [Button]
        public void UpdateSharedVariables()
        {
            foreach (var b in behaviours)
            {
                b.UpdateSharedVariables(this);
            }
        }

        public IBehaviour Instantiate()
        {
            var copy = CreateInstance<BehavioursContainer>();

            copy._sharedVariables =
                new Dictionary<string, IValueHolder>(_sharedVariables.Count, _sharedVariables.Comparer);
            foreach (KeyValuePair<string, IValueHolder> entry in _sharedVariables)
            {
                copy._sharedVariables.Add(entry.Key, entry.Value.Instantiate());
            }

            copy.behaviours = new List<IBehaviour>(behaviours.Count);
            for (int i = 0; i < behaviours.Count; i++)
            {
                if (behaviours[i] != null)
                    copy.behaviours.Add(behaviours[i] == null ? null : behaviours[i].Instantiate());
            }

            copy.UpdateSharedVariables();

            return copy;
        }
    }
}