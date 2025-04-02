using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using VogueVR.FSM;

namespace VogueVR.Recording
{
    public class RecordingClosedState : StateBehaviour
    {
        [SerializeField] private UnityEvent onClickSound = default;
        [SerializeField] private SteamVR_Behaviour_Boolean openRecording = default;

        public override void EnterState(FiniteStateMachine machine)
        {
            base.EnterState(machine);
            
            this.onClickSound?.Invoke();
            this.openRecording.onPressDownEvent += OpenRecording;
        }

        private void OpenRecording(SteamVR_Behaviour_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            this.machine.TransitionTo(typeof(RecordingOpenState));
        }

        public override void ExitState()
        {
            this.openRecording.onPressDownEvent -= OpenRecording;
        }
    }
}