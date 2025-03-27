using UnityEngine;
using VogueVR.Heartbeat;

namespace VogueVR.Composites
{
    [RequireComponent(typeof(TrailRenderer))]
    public class TrailRendColor : SelfSubscriber, ISetupable
    {
        [SerializeField] private Color color = default;

        public void DoSetup()
        {
            SetColor(this.color);
        }

        public void SetColor(Color color)
        {
            TrailRenderer rend = GetComponent<TrailRenderer>();

            rend.startColor = color;
            
            Color endColor = color;
            endColor.a = 0f;

            rend.endColor = endColor;
        }
    }
}