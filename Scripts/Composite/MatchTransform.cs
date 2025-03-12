using UnityEngine;

namespace VogueVR.Composite
{
    public class MatchTransform : MonoBehaviour
    {
        [Header("References")]

        [SerializeField] private Transform model = default;

        [Header("Settings")]

        [SerializeField] private bool matchPos = default;
        [SerializeField] private bool matchRot = default;
        [SerializeField] private bool matchScale = default;

        private void Update()
        {
            if (this.matchPos)
                this.transform.position = this.model.position;

            if (this.matchRot)
                this.transform.rotation = this.model.rotation;

            if (this.matchScale)
                this.transform.localScale = this.model.localScale;
        }
    }
}