using System;
using UnityEngine;
using VogueVR.Heartbeat;

namespace VogueVR.Composite
{
    public class RandomText : BaseBehaviour, ISetupable
    {
        public Action<string> OnTextChanged;

        [SerializeField] private bool onStart = default;
        [SerializeField] private string[] strings = default;

        public void DoSetup()
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