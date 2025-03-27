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
    public class GameContext : SelfSubscriber, ISetupable
    {
        [SerializeField] private SongPlayer songPlayer = default;
        [SerializeField] private BeatIndicationSpawner beatIndicationSpawner = default;
        [SerializeField] private Score score = default;
        [SerializeField] private RandomText randomText = default;

        [SerializeField] private EasyTextWriter scoreText = default;
        [SerializeField] private EasyTextWriter accentText = default;
        [SerializeField] private BeatHittingController[] controllers = default;

        public void DoSetup()
        {
            this.songPlayer.OnGhostBeat += beatIndicationSpawner.Spawn;

            for (int i = 0; i < this.controllers.Length; i++)
            {
                this.controllers[i].OnModifyScore += this.score.ModifyScore;
                this.songPlayer.OnBeat += this.controllers[i].CheckHit;
                this.beatIndicationSpawner.OnSpawn += this.controllers[i].HookDestroy;
            }

            this.score.OnScoreChanged += this.scoreText.Write;
            this.randomText.OnTextChanged += this.accentText.Write;
        }

        private void OnDestroy()
        {
            this.songPlayer.OnGhostBeat -= this.beatIndicationSpawner.Spawn;

            for (int i = 0; i < this.controllers.Length; i++)
            {
                this.controllers[i].OnModifyScore -= this.score.ModifyScore;
                this.songPlayer.OnBeat -= this.controllers[i].CheckHit;
                this.beatIndicationSpawner.OnSpawn -= this.controllers[i].HookDestroy;
            }

            this.score.OnScoreChanged -= this.scoreText.Write;
            this.randomText.OnTextChanged -= this.accentText.Write;
        }
    }
}