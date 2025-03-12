using UnityEngine;
using VogueVR.Heartbeat;

namespace VogueVR.Example
{
    public class Projectile : BaseBehaviour, ISetupable, IFixedTickable
    {
        [SerializeField] private float speed = default;

        private Transform proj;

        public void DoSetup()
        {
            this.proj = this.transform;
        }

        public void DoFixedTick()
        {
            this.proj.Translate(this.proj.forward * this.speed * Time.fixedDeltaTime);
        }
    }
}