using UnityEngine;

namespace VogueVR.Composite
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField] private Transform cam = default;

        private void Update()
        {
            this.transform.LookAt(this.cam.position, Vector3.up);
        }
    }
}