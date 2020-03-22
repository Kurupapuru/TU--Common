using System;
using NaughtyAttributes;
using TU.Sharp.Extensions;
using UnityEngine;

namespace TU.Unity.Enabable
{
    [Serializable]
    public class BehavioursManager
    { 
        public MonoBehaviour[] behaviours = new MonoBehaviour[0];
        
        public void EnableOnly(int id)
        {
            for (int i = 0; i < behaviours.Length; i++)
                behaviours[i].enabled = i == id;
        }

        public void SetEnabled(int id, bool enabled)
            => behaviours[id].enabled = enabled;

        [Button]
        private void EnableOnlyFirst()
        {
            behaviours.ForEach(x => x.enabled = false);
            behaviours[0].enabled = true;
        }
    }
}