using System;
using UnityEngine;

namespace UXK.HealthSystem
{
    [CreateAssetMenu(fileName = "HealthSystemSettings", menuName = "Config/Health System Settings")]
    public class HealthSystemSettings : ScriptableObject
    {
        #region Singletone
        public static HealthSystemSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<HealthSystemSettings>("Configs/HealthSystemSettings");
                    if (_instance == null)
                        throw new Exception("You need to create \"Config/Health System Settings\" asset in \"Resources/Configs\" folder");
                }

                return _instance;
            }
        }
        private static HealthSystemSettings _instance;
        #endregion
        
        public HealthChangeType[] canResurrectTypes;
    }
}