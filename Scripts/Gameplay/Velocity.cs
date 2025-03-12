using UnityEngine;
using VogueVR.Heartbeat;

namespace VogueVR.Gameplay
{
    public class Velocity : BaseBehaviour, ISetupable, IFixedTickable
    {
        public float Speed => speed;

        [SerializeField] private float sampleRate = default;

        private Vector3 velocity;
        private Vector3 prevPosition;
        private float speed;

        /// <summary>
        /// Bool is NOT scalable ~ Miku.
        /// </summary>
        private bool invoke;
        private float interval;

        public void DoSetup()
        {
            this.prevPosition = this.transform.position;

            this.invoke = this.sampleRate > 0f && 1f / this.sampleRate != Time.fixedDeltaTime;

            this.interval = this.invoke ? 1f / this.sampleRate : Time.fixedDeltaTime;

            if (this.invoke)
                InvokeRepeating(nameof(Sample), 0f, this.interval);
        }
        
        public void DoFixedTick()
        {
            if (this.invoke)
                return;

            Sample();
        }
        
        private void Sample()
        {
            this.velocity = (this.transform.position - this.prevPosition) / this.interval;
            this.speed = this.velocity.magnitude;

            this.prevPosition = this.transform.position;
        }
    }
}