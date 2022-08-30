using System;
using System.Reflection;
using MyBox;
using UnityEngine;

/// <summary>
/// A faster way to run updates than MonoBevahiour.
/// From a test of 10,000 updates, this method ran in 0.0346ms whereas Update() ran in 1.4ms
/// Aside from speed, it makes scripts more managable and includes other helpful behaviours like Initialise()
/// </summary>
public class Updatable : MonoBehaviour
{
    
    private static readonly Type baseType = typeof(Updatable);

    [Foldout("Updatable"), SerializeField, ReadOnly]
    protected bool isInitialised = false;
    public bool IsInitialised => isInitialised;
    
    private bool isUpdateUsed;
    private bool isLateUpdateUsed;
    private bool isFixedUpdateUsed;

    protected virtual void Awake()
    {
        if (!isInitialised)
        {
            Initialise();
        }
    }
    
    protected virtual void Initialise()
    {
        isInitialised = true;
        
        Type finalType = GetType();
        isUpdateUsed = IsMethodUsed(finalType, "FastUpdate");
        isLateUpdateUsed = IsMethodUsed(finalType, "FastLateUpdate");
        isFixedUpdateUsed = IsMethodUsed(finalType, "FastFixedUpdate");
    }

    protected virtual void OnEnable()
    {
        ListenToUpdateManager();
    }

    protected virtual void OnDisable()
    {
        StopListeningToUpdateManager();
    }

    private void ListenToUpdateManager()
    {
        if (isUpdateUsed) {
            UpdateManager.Instance.OnUpdate += FastUpdate;
        }
        
        if (isLateUpdateUsed) {
            UpdateManager.Instance.OnLateUpdate += FastLateUpdate;
        }

        if (isFixedUpdateUsed) {
            UpdateManager.Instance.OnFixedUpdate += FastFixedUpdate;
        }
    }

    private void StopListeningToUpdateManager()
    {
        if (isUpdateUsed)
        {
            UpdateManager.Instance.OnUpdate -= FastUpdate;
        }

        if (isLateUpdateUsed)
        {
            UpdateManager.Instance.OnLateUpdate -= FastLateUpdate;
        }

        if (isFixedUpdateUsed)
        {
            UpdateManager.Instance.OnFixedUpdate -= FastFixedUpdate;
        }
    }
    
    private bool IsMethodUsed(Type type, string method) =>
        type.GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic)?.DeclaringType != baseType;

    protected virtual void FastUpdate()
    {
        
    }
    
    protected virtual void FastLateUpdate()
    {
        
    }
    
    protected virtual void FastFixedUpdate()
    {
        
    }
    
}
