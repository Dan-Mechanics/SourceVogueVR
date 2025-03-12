using System.Collections.Generic;
using UnityEngine;

namespace VogueVR.Heartbeat
{
    /// <summary>
    /// Because order of execution and performance.
    /// References:
    /// https://github.com/vmuijrers/GitGud/blob/main/Assets/Scripts/GameManager.cs#L71
    /// https://github.com/vmuijrers/GitGud/blob/main/Assets/Scripts/ExampleProject/CustomMonoBehaviour.cs
    /// https://github.com/vmuijrers/GitGud/blob/main/Assets/Scripts/ExampleProject/UpdateManager.cs
    /// </summary>
    public class Heart : MonoBehaviour
    {
        [Tooltip("This happens before all other setups and in this order.")]
        [SerializeField] private MonoBehaviour[] prioritySetup = default;

        private static readonly List<ISetupable> setupables = new List<ISetupable>();
        private static readonly List<ITickable> tickables = new List<ITickable>();
        private static readonly List<ILateTickable> lateTickables = new List<ILateTickable>();
        private static readonly List<IFixedTickable> fixedTickables = new List<IFixedTickable>();

        /// <summary>
        /// Made it so that we don't 
        /// have to use FindObjectsOfType<>.
        /// </summary>
        private void Start()
        {
            for (int i = 0; i < this.prioritySetup.Length; i++)
            {
                if (!(this.prioritySetup[i] is ISetupable setupable))
                    continue;

                if (setupables.Contains(setupable))
                    setupables.Remove(setupable);

                setupable.DoSetup();
            }

            // Cannot use "this." because its static.
            setupables.ForEach(x => x.DoSetup());

            // Don't need em anymore.
            setupables.Clear();
        }

        private void Update()
        {
            tickables.ForEach(x => x.DoTick());
        }

        private void FixedUpdate()
        {
            fixedTickables.ForEach(x => x.DoFixedTick());
        }

        private void LateUpdate()
        {
            lateTickables.ForEach(x => x.DoLateTick());
        }

        /// <summary>
        /// Forget old stuff when moving into new scene.
        /// </summary>
        private void OnDestroy()
        {
            tickables.Clear();
            fixedTickables.Clear();
            setupables.Clear();
        }

        public static void RegisterSetup(ISetupable setupable) 
        {
            if (setupables.Contains(setupable))
                return;

            setupables.Add(setupable);
        }

        /// <summary>
        /// We are only allowed to register BaseBehaviour because
        /// those always deregister too.
        /// </summary>
        public static void Register(BaseBehaviour behaviour) 
        {
            if (behaviour is ITickable tickable && !tickables.Contains(tickable))
                tickables.Add(tickable);

            if (behaviour is IFixedTickable fixedTickable && !fixedTickables.Contains(fixedTickable))
                fixedTickables.Add(fixedTickable);

            if (behaviour is ILateTickable lateTickable && !lateTickables.Contains(lateTickable))
                lateTickables.Add(lateTickable);
        }

        public static void Deregister(BaseBehaviour behaviour)
        {
            if (behaviour is ITickable tickable)
                tickables.Remove(tickable);

            if (behaviour is IFixedTickable fixedTickable)
                fixedTickables.Remove(fixedTickable);

            if (behaviour is ILateTickable lateTickable)
                lateTickables.Remove(lateTickable);
        }
    }
}