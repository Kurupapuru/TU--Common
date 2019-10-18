using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BehavioursManager
{
    public class BehavioursManager : SerializedMonoBehaviour
    {
        [SerializeField] private Dictionary<string, object> applyToSharedVariablesOnAwake;
        [SerializeField] private BehavioursContainer _behavioursContainer;

        public BehavioursContainer BehavioursContainer
        {
            get => _behavioursContainer;
            set => _behavioursContainer = value;
        }

        [SerializeField] private bool enableOnStart = true;

        private void Awake()
        {
            _behavioursContainer = (BehavioursContainer) _behavioursContainer.Instantiate();

            foreach (var keyValuePair in applyToSharedVariablesOnAwake)
            {
                _behavioursContainer.FindOrAddSharedVariable(keyValuePair.Key, keyValuePair.GetType())
                    .SetValue(keyValuePair.Value);
            }
            
            _behavioursContainer.UpdateSharedVariables();
        }

        private void Start()
        {
            BehavioursContainer.enabled = enableOnStart;
        }
    }
}