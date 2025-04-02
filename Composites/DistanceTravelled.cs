using UnityEngine;
using VogueVR.Heartbeat;

namespace VogueVR.Composites
{
    public class DistanceTravelled : BaseBehaviour, IFixedTickable
    {
        public float Distance => this.distance;

        private Vector3 prevPosition;
        private float distance;

        public override void DoSetup()
        {
            this.prevPosition = this.transform.position;
        }

        public void DoFixedTick()
        {
            Sample();
        }
        
        private void Sample()
        {
            this.distance += Vector3.Distance(this.transform.position, this.prevPosition);
            this.prevPosition = this.transform.position;
        }

        public void ResetDistance() 
        {
            this.distance = 0f;
        }
    }
}