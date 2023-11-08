using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JacksUtils
{
    public static class LoggingTools
    {

        /// <summary>
        /// Splits a large stack trace across multiple debug messages.
        /// </summary>
        public static void LogLargeStackTrace(string fullStackTrace)
        {
#if !UNITY_EDITOR && DEVELOPMENT_BUILD
        const int maxStringSize = 1000;
        
        string lastStackSent = "";
        for (int count = 0; count < fullStackTrace.Length; count += maxStringSize)
        {
            int desiredLength = count + maxStringSize;
            if (desiredLength > (fullStackTrace.Length - 1))
                desiredLength = fullStackTrace.Length-1;
                
            string substringed = fullStackTrace.Substring(0, desiredLength);
            string final = substringed;
            if (!lastStackSent.Equals(""))
            {
                final = substringed.Replace(lastStackSent, ""); //remove already sent stack
            }
            lastStackSent = substringed;

            LogWithoutStack(LogType.Warning, "Stack trace has been split. Showing " + count + " through to " + desiredLength + ".");
            LogWithoutStack(LogType.Log, final);
        }
#endif
        }

        public static void LogWithoutStack(LogType type, string message) =>
            Debug.LogFormat(type, LogOption.NoStacktrace, null, message);

    }
}