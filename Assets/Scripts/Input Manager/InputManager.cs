using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance { get; private set; }
    private InputSystem inputSystem;

    public Action onInteract;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        inputSystem = new InputSystem();
    }

    private void OnEnable()
    {
        inputSystem.Enable();

        inputSystem.Player.Interact.performed += OnInteractButtonPressed;
    }

    private void OnDisable()
    {
        inputSystem.Disable();

        inputSystem.Player.Interact.performed -= OnInteractButtonPressed;
    }

    public Vector2 GetMouseDelta()
    {
        return inputSystem.Player.Look.ReadValue<Vector2>();
    }

    public Vector2 GetPlayerMovement()
    {
        return inputSystem.Player.Move.ReadValue<Vector2>();
    }

    private void OnInteractButtonPressed(InputAction.CallbackContext ctx)
    {
        onInteract?.Invoke();
    }
}
