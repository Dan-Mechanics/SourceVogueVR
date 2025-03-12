using UnityEngine;

namespace VogueVR.Gameplay
{
    //[System.Serializable]
    [CreateAssetMenu(fileName = "New Song", menuName = "ScriptableObjects/Song", order = 1)]
    public class Song : ScriptableObject
    {
        public AudioClip clip;
        public string json;

        public SongBeat[] ConvertToSongBeats() 
        {
            SongBeatSequence song = JsonUtility.FromJson<SongBeatSequence>(json);

            return song.beats;
        }
    }
}