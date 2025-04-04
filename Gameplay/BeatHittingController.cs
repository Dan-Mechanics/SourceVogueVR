using System;
using UnityEngine;
using UnityEngine.Events;
using VogueVR.Composites;

namespace VogueVR.Gameplay
{
    [RequireComponent(typeof(DistanceTravelled))]
    public class BeatHittingController : MonoBehaviour
    {
        public event Action<float> OnModifyScore;
        public event Action<OnDestroyBeatIndicationArgs> OnDestroyBeatIndication;

        public struct OnDestroyBeatIndicationArgs
        {
            public float scoreGained;
            public int index;

            public OnDestroyBeatIndicationArgs(float scoreGained, int index)
            {
                this.scoreGained = scoreGained;
                this.index = index;
            }
        }

        [SerializeField] private DistanceTravelled distanceTravelled = default;
        [SerializeField] private UnityEvent onHit = default;
        [SerializeField] private UnityEvent onMiss = default;

        public void CheckForBeatCollision(BeatTrack.OnBeatArgs args)
        {
            float scoreGained = 0f;

            if (CheckDistance(args))
            {
                scoreGained = this.distanceTravelled.Distance;

                if (args.hasBomb)
                    scoreGained *= -1f;

                print($"hit beat @ {args.songBeat.time}.");
                this.onHit?.Invoke();
            }
            else
            {
                print($"missed beat @ {args.songBeat.time}.");
                this.onMiss?.Invoke();
            }

            this.OnModifyScore?.Invoke(scoreGained);
            this.OnDestroyBeatIndication?.Invoke(new OnDestroyBeatIndicationArgs(scoreGained, args.index));
            this.distanceTravelled.ResetDistance();
        }

        private bool CheckDistance(BeatTrack.OnBeatArgs args)
        {
            return Vector3.Distance(this.transform.position, args.songBeat.pos) <= args.minDistForHit;
        }
    }
}