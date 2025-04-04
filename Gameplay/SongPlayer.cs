using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VogueVR.Composites;
using VogueVR.Heartbeat;
using VogueVR.Recording;

namespace VogueVR.Gameplay
{
    public class SongPlayer : BaseBehaviour, ITickable
    {
        public BeatTrack MainTrack { get; private set; }
        public BeatTrack AnticipationTrack { get; private set; }

        [Header("References")]

        [SerializeField] private AudioSource source = default;
        [SerializeField] private Song song = default;

        [Header("Settings")]

        [SerializeField] private float minDistForHit = default;
        [SerializeField] private float anticipationTrackLeadTime = default;
        [SerializeField] [Range(0f, 1f)] private float bombPercentage = default;

        [Header("Events")]

        [SerializeField] private UnityEvent onPlay = default;
        [SerializeField] private UnityEvent onStop = default;

        private SongBeat[] songBeats;
        private readonly List<int> bombBeatIndexes = new List<int>();
        private readonly SubscribedTimer subscribedTimer = new SubscribedTimer();
        private float startTime;
        private float stopTime;

        public override void DoSetup()
        {
            // Start as stopped.
            Stop();

            this.songBeats = this.song.ConvertToSongBeats();

            if (this.songBeats == null || this.songBeats.Length <= 0)
            {
                Debug.LogWarning("no beats to play !!");
                Destroy(this.gameObject);

                return;
            }

            this.MainTrack = new BeatTrack(0, 0f);
            this.AnticipationTrack = new BeatTrack(0, this.anticipationTrackLeadTime);
        }

        public void DoTick()
        {
            if (Time.time >= this.stopTime)
            {
                Stop();

                return;
            }

            if (this.AnticipationTrack.CheckIfBeatIsNow(this.songBeats, this.startTime))
                SpawnAnticipationBeat();

            if (this.MainTrack.CheckIfBeatIsNow(this.songBeats, this.startTime))
                this.MainTrack.GoNextBeat(this.songBeats, this.minDistForHit, this.bombBeatIndexes);
        }

        private void SpawnAnticipationBeat()
        {
            // The anticipation track makes the bombs.
            if (this.bombPercentage >= Random.value)
                this.bombBeatIndexes.Add(this.AnticipationTrack.index);

            this.AnticipationTrack.GoNextBeat(this.songBeats, this.minDistForHit, this.bombBeatIndexes);
        }

        /// <summary>
        /// Called via Unity Event.
        /// </summary>
        public void StartPlayingSong()
        {
            print($"waiting for {this.song.name} ...");

            this.bombBeatIndexes.Clear();
            this.MainTrack.Reset();
            this.AnticipationTrack.Reset();

            // This makes it even more random!
            Random.InitState(Mathf.RoundToInt(Time.time * 60f));

            if (this.anticipationTrackLeadTime > this.songBeats[0].time)
            {
                this.subscribedTimer.OnComplete += PlaySong;
                this.subscribedTimer.SetValue(this.anticipationTrackLeadTime - this.songBeats[0].time);

                SpawnAnticipationBeat();

                return;
            }

            PlaySong();
        }

        /// <summary>
        /// If this is called from SubscribedTimer, it has stopped itself,
        /// and if it's called directly from StartPlayingSong(), then it didnt start in the first place.
        /// </summary>
        private void PlaySong()
        {
            this.subscribedTimer.OnComplete -= PlaySong;

            this.startTime = Time.time;
            this.stopTime = this.startTime + this.song.clip.length;

            this.source.PlayOneShot(this.song.clip);
            this.onPlay?.Invoke();

            Heart.Subscribe(this);

            print($"now playing {this.song.name} !");
        }

        /// <summary>
        /// Called via Unity Event.
        /// </summary>
        public void Stop()
        {
            this.subscribedTimer.OnComplete -= PlaySong;
            this.subscribedTimer.Stop();

            Heart.Unsubscribe(this);

            this.onStop?.Invoke();
            this.source.Stop();

            print($"stopped {this.song.name} ...");
        }
    }
}
