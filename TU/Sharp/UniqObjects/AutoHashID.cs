namespace ZR.Runtime.Utils
{
    using System;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [Serializable]
    public struct AutoHashID : IAutoHashID
    {
        [OnValueChanged("UpdateID")]
        [SerializeField, HorizontalGroup("AutoHashID", LabelWidth = 40)] private string name;
        [SerializeField, HorizontalGroup("AutoHashID", LabelWidth = 20)] private int id;

        public string Name {
            get => name;
            set {
                name = value;
                UpdateID();
            } 
        }

        public int ID => id;
        public bool HasValue => !String.IsNullOrEmpty(name);

        [Button]
        public void UpdateID() => id = Animator.StringToHash(name);

        public override bool Equals(object obj)
        {
            if (obj is IAutoHashID hashID) 
                return id == hashID.ID;
            else 
                return false;
        }
    }
}