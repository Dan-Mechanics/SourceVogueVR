using TMPro;
using UnityEngine;

namespace VogueVR.Gameplay
{
    /// <summary>
    /// Removes, and can leave a text popup behind.
    /// </summary>
    public class BeatIndicationDestroyEffect : MonoBehaviour
    {
        public BodyPart BodyPart => this.bodyPart;
        
        [SerializeField] private GameObject popupPrefab = default;
        [SerializeField] private float popupTime = default;

        private int index;
        private BodyPart bodyPart;

        /// <summary>
        /// You could almost give OnBeatArgs here.
        /// </summary>
        public void Setup(int index, BodyPart bodyPart)
        {
            this.index = index;
            this.bodyPart = bodyPart;
        }
        
        public void DestroyBeatIndication(object sender, BeatHittingController.OnDestroyBeatIndicationArgs args)
        {
            if (this.index != args.index)
                return;

            if (args.scoreGained > 0f)
            {
                GameObject popup = Instantiate(this.popupPrefab, this.transform.position, Quaternion.identity);
                TMP_Text text = popup.transform.GetChild(0).GetComponent<TMP_Text>();
                text.text = args.scoreGained.ToString();

                Destroy(popup, popupTime);
            }

            ((BeatHittingController)sender).OnDestroyBeatIndication -= DestroyBeatIndication;
            Destroy(this.gameObject);
        }
    }
}