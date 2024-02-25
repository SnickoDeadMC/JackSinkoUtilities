using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JacksUtils
{
    public abstract class ActionMapManager : ScriptableObject
    {

        private readonly Dictionary<string, InputAction> actionsCached = new();

        private InputActionMap actionsMapCached;

        private InputActionMap ActionMap =>
            actionsMapCached ??= InputManager.Instance.Controls.FindActionMap(GetActionMapName());

        public void Enable(bool enable = true)
        {
            if (enable)
            {
                ActionMap.Enable();
                OnEnableMap();
            }
            else
            {
                ActionMap.Disable();
                OnDisableMap();
            }

            UtilsLoggers.InputLogger.Log($"{(enable ? "Enabled" : "Disabled")} action map {GetActionMapName()}");
        }

        public void Disable() => Enable(false);

        protected virtual void OnEnableMap()
        {
            
        }

        protected virtual void OnDisableMap()
        {
            
        }

        protected InputAction GetOrCacheAction(string action)
        {
            if (!actionsCached.ContainsKey(action))
                actionsCached[action] = InputManager.Instance.Controls.FindAction(action);

            return actionsCached[action];
        }

        protected abstract string GetActionMapName();

    }
}
