using UnityEngine;
using UnityEngine.Events;
using VogueVR.Heartbeat;

namespace VogueVR.Composites
{
    public class TimerBehaviour : BaseBehaviour, IFixedTickable
    {
        [SerializeField] private UnityEvent onTimerComplete = default;

        private readonly Timer timer = new Timer();
        
        public void DoFixedTick()
        {
            if (this.timer.Tick(Time.fixedDeltaTime))
            {
                this.onTimerComplete?.Invoke();

                Heart.Unsubscribe(this);
            }
        }

        public void SetTimer(float value) 
        {
            this.timer.SetValue(value);
            this.gameObject.SetActive(true);

            Heart.Subscribe(this);
        }
    }
}