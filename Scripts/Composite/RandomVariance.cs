using UnityEngine;

namespace VogueVR.Composite
{
    public class RandomVariance : MonoBehaviour
    {
        [SerializeField] private float pos = default;
        [SerializeField] private float rot = default;
        [SerializeField] private float scale = default;
        [SerializeField] private bool flatten = default;

        private void Start()
        {
            Vector3 insideUnitSphere = Random.insideUnitSphere;

            if (this.flatten) 
            {
                insideUnitSphere.y /= 3f;

                this.transform.Rotate(Vector3.up * Random.Range(-30f, 30f), Space.World);
            }
            
            this.transform.localPosition += insideUnitSphere * this.pos;
            this.transform.localScale *= Random.Range(1f - this.scale, 1f + this.scale);
            this.transform.localEulerAngles += Random.insideUnitSphere * this.rot;
        }
    }
}