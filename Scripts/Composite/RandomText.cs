using System;
using TMPro;
using UnityEngine;

namespace VogueVR.Composite
{
    public class RandomText : MonoBehaviour
    {
        public Action<string> OnTextChanged;

        [SerializeField] private bool onStart = default;
        [SerializeField] private string[] strings = default;

        private void Start()
        {
            if (!this.onStart)
                return;

            Set();
        }

        public void Set() 
        {
            if (this.strings.Length <= 0)
                return;
            
            OnTextChanged?.Invoke(this.strings[UnityEngine.Random.Range(0, this.strings.Length)]);
        }
    }
}