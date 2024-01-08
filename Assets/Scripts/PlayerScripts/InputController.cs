/// <Note>
/// Tutorial used for assistance in creation
/// https://www.youtube.com/watch?v=kGykP7VZCvg 
/// 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]

public class MoveInputEvent : UnityEvent<float, float> { }

public class InputController : MonoBehaviour
{
    Controls controls;
    public MoveInputEvent moveInputEvent;

    //anything in awake is for the InputController class specifically
    private void Awake()
    {
        controls = new Controls();
    }

    //this enables the action map controls
    private void OnEnable()
    {
        controls.GamePlay.Enable();
        //when Movement is performed it calls OnMove function
        controls.GamePlay.Movement.performed += OnMovePerformed;
        controls.GamePlay.Movement.canceled += OnMovePerformed;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        moveInputEvent.Invoke(moveInput.x, moveInput.y);
        //Debug.Log($"Move Input: {moveInput}");
    }
}
