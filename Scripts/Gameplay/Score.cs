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

        private bool isPaused = true;
        private float score;

        private void Refresh()
        {
            if (this.isPaused)
                return;

            this.OnScoreChanged?.Invoke(score);
        }

        public void AddScore(float amount)
        {
            if (amount <= 0)
                return;

            this.score += amount;

            Refresh();
        }

        public void ResetScore()
        {
            this.isPaused = false;
            this.score = 0f;

            Refresh();
        }

        public void Pause() 
        {
            this.isPaused = true;
        }
    }
}