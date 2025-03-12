using UnityEngine;

namespace VogueVR.Gameplay
{
    [CreateAssetMenu(fileName = "New Song", menuName = "ScriptableObjects/Song", order = 1)]
    public class Song : ScriptableObject
    {
        public AudioClip clip;
        public string json;

        public SongBeat[] ConvertToSongBeats() 
        {
            if (string.IsNullOrEmpty(json) || string.IsNullOrWhiteSpace(json))
                return null;
            
            SongBeatSequence song = JsonUtility.FromJson<SongBeatSequence>(json);

            return song.beats;
        }
    }
}