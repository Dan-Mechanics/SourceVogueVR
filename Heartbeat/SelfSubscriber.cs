using UnityEngine;

namespace VogueVR.Heartbeat
{
    /// <summary>
    /// This class communes with the heart.
    /// </summary>
    public abstract class SelfSubscriber : MonoBehaviour
    {
        [Header("Self Subscriber")]
        [SerializeField] private int setupPriority = default;
        
        protected virtual void Awake()
        {
            if (!(this is ISetupable setupable))
                return;

            Heart.RegisterSetupable(new Heart.Setupable(setupable, setupPriority));
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