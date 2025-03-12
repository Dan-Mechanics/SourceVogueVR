using UnityEngine;
using UnityEngine.Events;
using VogueVR.Heartbeat;

namespace VogueVR.Composite
{
    public class RandomRepeatingEvent : BaseBehaviour, ISetupable
    {
        [SerializeField] [Min(0f)] private float minTimeBetweenHooks = default;
        [SerializeField] [Min(0.02f)] private float maxTimeBetweenHooks = default;
        [SerializeField] private UnityEvent onHook = default;

        public void DoSetup()
        {
            Invoke(nameof(Hook), Random.Range(this.minTimeBetweenHooks, this.maxTimeBetweenHooks));
        }

        public void Hook() 
        {
            CancelInvoke(nameof(Hook));

            this.onHook?.Invoke();

            Invoke(nameof(Hook), Random.Range(this.minTimeBetweenHooks, this.maxTimeBetweenHooks));
        }
    }
}
