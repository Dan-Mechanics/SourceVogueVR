using UnityEngine;
using UnityEngine.Events;
using VogueVR.Heartbeat;

namespace VogueVR.Composites
{
    public class TimerBehaviour : SelfSubscriber, IFixedTickable
    {
        [SerializeField] private UnityEvent onTimerComplete = default;

        private readonly Timer timer = new Timer();
        
        public void DoFixedTick()
        {
            if (this.timer.Tick(Time.fixedDeltaTime))
            {
                this.onTimerComplete?.Invoke();

                Heart.Deregister(this);
            }
        }

        public void SetTimer(float value) 
        {
            this.timer.SetValue(value);
            this.gameObject.SetActive(true);

            Heart.Register(this);
        }
    }
}