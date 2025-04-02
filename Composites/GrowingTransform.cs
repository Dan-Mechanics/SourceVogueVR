using UnityEngine;
using VogueVR.Heartbeat;

namespace VogueVR.Composites
{
    public class GrowingTransform : BaseBehaviour, IFixedTickable
    {
        [SerializeField] private Transform growTarget = default;
        [SerializeField] private float growTargetDestroyLead = default;
        [SerializeField] private float highlightTime = default;

        private float startTime;
        private float growTime;
        private float startHighlightTime;

        public void Setup(float growTime, bool destroy = true)
        {
            this.startTime = Time.time;
            this.growTime = growTime;

            if (destroy)
                Destroy(this.growTarget.gameObject, this.growTime - this.growTargetDestroyLead);
        }

        public void DoFixedTick()
        {
            if (this.growTarget == null)
                return;
            
            float lerpValue = Time.time - this.startHighlightTime <= this.highlightTime ? 1f : (Time.time - this.startTime) / this.growTime;
            this.growTarget.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, lerpValue);
        }

        private void OnTriggerEnter(Collider other)
        {
            this.startHighlightTime = Time.time;
        }
    }
}