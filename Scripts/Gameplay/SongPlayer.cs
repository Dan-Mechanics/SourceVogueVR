using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using VogueVR.Heartbeat;

namespace VogueVR.Gameplay
{
    public class SongPlayer : BaseBehaviour, ISetupable, ITickable
    {
        public bool IsPlaying => isPlaying;
        public float BeatLeadTime => beatLeadTime;
        public float MinDistForHit => minDistForHit;

        public Action<SongBeat, float, float> OnSpawnBeatIndication;
        public Action<float> OnGainScore;
        public Action OnBeat;

        [Header("References")]

        [SerializeField] private AudioSource source = default;
        [SerializeField] private Song song = default;

        [Header("Settings")]
        
        [SerializeField] private float minDistForHit = default;
        [SerializeField] private float beatLeadTime = default;

        [Header("Events")]

        [SerializeField] private UnityEvent onPlay = default;
        [SerializeField] private UnityEvent onStop = default;
        [SerializeField] private UnityEvent onHit = default;
        [SerializeField] private UnityEvent onMiss = default;

        private SongBeat[] songBeats;

        private float startTime;
        private bool isPlaying;
        private int beatIndex;
        private int beatLeadIndex;

        public void DoSetup()
        {
            this.songBeats = this.song.ConvertToSongBeats();

            if (this.songBeats == null || this.songBeats.Length <= 0)
            {
                Debug.LogWarning("no beats to play");
                Destroy(this.gameObject);
            }
        }

        public void DoTick()
        {
            if (!this.isPlaying)
                return;
            
            if (this.beatLeadIndex < this.songBeats.Length)
                ProcessGhostBeats();

            if (this.beatIndex < this.songBeats.Length)
                ProcessBeat();
        }

        private void ProcessBeat()
        {
            if (Time.time - this.startTime >= this.songBeats[this.beatIndex].time)
                this.OnBeat?.Invoke();
        }

        private void ProcessGhostBeats()
        {
            if (Time.time - this.startTime >= this.songBeats[this.beatLeadIndex].time - this.beatLeadTime)
                SpawnNextGhostBeat();
        }

        private void SpawnNextGhostBeat()
        {
            this.OnSpawnBeatIndication?.Invoke(this.songBeats[beatLeadIndex], this.minDistForHit, this.beatLeadTime);

            this.beatLeadIndex++;
        }

        public void ProcessBeat(Velocity left, Velocity right)
        {
            if (CheckBeatHit(this.songBeats[this.beatIndex], left.transform, right.transform))
            {
                print($"hit beat @ {this.songBeats[this.beatIndex]}");

                this.onHit?.Invoke();

                this.OnGainScore?.Invoke(left.Speed + right.Speed);
            }
            else
            {
                print($"missed beat @ {this.songBeats[this.beatIndex]}");

                this.onMiss?.Invoke();
            }

            this.beatIndex++;
        }

        private bool CheckBeatHit(SongBeat beat, Transform left, Transform right)
        {
            return Vector3.Distance(beat.bodyPart == BodyPart.LeftHand ?
                left.position : right.position, beat.pos) <= this.minDistForHit;
        }

        public void Play()
        {
            if (!this.gameObject.activeSelf)
                return;

            if (this.isPlaying)
                return;

            this.isPlaying = true;

            this.startTime = Time.time;
            this.beatIndex = 0;
            this.beatLeadIndex = 0;

            StartCoroutine(PlayCoroutine());
        }

        private IEnumerator PlayCoroutine()
        {
            if (this.beatLeadTime > this.songBeats[beatIndex].time)
            {
                float extraTime = this.beatLeadTime - this.songBeats[0].time;
                this.startTime = Time.time + extraTime;

                SpawnNextGhostBeat();

                yield return new WaitForSeconds(extraTime);
            }

            this.source.PlayOneShot(this.song.clip);

            print($"now playing {this.song.name}");

            this.onPlay?.Invoke();

            Invoke(nameof(Stop), this.song.clip.length);
        }

        public void Stop()
        {
            if (!this.isPlaying)
                return;

            this.isPlaying = false;

            this.onStop?.Invoke();

            this.source.Stop();
        }
    }
}