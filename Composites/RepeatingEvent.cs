using UnityEngine;
using UnityEngine.Events;
using VogueVR.Heartbeat;

namespace VogueVR.Composites
{
    public class RepeatingEvent : BaseBehaviour, IFixedTickable
    {
        [SerializeField] [Min(0.01f)] private float interval = default;
        [SerializeField] private bool fromStart = default;
        [SerializeField] private UnityEvent onHook = default;

        private readonly Timer timer = new Timer();

        public override void DoSetup()
        {
            if (this.interval <= 0f) 
            {
                Debug.LogWarning("if (interval <= 0f) !!");
                this.interval = Time.fixedDeltaTime;
            }

            if (this.fromStart)
                Hook();
        }

        public void DoFixedTick()
        {
            if (this.timer.Tick(Time.fixedDeltaTime))
                Hook();
        }

        public void StartAfterTime(float time) 
        {
            this.timer.SetValue(time);
        }

        public void Hook()
        {
            this.timer.SetValue(this.interval);
            this.onHook?.Invoke();
        }
    }
}