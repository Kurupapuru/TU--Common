namespace TU.Unity.HealthSystem
{
    public struct DamagePack {
        public DamageType type;
        public float value;

        public DamagePack(float value, DamageType type = DamageType.Normal)
        {
            this.value = value;
            this.type = type;
        }
    }
}