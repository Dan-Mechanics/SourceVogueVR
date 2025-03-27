using UnityEngine;

namespace VogueVR.Gameplay
{
    [System.Serializable]
    public struct SongBeat 
    {
        public float time;
        public Vector3 pos;
        public BodyPart bodyPart;

        public SongBeat(float time, Vector3 pos, BodyPart bodyPart)
        {
            this.time = time;
            this.pos = pos;
            this.bodyPart = bodyPart;
        }

        public override string ToString()
        {
            return $"{this.time} {this.pos} {this.bodyPart}";
        }
    }
}