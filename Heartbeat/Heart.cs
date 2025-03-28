using System.Collections.Generic;
using UnityEngine;

namespace VogueVR.Heartbeat
{
    /// <summary>
    /// Because Unity uses SendMessage for all the Update() and Start() calls,
    /// which is not ideal.
    /// References:
    /// https://github.com/vmuijrers/GitGud/blob/main/Assets/Scripts/GameManager.cs#L71
    /// https://github.com/vmuijrers/GitGud/blob/main/Assets/Scripts/ExampleProject/CustomMonoBehaviour.cs
    /// https://github.com/vmuijrers/GitGud/blob/main/Assets/Scripts/ExampleProject/UpdateManager.cs
    /// </summary>
    public class Heart : MonoBehaviour
    {
        private static readonly List<Setupable> setupables = new List<Setupable>();
        private static readonly List<ITickable> tickables = new List<ITickable>();
        private static readonly List<IFixedTickable> fixedTickables = new List<IFixedTickable>();

        private void Start()
        {
            setupables.ForEach(x => x.setupable1.DoSetup());
            setupables.Clear();
        }

        private void Update()
        {
            for (int i = tickables.Count - 1; i >= 0; i--)
            {
                tickables[i].DoTick();
            }
        }

        private void FixedUpdate()
        {
            for (int i = fixedTickables.Count - 1; i >= 0; i--)
            {
                fixedTickables[i].DoFixedTick();
            }
        }

        /// <summary>
        /// Forget old stuff when moving into new scene.
        /// </summary>
        private void OnDestroy()
        {
            setupables.Clear();
            tickables.Clear();
            fixedTickables.Clear();
        }

        /// <summary>
        /// We don't have to deregister setupable because it's setup.
        /// </summary>
        public static void RegisterSetupable(Setupable setupable) 
        {
            if (setupables.Contains(setupable))
                return;

            for (int i = 0; i < setupables.Count; i++)
            {
                if (setupable.priority < setupables[i].priority) 
                {
                    setupables.Insert(i, setupable);
                    return;
                }
            }

            setupables.Add(setupable);
        }

        public static void Register(SelfSubscriber selfSubscriber) 
        {
            if (selfSubscriber is ITickable tickable)
                RegisterTickable(tickable);

            if (selfSubscriber is IFixedTickable fixedTickable)
                RegisterFixedTickable(fixedTickable);
        }

        public static void Deregister(SelfSubscriber selfSubscriber)
        {
            if (selfSubscriber is ITickable tickable)
                DeregisterTickable(tickable);

            if (selfSubscriber is IFixedTickable fixedTickable)
                DeregisterFixedTickable(fixedTickable);
        }

        public static void RegisterTickable(ITickable tickable)
        {
            if (tickables.Contains(tickable))
                return;

            tickables.Add(tickable);
        }

        public static void DeregisterTickable(ITickable tickable)
        {
            tickables.Remove(tickable);
        }

        public static void RegisterFixedTickable(IFixedTickable fixedTickable)
        {
            if (fixedTickables.Contains(fixedTickable))
                return;

            fixedTickables.Add(fixedTickable);
        }

        public static void DeregisterFixedTickable(IFixedTickable fixedTickable)
        {
            fixedTickables.Remove(fixedTickable);
        }

        public struct Setupable 
        {
            public ISetupable setupable1;
            public int priority;

            public Setupable(ISetupable setupable1, int priority)
            {
                this.setupable1 = setupable1;
                this.priority = priority;
            }
        }
    }
}