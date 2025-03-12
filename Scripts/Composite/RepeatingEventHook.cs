using UnityEngine;
using UnityEngine.Events;
using VogueVR.Heartbeat;

namespace VogueVR.Composite
{
    public class RepeatingEventHook : BaseBehaviour, ISetupable
    {
        /// <summary>
        /// This follows Unity convention.
        /// </summary>
        public enum Startup
        { 
            Manual = 0,
            Start = 1, 
            Enable = 2 
        }
        
        [SerializeField] [Min(0.01f)] private float interval = default;
        [SerializeField] private Startup startup = default;
        [SerializeField] private UnityEvent onHook = default;

        public void DoSetup()
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

        protected override void OnEnable()
        {
            base.OnEnable();
            
            if (this.startup != Startup.Enable)
                return;

            InvokeRepeating(nameof(Tick), 0f, this.interval);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            if (this.startup != Startup.Enable)
                return;

            CancelInvoke(nameof(Tick));
        }
    }
}