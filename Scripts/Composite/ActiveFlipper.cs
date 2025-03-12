using UnityEngine;

namespace VogueVR.Composite
{
    public class ActiveFlipper : MonoBehaviour
    {
        public void Flip() 
        {
            this.gameObject.SetActive(!this.gameObject.activeSelf);
        }
    }
}