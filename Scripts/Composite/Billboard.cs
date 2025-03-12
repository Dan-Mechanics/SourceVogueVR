using UnityEngine;
using VogueVR.Heartbeat;

namespace VogueVR.Composite
{
    public class Billboard : BaseBehaviour, ITickable
    {
        [SerializeField] private Transform cam = default;

        public void DoTick()
        {
            this.transform.LookAt(this.cam.position, Vector3.up);
        }
    }
}