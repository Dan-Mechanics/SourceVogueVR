using UnityEngine;
using UnityEngine.Events;
using VogueVR.Heartbeat;

namespace VogueVR.Composite
{
    public class Timer : BaseBehaviour, ITickable
    {
        public float TimeValue => timeValue;
        public bool TimerCompleted => timerCompleted;

        [SerializeField] private float timeValue = default;
        [SerializeField] private bool timerCompleted = default;

        [SerializeField] private UnityEvent onTimerComplete = default;

        public void DoTick()
        {
            // Or we deregister this here instead of bools.
            if (this.timerCompleted)
                return;

            this.timeValue -= Time.deltaTime;

            if (this.timeValue <= 0f)
            {
                this.timeValue = 0f;
                this.onTimerComplete?.Invoke();
                this.timerCompleted = true;
            }
        }

        public void SetTimer(float timeValue) 
        {
            this.timeValue = timeValue;

            this.timerCompleted = false;
            this.gameObject.SetActive(true);
        }
    }
}