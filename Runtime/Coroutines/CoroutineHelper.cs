using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace JacksUtils
{
    public class CoroutineHelper : PersistentSingleton<CoroutineHelper>
    {
        
        public static event Action onUnityUpdate;
        public static event Action onUnityLateUpdate;
        public static event Action onUnityFixedUpdate;

        [RuntimeInitializeOnLoadMethod]
        private static void RuntimeInitialise()
        {
            onUnityUpdate = null;
            onUnityLateUpdate = null;
            onUnityFixedUpdate = null;
        }
        
        private void Update()
        {
            onUnityUpdate?.Invoke();
        }

        private void LateUpdate()
        {
            onUnityLateUpdate?.Invoke();
        }

        private void FixedUpdate()
        {
            onUnityFixedUpdate?.Invoke();
        }

        public static Coroutine PerformAtEndOfFrame(Action action, MonoBehaviour monoToRunOn = null)
        {
            if (action == null)
                return null;

            if (monoToRunOn == null)
                monoToRunOn = Instance;
            return monoToRunOn.StartCoroutine(PerformAtEndOfFrameIE(action));
        }

        public static Coroutine PerformAfterFixedUpdate(Action action, MonoBehaviour monoToRunOn = null)
        {
            if (action == null)
                return null;

            if (monoToRunOn == null)
                monoToRunOn = Instance;
            return monoToRunOn.StartCoroutine(PerformAfterFixedUpdateIE(action));
        }

        public static Coroutine PerformAfterTrue(Func<bool> condition, Action action, MonoBehaviour monoToRunOn = null)
        {
            if (condition == null || condition.Invoke())
            {
                //can perform instantly
                action?.Invoke();
                return null;
            }

            if (monoToRunOn == null)
                monoToRunOn = Instance;
            return monoToRunOn.StartCoroutine(PerformAfterTrueIE(condition, action));
        }

        public static Coroutine PerformAfterDelay(float delay, Action action, MonoBehaviour monoToRunOn = null)
        {
            if (delay <= 0)
            {
                //can perform instantly
                action?.Invoke();
                return null;
            }

            if (monoToRunOn == null)
                monoToRunOn = Instance;
            return monoToRunOn.StartCoroutine(PerformAfterDelayIE(delay, action));
        }

        private static IEnumerator PerformAtEndOfFrameIE(Action action)
        {
            yield return new WaitForEndOfFrame();
            action?.Invoke();
        }

        private static IEnumerator PerformAfterFixedUpdateIE(Action action)
        {
            yield return new WaitForFixedUpdate();
            action?.Invoke();
        }

        private static IEnumerator PerformAfterTrueIE(Func<bool> condition, Action action)
        {
            yield return new WaitUntil(condition);
            action?.Invoke();
        }

        private static IEnumerator PerformAfterDelayIE(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }

    }
}