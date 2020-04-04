using System;

namespace UXK.HealthSystem
{
    [Serializable]
    public struct HealthChangeInfo
    {
        public float      amount;
        public HealthChangeType type;

        public HealthChangeInfo(float amount, HealthChangeType type)
        {
            this.amount = amount;
            this.type = type;
        }
    }
}