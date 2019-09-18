namespace TU.Unity.HealthSystem
{
    public struct HealPack {
        public HealType type;
        public float value;

        public HealPack(float value, HealType type = HealType.Normal)
        {
            this.value = value;
            this.type = type;
        }
    }
}