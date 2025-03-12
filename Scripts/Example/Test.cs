using VogueVR.Heartbeat;
using UnityEngine;

namespace VogueVR.Example
{
    public class Test : BaseBehaviour, ITickable
    {
        public void DoTick()
        {
            print($"it's now {Time.time}.");
        }
    }
}