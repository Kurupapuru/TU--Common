namespace TU.Unity.HealthSystem
{
    public struct HealthInfluenceMessage
    {
        public int identificator;
        public bool canRevive;
        public bool procents;
        public float influence;

        public HealthInfluenceMessage(int identificator, bool canRevive, bool procents, float influence)
        {
            this.identificator = identificator;
            this.canRevive     = canRevive;
            this.procents      = procents;
            this.influence     = influence;
        }
    }
}