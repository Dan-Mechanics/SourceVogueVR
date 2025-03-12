using UnityEngine;

namespace VogueVR.Gameplay
{
    public class BeatIndication : MonoBehaviour
    {
        [SerializeField] private Transform growTarget = default;
        [SerializeField] private float growTargetDestroyLead = default;

        private float startTime;
        private float destroyTime;

        public void Play(float startTime, float destroyTime)
        {
            this.startTime = startTime;
            this.destroyTime = destroyTime;
            
            Destroy(this.growTarget.gameObject, this.destroyTime - this.growTargetDestroyLead);
            Destroy(this.gameObject, this.destroyTime);
        }

        private void FixedUpdate()
        {
            if (this.growTarget == null)
                return;

            this.growTarget.localScale = Vector3.Lerp(Vector3.zero, Vector3.one,
                (Time.time - this.startTime) / this.destroyTime);
        }
    }
}