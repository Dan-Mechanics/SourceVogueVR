using UnityEngine;

namespace VogueVR.Gameplay
{
    public class BeatIndicationSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject leftPrefab = default;
        [SerializeField] private GameObject rightPrefab = default;

        public void Spawn(SongBeat songBeat, float dist, float lead)
        {
            SetupIndication(Instantiate(songBeat.bodyPart == BodyPart.LeftHand ?
                this.leftPrefab : this.rightPrefab, songBeat.pos, Quaternion.identity),
                dist, lead);
        }

        private void SetupIndication(GameObject beat, float dist, float lead)
        {
            beat.transform.localScale = dist * 2f * Vector3.one;
            beat.GetComponent<BeatIndication>().Play(Time.time, lead);
        }
    }
}