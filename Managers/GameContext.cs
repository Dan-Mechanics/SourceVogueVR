using System.Collections.Generic;
using UnityEngine;
using VogueVR.Composites;
using VogueVR.Gameplay;
using VogueVR.Heartbeat;

namespace VogueVR.Managers
{
    /// <summary>
    /// This cannot be abstract because its the actual context.
    /// Reference: https://github.com/vmuijrers/GitGud/blob/main/Assets/Scripts/ExampleProject/GameContext.cs
    /// </summary>
    public class GameContext : BaseBehaviour
    {
        [SerializeField] private SongPlayer songPlayer = default;
        [SerializeField] private BeatIndicationSpawner beatIndicationSpawner = default;
        [SerializeField] private Score score = default;
        [SerializeField] private RandomText randomText = default;
        [SerializeField] private EasyTextWriter scoreText = default;
        [SerializeField] private EasyTextWriter accentText = default;
        [SerializeField] private BeatHittingController[] controllers = default;

        private readonly Dictionary<BodyPart, BeatHittingController> bodyPartToControllers = new Dictionary<BodyPart, BeatHittingController>();

        public override void DoSetup()
        {
            if (controllers.Length < 2) 
            {
                Debug.LogError("please assign controllers !!!");
                return;
            }

            bodyPartToControllers.Add(BodyPart.LeftHand, controllers[0]);
            bodyPartToControllers.Add(BodyPart.RightHand, controllers[1]);

            this.songPlayer.AnticipationTrack.OnBeat += beatIndicationSpawner.Spawn;

            for (int i = 0; i < this.controllers.Length; i++)
            {
                this.controllers[i].OnModifyScore += this.score.ModifyScore;
                this.songPlayer.MainTrack.OnBeat += (BeatTrack.OnBeatArgs args) => { bodyPartToControllers[args.songBeat.bodyPart].CheckForBeatCollision(args); };
                this.beatIndicationSpawner.OnSpawn += (BeatIndicationDestroyEffect effect, BeatTrack.OnBeatArgs args) => { effect.Setup(args.index, bodyPartToControllers[args.songBeat.bodyPart]); };
            }

            this.score.OnScoreChanged += this.scoreText.Write;
            this.randomText.OnTextChanged += this.accentText.Write;
        }

        private void OnDestroy()
        {
            this.songPlayer.AnticipationTrack.OnBeat -= beatIndicationSpawner.Spawn;

            for (int i = 0; i < this.controllers.Length; i++)
            {
                this.controllers[i].OnModifyScore -= this.score.ModifyScore;
                this.songPlayer.MainTrack.OnBeat -= (BeatTrack.OnBeatArgs args) => { bodyPartToControllers[args.songBeat.bodyPart].CheckForBeatCollision(args); };
                this.beatIndicationSpawner.OnSpawn -= (BeatIndicationDestroyEffect effect, BeatTrack.OnBeatArgs args) => { effect.Setup(args.index, bodyPartToControllers[args.songBeat.bodyPart]); };
            }

            this.score.OnScoreChanged -= this.scoreText.Write;
            this.randomText.OnTextChanged -= this.accentText.Write;

            bodyPartToControllers.Clear();
        }
    }
}