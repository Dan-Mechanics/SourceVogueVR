using TMPro;
using UnityEngine;
using VogueVR.Heartbeat;

namespace VogueVR.Composite
{
    [RequireComponent(typeof(TMP_Text))]
    public class EasyTextWriter : BaseBehaviour, ISetupable
    {
        private TMP_Text text;
        
        public void DoSetup()
        {
            this.text = GetComponent<TMP_Text>();
        }

        public void Write(float value)
        {
            Write(value.ToString());
        }

        public void Write(object obj) 
        {
            Write(obj.ToString());
        }

        public void Write(string str) 
        {
            this.text.text = str;
        }
    }
}