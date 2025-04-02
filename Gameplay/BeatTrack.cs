using System;
using System.Collections.Generic;
using UnityEngine;

namespace VogueVR.Gameplay
{
    public class BeatTrack
    {
        public int index;
        public float trackLeadSeconds;

        public event Action<OnBeatArgs> OnBeat;

        public struct OnBeatArgs
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

        public BeatTrack(int index, float trackLeadSeconds)
        {
            this.index = index;
            this.trackLeadSeconds = trackLeadSeconds;
        }

        public bool CheckIfBeatIsNow(SongBeat[] songBeats, float startTime)
        {
            return this.index < songBeats.Length &&
                Time.time - startTime >= songBeats[this.index].time - this.trackLeadSeconds;
        }

        public void GoNextBeat(SongBeat[] songBeats, float minDistForHit, List<int> bombBeatIndexes) 
        {
            this.OnBeat?.Invoke(new OnBeatArgs(songBeats[this.index], minDistForHit,
                this.trackLeadSeconds, this.index, bombBeatIndexes.Contains(this.index)));

            this.index++;
        }

        public void Reset() 
        {
            this.index = 0;
        }
    }
}