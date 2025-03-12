using UnityEngine;

namespace VogueVR.Heartbeat
{
    /// <summary>
    /// AKA BloodVessel.cs because it talks with the heart.
    /// </summary>
    public abstract class BaseBehaviour : MonoBehaviour
    {
        protected virtual void Awake()
        {
            // Is this good ?
            if (!(this is ISetupable setupable))
                return;

            Heart.RegisterSetup(setupable);
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