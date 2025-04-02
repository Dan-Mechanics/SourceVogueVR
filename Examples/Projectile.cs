using UnityEngine;
using VogueVR.Heartbeat;

namespace VogueVR.Examples
{
    public class Projectile : BaseBehaviour, ITickable, IFixedTickable
    {
        [SerializeField] private float speed = default;

        private Transform proj;

        public override void DoSetup()
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
            this.proj.Translate(this.speed * Time.fixedDeltaTime * this.proj.forward);
        }
    }
}