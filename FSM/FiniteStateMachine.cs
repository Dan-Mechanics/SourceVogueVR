using System;
using System.Collections.Generic;
using VogueVR.Heartbeat;

namespace VogueVR.FSM
{
    /// <summary>
    /// Rough implementation of a finite state machine.
    /// </summary>
    public class FiniteStateMachine : ITickable, IFixedTickable
    {
        public readonly Dictionary<Type, IStatable> states = new Dictionary<Type, IStatable>();
        private IStatable currentState;
        
        public FiniteStateMachine(IStatable startingState)
        {
            this.currentState = startingState;
            this.currentState.EnterState(this);
        }

        public void TransitionTo(Type type)
        {
            if (!this.states.ContainsKey(type))
                return;
            
            if (currentState != null)
                this.currentState.ExitState();

            this.currentState = this.states[type];
            this.states[type].EnterState(this);
        }

        public void DoTick()
        {
            if (this.currentState == null)
                return;

            this.currentState.DoTick();
        }

        public void DoFixedTick()
        {
            if (this.currentState == null)
                return;

            this.currentState.DoTick();
        }        
    }
}