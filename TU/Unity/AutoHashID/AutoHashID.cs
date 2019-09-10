using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public class AutoHashID : IHaveID
    {
        [SerializeField, HorizontalGroup("1", LabelWidth = 50)] private int id;
        [SerializeField, HorizontalGroup("1", LabelWidth = 50)] private string name;
        
        public int Id => id;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                UpdateId();
            }
        }
        
        private void UpdateId()
        {
            id = Animator.StringToHash(name);
        }
        
        
        
        //Constructors
        public AutoHashID() {}
        public AutoHashID(string name)
        {
            Name = name;
        }
    }
}