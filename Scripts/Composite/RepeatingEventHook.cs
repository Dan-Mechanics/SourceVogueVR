using UnityEngine;
using UnityEngine.Events;
using VogueVR.Heartbeat;

namespace VogueVR.Composite
{
    public class RepeatingEventHook : BaseBehaviour, ISetupable
    {
        [SerializeField] [Min(0.01f)] private float interval = default;
        [SerializeField] private bool fromStart = default;
        [SerializeField] private UnityEvent onHook = default;

        public void DoSetup()
        {
            if (!fromStart)
                return;
            
            InvokeRepeating(nameof(Tick), 0f, this.interval);
        }

        public void StartAfterTime(float time) 
        {
            CancelInvoke(nameof(Tick));
            InvokeRepeating(nameof(Tick), time, this.interval);
        }

        private void Tick()
        {
            this.onHook?.Invoke();
        }
    }
}