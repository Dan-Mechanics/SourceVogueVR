using UnityEngine;
using UnityEngine.Events;

namespace VogueVR.Composite
{
    public class Timer : MonoBehaviour
    {
        public float TimeValue => timeValue;
        public bool TimerCompleted => timerCompleted;

        [SerializeField] private float timeValue = default;
        [SerializeField] private bool timerCompleted = default;

        [SerializeField] private UnityEvent onTimerComplete = default;

        private void FixedUpdate()
        {
            if (this.timerCompleted)
                return;

            this.timeValue -= Time.fixedDeltaTime;

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