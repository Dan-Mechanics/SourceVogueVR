namespace VogueVR.Composites
{
    public class Timer
    {
        private float value;

        public bool Tick(float interval) 
        {
            this.value -= interval;

            return this.value <= 0f;
        }

        public void SetValue(float value) 
        {
            this.value = value;
        }
    }
}