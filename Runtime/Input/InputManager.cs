using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace JacksUtils
{
    [CreateAssetMenu(menuName = "Input/Input Manager (Utils)")]
    public class InputManager : SingletonScriptable<InputManager>
    {
        
        [SerializeField] private InputActionAsset controls;
        
        [Header("Action maps")]
        [SerializeField] private GeneralInputManager generalInput;
        
        public InputActionAsset Controls => controls;
        public GeneralInputManager GeneralInput => generalInput;

        public static ReadOnlyArray<Touch> ActiveTouches => Touch.activeTouches;
        
        protected override void OnInstanceLoaded()
        {
            base.OnInstanceLoaded();
            
            generalInput.Enable();

        }

    }
}
