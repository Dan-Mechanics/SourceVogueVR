using System;
using UnityEngine;
using VogueVR.Composites;

namespace VogueVR.Gameplay
{
    public class BeatIndicationSpawner : MonoBehaviour
    {
        public Action<BeatIndicationDestroyEffect> OnSpawn;

        [SerializeField] private GameObject leftPrefab = default;
        [SerializeField] private GameObject rightPrefab = default;
        [SerializeField] private Material bombMaterial = default;

        public void Spawn(object sender, SongPlayer.OnBeatArgs args)
        {
            GameObject beat = Instantiate(args.songBeat.bodyPart == BodyPart.LeftHand ? this.leftPrefab : this.rightPrefab, args.songBeat.pos, Quaternion.identity);

            beat.transform.localScale = args.minDistForHit * 2f * Vector3.one;
            beat.GetComponent<GrowingTransform>().Setup(args.leadTime);

            if (args.hasBomb)
                beat.GetComponent<MeshRenderer>().material = this.bombMaterial;

            BeatIndicationDestroyEffect remover = beat.GetComponent<BeatIndicationDestroyEffect>();
            remover.Setup(args.index, args.songBeat.bodyPart);

            this.OnSpawn?.Invoke(remover);
        }
    }
}