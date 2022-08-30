using System;
using UnityEngine;
using MyBox;

public abstract class Singleton<T> : Updatable where T : MonoBehaviour
{
    
    public static T Instance { get; private set; }
    
    [Foldout("Singleton"), InitializationField, SerializeField]
    protected bool dontDestroy = true;

    protected override void Initialise()
    {
        base.Initialise();

        if (Instance != null)
        {
            //instance already exists, so just remove this new one
            Destroy(gameObject);
            return;
        }
        
        Instance = this as T;
        
        if (dontDestroy)
        {
            transform.parent = null; //can't be a child
            DontDestroyOnLoad(gameObject);
        }
    }
    
}