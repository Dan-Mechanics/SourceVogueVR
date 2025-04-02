namespace VogueVR.Composites
{
    /// <summary>
    /// Can this be a struct?
    /// </summary>
    public class Timer
    {
        private float value;
        private bool hasBeenSet;

        public bool Tick(float interval) 
        {
            if (!this.hasBeenSet)
                return false;
            
            this.value -= interval;
            return this.value <= 0f;
        }

        public void SetValue(float value) 
        {
            this.value = value;
            this.hasBeenSet = true;
        }

        public void DisableUntilSet() 
        {
            this.hasBeenSet = false;
        }
    }
}