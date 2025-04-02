using UnityEngine;

namespace VogueVR.Heartbeat
{
    /// <summary>
    /// This class communes with the heart.
    /// Reference:
    /// https://github.com/vmuijrers/GitGud/blob/main/Assets/Scripts/ExampleProject/CustomMonoBehaviour.cs
    /// </summary>
    public abstract class BaseBehaviour : MonoBehaviour, ISetupable
    {
        [Header("Base Behaviour")]
        [SerializeField] protected int setupOrder = default;

        public int SetupOrder => this.setupOrder;

        protected virtual void Awake()
        {
            Heart.RegisterSetupable(this);
        }

        public virtual void DoSetup() { }

        protected virtual void OnEnable()
        {
            Heart.Subscribe(this);
        }

        protected virtual void OnDisable()
        {
            Heart.Unsubscribe(this);
        }
    }
}