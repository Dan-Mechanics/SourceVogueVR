using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VogueVR.Heartbeat;

namespace VogueVR.Gameplay
{
    /// <summary>
    /// Raises events from the timing of the beats and handles
    /// the contents of the beats.
    /// </summary>
    public class SongPlayer : SelfSubscriber, ISetupable, ITickable
    {
        public event EventHandler<OnBeatArgs> OnBeat;
        public event EventHandler<OnBeatArgs> OnGhostBeat;

        public class OnBeatArgs : EventArgs
        {
            public SongBeat songBeat;
            public float minDistForHit;
            public float leadTime;
            public int index;
            public bool hasBomb;

            public OnBeatArgs(SongBeat songBeat, float minDistForHit, float leadTime, int index, bool hasBomb)
            {
                this.songBeat = songBeat;
                this.minDistForHit = minDistForHit;
                this.leadTime = leadTime;
                this.index = index;
                this.hasBomb = hasBomb;
            }
        }

        [Header("References")]

        [SerializeField] private AudioSource source = default;
        [SerializeField] private Song song = default;

        [Header("Settings")]

        [SerializeField] private float minDistForHit = default;
        [SerializeField] private float beatLeadTime = default;
        [SerializeField] [Range(0f, 1f)] private float bombPercentage = default;

        [Header("Events")]

        [SerializeField] private UnityEvent onPlay = default;
        [SerializeField] private UnityEvent onStop = default;

        private SongBeat[] songBeats;
        private readonly List<int> bombBeatIndexes = new List<int>();

        private float startTime;
        private float stopTime;
        /*private int beatIndex;
        private int ghostBeatIndex;*/

        private BeatTrack mainTrack;
        private BeatTrack anticipationTrack;

        public void DoSetup()
        {
            // We want to start as stopped.
            Stop();

            this.songBeats = this.song.ConvertToSongBeats();

            if (this.songBeats == null || this.songBeats.Length <= 0)
            {
                Debug.LogWarning("no beats to play !!");
                Destroy(this.gameObject);

                return;
            }

            mainTrack = new BeatTrack(0, 0f);
            anticipationTrack = new BeatTrack(0, beatLeadTime);
        }

        public void DoTick()
        {
            if (Time.time >= stopTime) 
            {
                Stop();

                return;
            }

            if (this.anticipationTrack.CheckIfBeatIsNow(this.songBeats, this.startTime))
                SpawnAnticipationBeat();

            if (this.mainTrack.CheckIfBeatIsNow(this.songBeats, this.startTime))
                SpawnBeat();
        }

        private void SpawnAnticipationBeat()
        {
            if (this.bombPercentage >= UnityEngine.Random.value)
                this.bombBeatIndexes.Add(this.anticipationTrack.index);

            this.OnGhostBeat?.Invoke(this, new OnBeatArgs(this.songBeats[this.anticipationTrack.index], this.minDistForHit,
                this.beatLeadTime, this.anticipationTrack.index, this.bombBeatIndexes.Contains(this.anticipationTrack.index)));

            this.anticipationTrack.index++;
        }

        public void SpawnBeat()
        {
            this.OnBeat.Invoke(this, new OnBeatArgs(this.songBeats[this.mainTrack.index], this.minDistForHit,
                this.beatLeadTime, this.mainTrack.index, this.bombBeatIndexes.Contains(this.mainTrack.index)));

            this.mainTrack.index++;
        }

        public void Play()
        {
            this.bombBeatIndexes.Clear();
            this.mainTrack.index = 0;
            this.anticipationTrack.index = 0;

            StopCoroutine(PlayCoroutine());
            StartCoroutine(PlayCoroutine());
        }

        private IEnumerator PlayCoroutine()
        {
            if (this.beatLeadTime > this.songBeats[this.mainTrack.index].time)
            {
                float waitForBeatLeadTime = this.beatLeadTime - this.songBeats[0].time;

                SpawnAnticipationBeat();

                yield return new WaitForSeconds(waitForBeatLeadTime);
            }

            this.startTime = Time.time;
            this.stopTime = startTime + this.song.clip.length;

            this.source.PlayOneShot(this.song.clip);
            this.onPlay?.Invoke();

            Heart.Register(this);

            print($"now playing {this.song.name}");
        }

        public void Stop()
        {
            StopCoroutine(PlayCoroutine());

            Heart.Deregister(this);

            this.onStop?.Invoke();
            this.source.Stop();
        }
    }
}