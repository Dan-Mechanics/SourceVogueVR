using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VogueVR.Heartbeat
{
    /// <summary>
    /// Because order of execution and performance.
    /// Sources:
    /// https://github.com/vmuijrers/GitGud/blob/main/Assets/Scripts/GameManager.cs#L71
    /// https://github.com/vmuijrers/GitGud/blob/main/Assets/Scripts/ExampleProject/CustomMonoBehaviour.cs
    /// https://github.com/vmuijrers/GitGud/blob/main/Assets/Scripts/ExampleProject/UpdateManager.cs
    /// 
    /// We dont need tick order of execution here because of GameContext.cs
    /// In the future we could have somewith where we have a priority float or int
    /// that is then used to sort the stuff.
    /// </summary>
    public class Heart : MonoBehaviour
    {
        [Tooltip("If you have GameInitiator.cs then you do not need this" +
            " because the setup sequence will be fully there. This should be empty" +
            " if there are no things that need to happen in order.")]
        [SerializeField] private MonoBehaviour[] prioritySetup = default;

        /// <summary>
        /// I dont see a problem where the setupable object is deleted but this is still here since its
        /// only used int he start basically. It becomes a null reference which is intended behaviour.
        /// </summary>
        private static readonly List<ISetupable> setupables = new List<ISetupable>();
        private static readonly List<ITickable> tickables = new List<ITickable>();
        private static readonly List<ILateTickable> lateTickables = new List<ILateTickable>();
        private static readonly List<IFixedTickable> fixedTickables = new List<IFixedTickable>();
        
        private void Start()
        {
            for (int i = 0; i < this.prioritySetup.Length; i++)
            {
                // and also not null ???
                if (this.prioritySetup[i] is ISetupable setupable)
                {
                    if (setupables.Contains(setupable))
                        setupables.Remove(setupable);

                    setupable.DoSetup();
                }
            }

            // Cannot use "this." because its static.
            setupables.ForEach(x => x.DoSetup());

            // dont need em anymore.
            setupables.Clear();
        }

        private void Update()
        {
            tickables.ForEach(x => x.DoTick());

            // ???
            // lateTickables.ForEach(x => x.LateTick());
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

            // ??
            setupables.Clear();
        }

        public static void RegisterSetup(ISetupable setupable) 
        {
            if (setupables.Contains(setupable))
                return;

            setupables.Add(setupable);
        }

        /// <summary>
        /// we are only alloed to register BaseBehaviour because
        /// those always deregister too.
        /// 
        /// Is there a way to make this cleaner with generics?
        /// </summary>
        public static void Register(BaseBehaviour behaviour) 
        {
            // it cant be null !!
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