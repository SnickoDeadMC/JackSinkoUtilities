using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace JacksUtils
{
    [CreateAssetMenu(menuName = "Logging/Logger")]
    public class Logger : ScriptableObject
    {

        [SerializeField] private bool isEnabled = true;
        
        /// <summary>
        /// Shortcut to check if the logger exists before logging the message.
        /// </summary>
        [Conditional("ENABLE_LOGS")]
        public static void Log(Logger logger, string message)
        {
            if (logger == null)
                return;

            logger.Log(message);
        }

        [Conditional("ENABLE_LOGS")]
        public void Log(string message)
        {
            if (!isEnabled)
                return;

            try
            {
                if (MainThreadDispatcher.Instance.MainThread != Thread.CurrentThread)
                {
                    Debug.Log($"[async] {message}");
                    return;
                }
                
                Debug.Log($"[{name}] {message}");
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error while logging.\n{e.Message}\n{e.StackTrace}");
            }
        }

    }
}