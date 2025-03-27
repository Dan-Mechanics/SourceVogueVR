using System;
using UnityEngine;

namespace VogueVR.Gameplay
{
    /// <summary>
    /// Handles score value.
    /// </summary>
    public class Score : MonoBehaviour
    {
        public Action<float> OnScoreChanged;

        private bool canGainScore;
        private float score;

        public void ModifyScore(float scoreGained)
        {
            if (!this.canGainScore)
                return;

            this.score += scoreGained;
            this.OnScoreChanged?.Invoke(this.score);
        }

        public void ResetScore()
        {
            this.canGainScore = true;
            this.score = 0f;

            this.OnScoreChanged?.Invoke(this.score);
        }

        /// <summary>
        /// This is so that we dont gain score when the game has ended.
        /// </summary>
        public void Pause() 
        {
            this.canGainScore = false;
        }
    }
}