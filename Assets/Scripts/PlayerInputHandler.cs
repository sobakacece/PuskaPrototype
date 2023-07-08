using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;


public class PlayerInputHandler : MonoBehaviour
{
    public const string mouseXstring = "Mouse X";
    public const string mouseYstring = "Mouse Y";
    public const string mouseScrollingString = "Mouse ScrollWheel";
    public static float MouseXInput { get => UnityEngine.Input.GetAxis(mouseXstring); }
    public static float MouseYInput { get => UnityEngine.Input.GetAxis(mouseYstring); }
    public static float MouseScrollInput { get => UnityEngine.Input.GetAxis(mouseScrollingString); }
    public static Action OnJump;
    public static Vector2 MoveInput { get; private set; } = Vector2.zero;
    PlayerInputMap input;

    void Awake()
    {
        // DontDestroyOnLoad(this.gameObject);
    }
    private void OnEnable()
    {
        if (input == null)
        {
            input = new PlayerInputMap();
        }
        input.Enable();

        input.Land.Move.performed += SetMove;
        input.Land.Move.canceled += SetMove;

        input.Land.Jump.performed += SetJump;
    }
    private void OnDisable()
    {
        input.Land.Move.performed -= SetMove;
        input.Land.Move.canceled -= SetMove;

        input.Land.Jump.performed -= SetJump;
        input.Disable();
    }
    private void SetMove(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }
    private void SetJump(InputAction.CallbackContext ctx)
    {
        OnJump?.Invoke();
    }


}
