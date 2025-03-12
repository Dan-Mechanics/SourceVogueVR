using UnityEngine;
using UnityEngine.Events;

namespace VogueVR.Composite
{
    public class RandomRepeatingEvent : MonoBehaviour
    {
        [SerializeField] [Min(0f)] private float minTimeBetweenHooks = default;
        [SerializeField] [Min(0.02f)] private float maxTimeBetweenHooks = default;
        [SerializeField] private UnityEvent onHook = default;

        private void Start()
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
