using System;
using UnityEngine;

namespace TU.Unity.Identification
{
    [Serializable]
    public class AutoHashID : IHaveID
    {
        [SerializeField] private int id;
        [SerializeField] private string name;
        
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