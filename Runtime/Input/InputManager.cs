using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.Utilities;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace JacksUtils
{
    public class InputManager : Singleton<InputManager>
    {
        
        [SerializeField] private InputActionAsset controls;
        [SerializeField] private PlayerInput playerInput;

        [Header("Action maps")]
        [SerializeField] private GeneralInputManager generalInput;
        
        public InputActionAsset Controls => controls;
        public GeneralInputManager GeneralInput => generalInput;

        public static ReadOnlyArray<Touch> ActiveTouches => Touch.activeTouches;

        protected override void Initialise()
        {
            base.Initialise();
            
            EnhancedTouchSupport.Enable();
            UtilsLoggers.InputLogger.Log("Enhanced touch support has been force enabled.");
            
            ForceTouchSimulation();

            generalInput.Enable();
        }
        
        private void ForceTouchSimulation()
        {
            TouchSimulation.Enable();

            DontDestroyOnLoad(TouchSimulation.instance.gameObject);

            //need to set the device as the current device after enabled
            InputDevice touchscreen = InputSystem.devices.First(device => device == Touchscreen.current);
            playerInput.SwitchCurrentControlScheme(touchscreen);
            
            UtilsLoggers.InputLogger.Log("Touch simulation has been force enabled.");
        }
        
    }
}
