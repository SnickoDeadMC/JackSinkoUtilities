using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using static MEC.Timing;
using System;

public static class CoroutineHelper
{

    public static void PerformAfterDelay(float delay, Action action) => RunCoroutine(WaitForSecondsIE(delay, action));
    private static IEnumerator<float> WaitForSecondsIE(float delay, Action action)
    {
        if (delay > 0)
        {
            yield return WaitForSeconds(delay);
        }
        
        action.Invoke();
    }
    
    public static void PerformAfterTrue(Func<bool> condition, Action action) => RunCoroutine(WaitUntilTrueIE(condition, action));
    private static IEnumerator<float> WaitUntilTrueIE(Func<bool> condition, Action action)
    {
        if (condition != null)
        {
            yield return WaitUntilTrue(condition);
        }
        
        action.Invoke();
    }
    
}
