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
using DIALOGUE;

[Serializable]

public class MoveInputEvent : UnityEvent<float, float> { }

public class InputController : MonoBehaviour
{

    Controls controls;
    public MoveInputEvent moveInputEvent;

    private PlayerInput input;
    private List<(InputAction action, Action<InputAction.CallbackContext> command)> actions = new List<(InputAction action, Action<InputAction.CallbackContext> command)>();
    
    //anything in awake is for the InputController class specifically
    private void Awake()
    {
        controls = new Controls();
        input = GetComponent<PlayerInput>();
        InitializeActions();
    }

    private void InitializeActions()
    {
        actions.Add((input.actions["Next"], OnNext));
    }

    //this enables the action map controls
    private void OnEnable()
    {
        foreach(var inputAction in actions)
        {
            inputAction.action.performed += inputAction.command;
        }
        controls.GamePlay.Enable();
        //when Movement is performed it calls OnMove function
        controls.GamePlay.Movement.performed += OnMovePerformed;
        controls.GamePlay.Movement.canceled += OnMovePerformed;
    }

    private void OnDisable()
    {
        foreach (var inputAction in actions)
        {
            inputAction.action.performed -= inputAction.command;
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        moveInputEvent.Invoke(moveInput.x, moveInput.y);
        //Debug.Log($"Move Input: {moveInput}");
    }

    public void OnNext(InputAction.CallbackContext c)
    {
        DialogueSystem.instance.OnUserPrompt_Next();
        //Debug.Log("Success");
    }

}
