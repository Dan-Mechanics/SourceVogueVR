using UnityEngine;
using VogueVR.Heartbeat;

namespace VogueVR.Composites
{
    public class Velocity : SelfSubscriber, ISetupable, IFixedTickable
    {
        public float Speed => speed;

        private Vector3 prevPosition;
        private float speed;

        public void DoSetup()
        {
            this.prevPosition = this.transform.position;
        }

        public void DoFixedTick()
        {
            Sample();
        }
        
        private void Sample()
        {
            this.speed = Vector3.Distance(this.transform.position, this.prevPosition) / Time.fixedDeltaTime;

            this.prevPosition = this.transform.position;
        }
    }
}