using TMPro;
using UnityEngine;
using VogueVR.Heartbeat;

namespace VogueVR.Composites
{
    [RequireComponent(typeof(TMP_Text))]
    public class EasyTextWriter : MonoBehaviour
    {
        [SerializeField] private TMP_Text text = default;

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