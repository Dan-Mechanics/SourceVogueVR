using UnityEngine;
using VogueVR.Heartbeat;

namespace VogueVR.Composites
{
    public class Billboard : BaseBehaviour, ITickable
    {
        [SerializeField] private Transform cam = default;

        protected override void Awake()
        {
            if (this.cam == null)
                this.cam = GameObject.FindWithTag("MainCamera").transform;

            base.Awake();
        }

        public void DoTick()
        {
            this.transform.LookAt(this.cam.position, Vector3.up);
        }
    }
}