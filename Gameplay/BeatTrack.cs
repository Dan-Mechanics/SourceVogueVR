using UnityEngine;

namespace VogueVR.Gameplay
{
    public class BeatTrack
    {
        public int index;
        public float lead;

        public BeatTrack(int index, float lead)
        {
            this.index = index;
            this.lead = lead;
        }

        public bool CheckIfBeatIsNow(SongBeat[] songBeats, float startTime)
        {
            return this.index < songBeats.Length &&
                Time.time - startTime >= songBeats[this.index].time - this.lead;
        }
    }
}