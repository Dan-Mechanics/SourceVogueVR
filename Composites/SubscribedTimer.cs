using VogueVR.Heartbeat;
using UnityEngine;
using System;

namespace VogueVR.Composites
{
    public class SubscribedTimer : Timer, ITickable
    {
        public event Action OnComplete;

        public void DoTick()
        {
            if (!Tick(Time.deltaTime))
                return;

            Stop();
            OnComplete?.Invoke();
        }

        public override void SetValue(float value)
        {
            base.SetValue(value);
            Heart.Subscribe(this);
        }

        public void Stop() 
        {
            DisableUntilSet();
            Heart.Unsubscribe(this);
        }
    }
}