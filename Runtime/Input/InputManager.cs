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
    public class InputManager : PersistentSingleton<InputManager>
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
            
            ForceTouchSimulation();

            generalInput.Enable();
        }
        
        private void ForceTouchSimulation()
        {
            TouchSimulation.Enable();
                    
            DontDestroyOnLoad(TouchSimulation.instance.gameObject);
            
            //need to set the device as the current device after enabled
            playerInput.SwitchCurrentControlScheme(InputSystem.devices.First(device => device == Touchscreen.current));
        }
        
    }
}
