using System.Collections.Generic;
using UnityEngine;
using VogueVR.Gameplay;
using VogueVR.FSM;
using UnityEngine.Events;
using Valve.VR;

namespace VogueVR.Recording
{
    public class RecordingOpenState : StateBehaviour
    {
        [SerializeField] private Song song = default;
        [SerializeField] private AudioSource source = default;
        [SerializeField] private Transform leftHand = default;
        [SerializeField] private Transform rightHand = default;
        [SerializeField] private UnityEvent onClickSound = default;

        [SerializeField] private SteamVR_Behaviour_Boolean closeRecording = default;
        [SerializeField] private SteamVR_Behaviour_Boolean leftCapture = default;
        [SerializeField] private SteamVR_Behaviour_Boolean rightCapture = default;

        private float openRecordingTime;
        private readonly Queue<SongBeat> recording = new Queue<SongBeat>();
        private readonly Dictionary<BodyPart, Transform> bodyPartToTransform = new Dictionary<BodyPart, Transform>();
        
        /// <summary>
        /// Open recoridng. Read.
        /// </summary>
        public override void EnterState(FiniteStateMachine machine)
        {
            base.EnterState(machine);
            
            if (this.bodyPartToTransform.Count <= 0)
            {
                this.bodyPartToTransform.Add(BodyPart.LeftHand, this.leftHand);
                this.bodyPartToTransform.Add(BodyPart.RightHand, this.rightHand);
            }

            this.source.PlayOneShot(this.song.clip);
            this.openRecordingTime = Time.time;
            this.recording.Clear();

            this.onClickSound?.Invoke();

            this.leftCapture.onPressDownEvent += CaptureLeftBeat;
            this.rightCapture.onPressDownEvent += CaptureRightBeat;
            this.closeRecording.onPressDownEvent += CloseRecording;
        }

        private void CloseRecording(SteamVR_Behaviour_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            machine.TransitionTo(typeof(RecordingClosedState));
        }

        private void CaptureLeftBeat(SteamVR_Behaviour_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            CaptureBeat(BodyPart.LeftHand);
        }

        private void CaptureRightBeat(SteamVR_Behaviour_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            CaptureBeat(BodyPart.RightHand);
        }

        private void CaptureBeat(BodyPart bodyPart)
        {
            this.recording.Enqueue(new SongBeat(
                Time.time - this.openRecordingTime,
                this.bodyPartToTransform[bodyPart].position, bodyPart));

            this.onClickSound?.Invoke();
        }

        /// <summary>
        /// Open recoridng. Write.
        /// </summary>
        public override void ExitState()
        {
            this.source.Stop();

            if (this.recording.Count <= 0)
            {
                Debug.LogWarning("recording is empty !!!");
                return;
            }

            SongBeatSequence songBeatSequence = new SongBeatSequence(this.recording.ToArray());
            this.song.json = JsonUtility.ToJson(songBeatSequence);

            this.leftCapture.onPressDownEvent -= CaptureLeftBeat;
            this.rightCapture.onPressDownEvent -= CaptureRightBeat;
            this.closeRecording.onPressDownEvent -= CloseRecording;
        }
    }
}