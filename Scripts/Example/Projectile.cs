using UnityEngine;
using VogueVR.Heartbeat;

namespace VogueVR.Example
{
    public class Projectile : BaseBehaviour, ISetupable, ITickable, IFixedTickable
    {
        [SerializeField] private float speed = default;

        private Transform proj;

        public void DoSetup()
        {
            this.proj = this.transform;
        }

        public void DoTick()
        {
            if (!Input.GetKey(KeyCode.Space))
                return;

            Destroy(this.gameObject);
        }

        public void DoFixedTick()
        {
            this.proj.Translate(this.proj.forward * this.speed * Time.fixedDeltaTime);
        }
    }
}