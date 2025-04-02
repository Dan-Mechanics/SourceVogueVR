using UnityEngine;

namespace VogueVR.FSM
{
    public class StateBehaviour : MonoBehaviour, IStatable
    {
        protected FiniteStateMachine machine;
        
        public virtual void EnterState(FiniteStateMachine machine) 
        {
            this.machine = machine;
        }

        public virtual void DoTick() { }

        public virtual void DoFixedTick() { }

        public virtual void ExitState() { }
    }
}