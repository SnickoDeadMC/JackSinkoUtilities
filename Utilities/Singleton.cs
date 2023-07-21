using System;
using UnityEngine;
using MyBox;

/// <summary>
/// Generic singleton class.
/// </summary>
/// <remarks>You can add [ExecuteInEditMode] if you need the instance in edit mode (eg. for edit mode tests).</remarks>
public abstract class Singleton<T> : Updatable where T : MonoBehaviour
{

    private static T instance;
    public static T Instance
    {
        get
        {
            if (!Application.isPlaying)
            {
                if (instance == null)
                    instance = FindObjectOfType<T>();
                return instance;
            }
            return instance;
        }
    }

    [Foldout("Singleton"), InitializationField, SerializeField]
    protected bool dontDestroy = true;

    protected override void Initialise()
    {
        base.Initialise();

        if (!Application.isPlaying)
            return;
        
        if (Instance != null)
        {
            //instance already exists, so just remove this new one
            gameObject.SetActive(false); //set inactive so it doesn't trigger colliders etc.
            Destroy(gameObject);
            return;
        }
        
        instance = this as T;
        
        if (dontDestroy)
        {
            transform.parent = null; //can't be a child
            DontDestroyOnLoad(gameObject);
        }
    }
    
}