using UnityEngine;
using VogueVR.Heartbeat;

namespace VogueVR.Composites
{
    public class QuitHandler : BaseBehaviour, ITickable
    {
        public void DoTick()
        {
            if (!Input.GetKeyDown(KeyCode.Escape))
                return;

            Quit();
        }

        public void Quit() => Application.Quit();
    }
}