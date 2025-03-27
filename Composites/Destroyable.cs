using UnityEngine;

namespace VogueVR.Composites
{
    public class Destroyable : MonoBehaviour
    {
        public void Destroy() { Destroy(this.gameObject); }

        /// <summary>
        /// Best practice so use Timer.cs for this.
        /// </summary>
        public void DestroyAfterTime(float time) { Destroy(this.gameObject, time); }
    }
}