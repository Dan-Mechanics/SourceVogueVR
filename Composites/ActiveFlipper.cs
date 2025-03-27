using UnityEngine;

namespace VogueVR.Composites
{
    public class ActiveFlipper : MonoBehaviour
    {
        public void Flip() 
        {
            this.gameObject.SetActive(!this.gameObject.activeSelf);
        }
    }
}