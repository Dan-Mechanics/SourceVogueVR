using System;
using System.Collections.Generic;
using UnityEngine;
using VogueVR.Composites;
using VogueVR.Heartbeat;

namespace VogueVR.Gameplay
{
    public class BeatIndicationSpawner : BaseBehaviour
    {
        public event Action<BeatIndicationDestroyEffect, BeatTrack.OnBeatArgs> OnSpawn;

        [SerializeField] private GameObject leftPrefab = default;
        [SerializeField] private GameObject rightPrefab = default;
        [SerializeField] private Material bombMaterial = default;

        private readonly Dictionary<BodyPart, GameObject> bodyPartToPrefab = new Dictionary<BodyPart, GameObject>();

        public override void DoSetup()
        {
            this.bodyPartToPrefab.Add(BodyPart.LeftHand, this.leftPrefab);
            this.bodyPartToPrefab.Add(BodyPart.RightHand, this.rightPrefab);
        }

        public void Spawn(BeatTrack.OnBeatArgs args)
        {
            if (!this.bodyPartToPrefab.ContainsKey(args.songBeat.bodyPart))
                return;
            
            GameObject beat = Instantiate(this.bodyPartToPrefab[args.songBeat.bodyPart], args.songBeat.pos, Quaternion.identity);

            beat.transform.localScale = args.minDistForHit * 2f * Vector3.one;
            beat.GetComponent<GrowingTransform>().Setup(args.leadTime);

            if (args.hasBomb)
                beat.GetComponent<MeshRenderer>().material = this.bombMaterial;

            Destroy(beat, args.leadTime + 0.1f);

            this.OnSpawn?.Invoke(beat.GetComponent<BeatIndicationDestroyEffect>(), args);
        }
    }
}