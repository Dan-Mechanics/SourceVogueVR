using System.Collections.Generic;
using UnityEngine;

namespace VogueVR.Heartbeat
{
    /// <summary>
    /// Because Unity uses SendMessage for all the Update() and Start() calls,
    /// which is not ideal because it uses Reflection.
    /// Reference:
    /// https://github.com/vmuijrers/GitGud/blob/main/Assets/Scripts/ExampleProject/UpdateManager.cs
    /// </summary>
    public class Heart : MonoBehaviour
    {
        private static readonly Channel<ISetupable> setupables = new Channel<ISetupable>();
        private static readonly Channel<ITickable> tickables = new Channel<ITickable>();
        private static readonly Channel<ILateTickable> lateTickables = new Channel<ILateTickable>();
        private static readonly Channel<IFixedTickable> fixedTickables = new Channel<IFixedTickable>();

        private static bool setupCompleted;

        private void Start()
        {
            print(setupables.followers.Count);
            
            for (int i = setupables.followers.Count - 1; i >= 0; i--)
            {
                setupables.followers[i].DoSetup();
            }
            
            setupables.Clear();
            setupCompleted = true;
        }

        /// <summary>
        /// It's reverse because elements can be removed.
        /// </summary>
        private void Update()
        {
            for (int i = tickables.followers.Count - 1; i >= 0; i--)
            {
                tickables.followers[i].DoTick();
            }
        }

        private void LateUpdate()
        {
            for (int i = lateTickables.followers.Count - 1; i >= 0; i--)
            {
                lateTickables.followers[i].DoLateTick();
            }
        }

        private void FixedUpdate()
        {
            for (int i = fixedTickables.followers.Count - 1; i >= 0; i--)
            {
                fixedTickables.followers[i].DoFixedTick();
            }
        }

        /// <summary>
        /// This is important because otherwise
        /// when switching scenes the static Channels
        /// persist.
        /// </summary>
        private void OnDestroy()
        {
            setupables.Clear();
            tickables.Clear();
            fixedTickables.Clear();

            setupCompleted = false;
        }

        public static void Subscribe(object follower)
        {
            if (follower is ITickable tickable)
                tickables.Subscribe(tickable);

            if (follower is IFixedTickable fixedTickable)
                fixedTickables.Subscribe(fixedTickable);

            if (follower is ILateTickable lateTickable)
                lateTickables.Subscribe(lateTickable);
        }

        public static void Unsubscribe(object follower)
        {
            if (follower is ITickable tickable)
                tickables.Unsubscribe(tickable);

            if (follower is IFixedTickable fixedTickable)
                fixedTickables.Unsubscribe(fixedTickable);

            if (follower is ILateTickable lateTickable)
                lateTickables.Unsubscribe(lateTickable);
        }

        /// <summary>
        /// Add and automatically sort setupable to a list.
        /// If a global priority sorted setup has already taken place,
        /// just call DoSetup() for setupable.
        /// </summary>
        public static void SubscribeSetupable(ISetupable setupable) 
        {
            if (setupCompleted)
            {
                setupable.DoSetup();
                return;
            }
            
            if (setupables.followers.Contains(setupable))
                return;

            for (int i = 0; i < setupables.followers.Count; i++)
            {
                if (setupable.SetupOrder < setupables.followers[i].SetupOrder) 
                {
                    setupables.followers.Insert(i, setupable);
                    return;
                }
            }

            setupables.followers.Add(setupable);
        }

        public class Channel<T> 
        {
            public readonly List<T> followers = new List<T>();

            public void Subscribe(T follower)
            {
                if (this.followers.Contains(follower))
                    return;

                this.followers.Add(follower);
            }

            /// <summary>
            /// We don't have to check if it's in there
            /// since it doesn't throw an error if it's not.
            /// </summary>
            public void Unsubscribe(T follower)
            {
                this.followers.Remove(follower);
            }

            public void Clear() => this.followers.Clear();
        }
    }
}