using UnityEngine;
using UnityEngine.Events;
using VogueVR.Heartbeat;

namespace VogueVR.Composites
{
    /// <summary>
    /// AKA sequential event handler.
    /// </summary>
    public class StageHandler : BaseBehaviour
    {
        [SerializeField] private int stage = default;
        [SerializeField] private bool loadStageInStart = default;
        [SerializeField] private UnityEvent[] events = default;

        private bool verified;

        public override void DoSetup()
        {
            if (this.events == null || this.events.Length <= 0) 
            {
                Destroy(this.gameObject);
                Debug.LogWarning("No events!");

                return;
            }

            if (this.loadStageInStart)
            {
                this.stage = Mathf.Clamp(this.stage, 0, this.events.Length - 1);
                this.events[this.stage]?.Invoke();
            }

            this.verified = true;
        }

        public void MoveStage(int movement) 
        {
            if (!this.verified)
                return;

            if (movement == 0)
                return;

            int repeatCount = movement % this.events.Length;
            movement -= repeatCount * this.events.Length;

            this.stage += movement;

            if (this.stage < 0) 
            {
                this.stage += this.events.Length;
            }
            else if (this.stage > this.events.Length - 1) 
            {
                this.stage -= this.events.Length;
            }

            this.events[this.stage]?.Invoke();
        }
    }
}