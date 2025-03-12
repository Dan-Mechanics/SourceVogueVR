using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VogueVR.Gameplay
{
    /// <summary>
    /// If we could implement a generic state machine code here that would be really epic.
    /// </summary>
    public class SongRecorder : MonoBehaviour
    {
        [Header("References")]

        [SerializeField] private Song song = default;
        [SerializeField] private AudioSource source = default;

        [SerializeField] private Transform leftHand = default;
        [SerializeField] private Transform rightHand = default;

        [Header("Events")]

        [SerializeField] private UnityEvent onClickSound = default;

        private float openRecordingTime;
        private bool isRecording;

        private readonly Queue<SongBeat> recording = new Queue<SongBeat>();

        /// <summary>
        /// Called by Valve VR input, with Unity Event.
        /// </summary>
        public void OpenRecording()
        {
            if (!this.gameObject.activeSelf)
                return;

            if (this.isRecording)
                return;

            this.source.PlayOneShot(this.song.clip);

            this.openRecordingTime = Time.time;

            this.recording.Clear();

            this.onClickSound?.Invoke();

            this.isRecording = true;
        }

        /// <summary>
        /// Called by Valve VR input, with Unity Event.
        /// </summary>
        public void CaptureBeat(int bodyPart) 
        {
            if (!this.isRecording)
                return;

            BodyPart part = (BodyPart)bodyPart;

            this.recording.Enqueue(new SongBeat(Time.time - this.openRecordingTime,
                part == BodyPart.LeftHand ? this.leftHand.position : this.rightHand.position, part));

            this.onClickSound?.Invoke();
        }

        /// <summary>
        /// Called by Valve VR input, with Unity Event.
        /// </summary>
        public void CloseRecording() 
        {
            if (!this.isRecording)
                return;

            this.isRecording = false;

            this.source.Stop();

            if (this.recording.Count <= 0)
            {
                Debug.LogWarning("Recoridng is empty!!");
                return;
            }

            SongBeatSequence songBeatSequence = new SongBeatSequence(this.recording.ToArray());
            this.song.json = JsonUtility.ToJson(songBeatSequence);

            this.onClickSound?.Invoke();
        }
    }
}