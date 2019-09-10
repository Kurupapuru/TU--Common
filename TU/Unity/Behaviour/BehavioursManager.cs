using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace TU.Unity.Enabable
{
    public class BehavioursManager
    {
        public List<IBehaviour> enabables = new List<IBehaviour>();

        public void Initialize(MonoBehaviour user, bool disableAllOnDestroy)
        {
            if (disableAllOnDestroy)
            {
                
            }
        }
        
        public void EnableOnly(int id)
        {
            for (int i = 0; i < enabables.Count; i++)
                enabables[i].enabled = i == id;
        }

        public void SetEnabled(int id, bool enabled)
            => enabables[id].enabled = enabled;
    }
}