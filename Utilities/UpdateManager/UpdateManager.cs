using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : Singleton<UpdateManager>
{

    public event Action OnUpdate;
    public event Action OnLateUpdate;
    public event Action OnFixedUpdate;

    private void Update()
    {
        OnUpdate?.Invoke();
    }

    private void FixedUpdate()
    {
        OnLateUpdate?.Invoke();
    }

    private void LateUpdate()
    {
        OnFixedUpdate?.Invoke();
    }
    
}
