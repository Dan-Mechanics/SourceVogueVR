using UnityEngine;
using UnityEngine.Events;

namespace VogueVR.Composite
{
    public class RepeatingEventHook : MonoBehaviour
    {
        public enum Startup
        { 
            Manual = 0,
            Start = 1, 
            Enable = 2 
        }
        
        [SerializeField] [Min(0.01f)] private float interval = default;

        /// <summary>
        /// Can this be remedied with Heart.cs??
        /// </summary>
        [SerializeField] private Startup startup = default;
        [SerializeField] private UnityEvent onHook = default;

        private void Start()
        {
            if (this.startup != Startup.Start)
                return;
            
            InvokeRepeating(nameof(Tick), 0f, this.interval);
        }

        public void StartAfterTime(float time) 
        {
            if (this.startup != Startup.Manual)
                return;

            InvokeRepeating(nameof(Tick), time, this.interval);
        }

        private void Tick()
        {
            this.onHook?.Invoke();
        }

        private void OnEnable()
        {
            if (this.startup != Startup.Enable)
                return;

            InvokeRepeating(nameof(Tick), 0f, this.interval);
        }

        private void OnDisable()
        {
            if (this.startup != Startup.Enable)
                return;

            CancelInvoke(nameof(Tick));
        }
    }
}