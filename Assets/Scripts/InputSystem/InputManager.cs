// using System;
// using UnityEngine;
// using UnityEngine.InputSystem;
// using ZPackage;

// [DefaultExecutionOrder(-1)]
// public class InputManager : GenericSingleton<InputManager>
// {
//     TouchControl touchControls;
//     public new bool IsClick => touchControls.Touch.TouchPress.IsPressed();
//     public new bool IsDown => touchControls.Touch.TouchPress.WasPressedThisFrame();
//     public new bool IsUp => touchControls.Touch.TouchPress.WasReleasedThisFrame();
//     public Vector2 TouchDelta => touchControls.Touch.TouchDelta.ReadValue<Vector2>();
//     public Vector2 TouchPos => touchControls.Touch.TouchPosition.ReadValue<Vector2>();
//     ///<summary> position and time<summary>
//     public Action<Vector2, float> OnDown;
//     ///<summary> called Twice<summary>
//     public Action<Vector2, float> OnPerform;
//     ///<summary> position and time<summary>
//     public Action<Vector2, float> OnUp;
//     private void Awake()
//     {
//         touchControls = new TouchControl();
//     }

//     private void OnEnable()
//     {
//         touchControls.Enable();
//         touchControls.Touch.TouchPress.started += TouchPressStart;
//         touchControls.Touch.TouchPress.performed += TouchPressPerform;
//         touchControls.Touch.TouchPress.canceled += TouchPressCancel;
//     }
//     // private void Update()
//     // {
//     //     print(TouchDelta.normalized);
//     // }

//     private void TouchPressPerform(InputAction.CallbackContext context)
//     {
//         // Debug.Log("OnPerform" + touchControls.Touch.TouchPosition.ReadValue<Vector2>() + " dd" + (float)context.time);
//         OnPerform?.Invoke(touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.time);
//     }

//     private void TouchPressStart(InputAction.CallbackContext context)
//     {
//         // Debug.Log("OnDown" + touchControls.Touch.TouchPosition.ReadValue<Vector2>() + " dd" + (float)context.startTime);
//         OnDown?.Invoke(touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.startTime);
//     }

//     private void TouchPressCancel(InputAction.CallbackContext context)
//     {
//         // Debug.Log("OnUP" + touchControls.Touch.TouchPosition.ReadValue<Vector2>() + " uu" + (float)context.time);
//         OnUp?.Invoke(touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.time);
//     }


//     private void OnDisable()
//     {
//         touchControls.Disable();
//     }
// }
