using UnityEngine;
using VogueVR.Composite;
using VogueVR.Gameplay;
using VogueVR.Heartbeat;

namespace VogueVR.Managers
{
    /// <summary>
    /// This cannot be abstract because its the actual context.
    /// Reference: https://github.com/vmuijrers/GitGud/blob/main/Assets/Scripts/ExampleProject/GameContext.cs
    /// </summary>
    public class GameContext : BaseBehaviour, ISetupable
    {
        [SerializeField] private SongPlayer songPlayer = default;
        [SerializeField] private BeatIndicationSpawner beatIndicationSpawner = default;

        [SerializeField] private Score score = default;
        [SerializeField] private RandomText randomText = default;

        [SerializeField] private EasyTextWriter scoreText = default;
        [SerializeField] private EasyTextWriter accentText = default;

        [SerializeField] private Velocity left = default;
        [SerializeField] private Velocity right = default;

        public void DoSetup()
        {
            this.songPlayer.OnSpawnBeatIndication += this.beatIndicationSpawner.Spawn;
            this.songPlayer.OnGainScore           += this.score.AddScore;
            this.score.OnScoreChanged             += this.scoreText.Write;
            this.randomText.OnTextChanged         += this.accentText.Write;
            this.songPlayer.OnBeat                += () => { this.songPlayer.ProcessBeat(this.left, this.right); };
        }

        private void OnDestroy()
        {
            this.songPlayer.OnSpawnBeatIndication -= this.beatIndicationSpawner.Spawn;
            this.songPlayer.OnGainScore           -= this.score.AddScore;
            this.score.OnScoreChanged             -= this.scoreText.Write;
            this.randomText.OnTextChanged         -= this.accentText.Write;
            this.songPlayer.OnBeat                -= () => { this.songPlayer.ProcessBeat(this.left, this.right); };
        }
    }
}