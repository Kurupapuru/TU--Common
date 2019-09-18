namespace TU.Unity.HealthSystem
{
    public class ImmortalityMessage
    {
        public int   identificator;
        public float timeLength;

        public ImmortalityMessage(int identificator, float timeLength)
        {
            this.identificator = identificator;
            this.timeLength    = timeLength;
        }
    }
}