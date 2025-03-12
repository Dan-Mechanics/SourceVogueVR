namespace VogueVR.Gameplay
{
    /// <summary>
    /// We do it like this because json doesnt
    /// accept just SongBeat[].
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