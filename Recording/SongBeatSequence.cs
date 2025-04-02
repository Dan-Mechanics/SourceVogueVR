using VogueVR.Gameplay;

namespace VogueVR.Recording
{
    /// <summary>
    /// We do it like this because JSON doesn't
    /// just accept just SongBeat[].
    /// </summary>
    [System.Serializable]
    public struct SongBeatSequence
    {
        public SongBeat[] beats;

        public SongBeatSequence(SongBeat[] beats)
        {
            this.beats = beats;
        }
    }
}