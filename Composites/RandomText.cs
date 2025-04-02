using System;
using UnityEngine;
using VogueVR.Heartbeat;

namespace VogueVR.Composites
{
    public class RandomText : BaseBehaviour
    {
        public event Action<string> OnTextChanged;

        [SerializeField] private bool fromStart = default;
        [SerializeField] private string[] strings = default;

        public override void DoSetup()
        {
            if (!this.fromStart)
                return;

            Set();
        }

        public void Set() 
        {
            if (this.strings.Length <= 0)
                return;
            
            this.OnTextChanged?.Invoke(this.strings[UnityEngine.Random.Range(0, this.strings.Length)]);
        }
    }
}