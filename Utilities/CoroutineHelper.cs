using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using static MEC.Timing;
using System;

public static class CoroutineHelper
{

    public static void PerformAfterDelay(Action action, float delay) => RunCoroutine(WaitForSecondsIE(action, delay));
    private static IEnumerator<float> WaitForSecondsIE(Action action, float delay)
    {
        if (delay > 0)
        {
            yield return WaitForSeconds(delay);
        }
        
        action.Invoke();
    }
    
    public static void PerformAfterTrue(Action action, Func<bool> condition) => RunCoroutine(WaitUntilTrueIE(action, condition));
    private static IEnumerator<float> WaitUntilTrueIE(Action action, Func<bool> condition)
    {
        if (condition != null)
        {
            yield return WaitUntilTrue(condition);
        }
        
        action.Invoke();
    }
    
}
