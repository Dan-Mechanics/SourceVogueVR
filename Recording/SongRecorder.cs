using UnityEngine;
using VogueVR.FSM;
using VogueVR.Heartbeat;

namespace VogueVR.Recording
{
    public class SongRecorder : BaseBehaviour
    {
        [SerializeField] private RecordingOpenState recordingOpenState = default;
        [SerializeField] private RecordingClosedState recordingClosedState = default;

        private FiniteStateMachine machine;

        public override void DoSetup()
        {
            this.machine = new FiniteStateMachine(this.recordingClosedState);

            this.machine.states.Add(typeof(RecordingOpenState), this.recordingOpenState);
            this.machine.states.Add(typeof(RecordingClosedState), this.recordingClosedState);

            Heart.Subscribe(this.machine);
        }

        private void OnDestroy()
        {
            Heart.Unsubscribe(this.machine);
        }
    }
}