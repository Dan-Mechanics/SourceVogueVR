using TMPro;
using UnityEngine;

namespace VogueVR.Gameplay
{
    /// <summary>
    /// Removes, and can leave a text popup behind.
    /// </summary>
    public class BeatIndicationDestroyEffect : MonoBehaviour
    {
        [SerializeField] private GameObject popupPrefab = default;
        [SerializeField] private float popupTime = default;

        private int index;
        private BeatHittingController controller;

        public void Setup(int index, BeatHittingController controller)
        {
            this.controller = controller;
            this.index = index;

            this.controller.OnDestroyBeatIndication += DestroyBeatIndication;
        }

        private void DestroyBeatIndication(BeatHittingController.OnDestroyBeatIndicationArgs args)
        {
            if (this.index != args.index)
                return;

            if (args.scoreGained > 0f)
                SpawnDestroyEffect(args);

            Destroy(this.gameObject);
        }

        private void SpawnDestroyEffect(BeatHittingController.OnDestroyBeatIndicationArgs args)
        {
            GameObject popup = Instantiate(this.popupPrefab, this.transform.position, Quaternion.identity);
            popup.GetComponentInChildren<TMP_Text>().text = args.scoreGained.ToString();

            Destroy(popup, this.popupTime);
        }

        private void OnDestroy()
        {
            if (this.controller == null)
                return;

            this.controller.OnDestroyBeatIndication -= DestroyBeatIndication;
        }
    }
}