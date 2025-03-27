using System;
using UnityEngine;
using UnityEngine.Events;
using VogueVR.Composites;

namespace VogueVR.Gameplay
{
    [RequireComponent(typeof(DistanceTravelled))]
    public class BeatHittingController : MonoBehaviour
    {
        public Action<float> OnModifyScore;
        public event EventHandler<OnDestroyBeatIndicationArgs> OnDestroyBeatIndication;

        public class OnDestroyBeatIndicationArgs : EventArgs 
        {
            public float scoreGained;
            public int index;

            public OnDestroyBeatIndicationArgs(float scoreGained, int index)
            {
                this.scoreGained = scoreGained;
                this.index = index;
            }
        }

        [SerializeField] private BodyPart bodyPart = default;
        [SerializeField] private DistanceTravelled distanceTravelled = default;

        [SerializeField] private UnityEvent onHit = default;
        [SerializeField] private UnityEvent onMiss = default;

        public void CheckHit(object sender, SongPlayer.OnBeatArgs args)
        {
            if (args.songBeat.bodyPart != this.bodyPart)
                return;

            float scoreGained = 0f;

            if (CheckDistance(args))
            {
                scoreGained = this.distanceTravelled.Distance;

                if (args.hasBomb)
                    scoreGained *= -1f;

                print($"hit beat @ {args.songBeat.time}");
                this.onHit?.Invoke();
            }
            else
            {
                print($"missed beat @ {args.songBeat.time}");
                this.onMiss?.Invoke();
            }

            this.OnModifyScore?.Invoke(scoreGained);
            this.OnDestroyBeatIndication?.Invoke(this, new OnDestroyBeatIndicationArgs(scoreGained, args.index));
            this.distanceTravelled.ResetDistance();
        }

        private bool CheckDistance(SongPlayer.OnBeatArgs args)
        {
            return Vector3.Distance(this.transform.position, args.songBeat.pos) <= args.minDistForHit;
        }

        public void HookDestroy(BeatIndicationDestroyEffect effect)
        {
            if (this.bodyPart != effect.BodyPart)
                return;

            this.OnDestroyBeatIndication += effect.DestroyBeatIndication;
        }
    }
}