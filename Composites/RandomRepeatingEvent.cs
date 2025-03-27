using UnityEngine;
using UnityEngine.Events;
using VogueVR.Heartbeat;

namespace VogueVR.Composites
{
    public class RandomRepeatingEvent : SelfSubscriber, ISetupable, IFixedTickable
    {
        [SerializeField] [Min(0f)] private float minTimeBetweenHooks = default;
        [SerializeField] [Min(0.01f)] private float maxTimeBetweenHooks = default;
        [SerializeField] private UnityEvent onHook = default;

        private readonly Timer timer = new Timer();

        public void DoSetup()
        {
            Hook(); 
        }

        public void DoFixedTick()
        {
            if (this.timer.Tick(Time.fixedDeltaTime))
                Hook();
        }

        public void Hook()
        {
            this.timer.SetValue(Random.Range(this.minTimeBetweenHooks, this.maxTimeBetweenHooks));

            this.onHook?.Invoke();
        }
    }
}
