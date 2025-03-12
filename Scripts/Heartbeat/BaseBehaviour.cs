using UnityEngine;

namespace VogueVR.Heartbeat
{
    /// <summary>
    /// Aka Vein.cs because veins move blood to the Heart.cs.
    /// This is best used for something like a projectile.
    /// </summary>
    public abstract class BaseBehaviour : MonoBehaviour
    {
        /// <summary>
        /// I could make an inhertied thign from this but i like this better tbh.
        /// Then i would have to manually make SetupBehavour for all the stuff and thats not the vibe.
        /// </summary>
        protected virtual void Awake() 
        {
            // You know you are cooking when you are
            // using normal English to code *O* 
            if (this is ISetupable setupable) 
            {
                Heart.RegisterSetup(setupable);
            }
        }
        
        protected virtual void OnEnable()
        {
            Heart.Register(this);
        }

        protected virtual void OnDisable()
        {
            Heart.Deregister(this);
        }
    }
}