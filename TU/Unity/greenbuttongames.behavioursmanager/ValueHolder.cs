using System;
using System.Collections.Generic;
using KurupapuruLab.SharedInterfaces;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Object = System.Object;

namespace BehavioursManager
{
    [Serializable]
    public class ValueHolder<T> : IValueHolder
    {
        public T Value;
        
        
        public ValueHolder(T value = default)
        {
            this.Value = value;
        }

        public void SetValue(object value)
        {
            Value = (T) value;
        }

        public TCastTo CastValueTo<TCastTo>()
        {
            return (TCastTo) (object)Value;
        }

        public IValueHolder Instantiate()
        {
            var copy = new ValueHolder<T>(Value);
            if (Value as ScriptableObject)
            {
                Debug.Log($"Original SO: {Value}, Copy SO: {copy.Value}");
            }

            return copy;
        }


        public TConvert ConvertedValue<TConvert>() where TConvert : T
        {
            return (TConvert) Value;
        }

        public static implicit operator T(ValueHolder<T> original)
        {
            return original.Value;
        }
    }

    public interface IValueHolder : IInstantiatable<IValueHolder>
    {
        void SetValue(object value);
        TCastTo CastValueTo<TCastTo>();
    }
}