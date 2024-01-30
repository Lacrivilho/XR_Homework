using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RemoveText : MonoBehaviour
{
    public InputActionReference primaryButton;
    public InputActionReference secondaryButton;
    public InputActionReference trigger;

    public float teleportDistance = 2.0f;

    private bool teleported = false;

    void Start()
    {
        primaryButton.action.Enable();
        primaryButton.action.performed += TeleportActionHandler;

        secondaryButton.action.Enable();
        secondaryButton.action.performed += TeleportActionHandler;

        trigger.action.Enable();
        trigger.action.performed += TeleportActionHandler;
    }

    private void TeleportActionHandler(InputAction.CallbackContext context)
    {
        if (!teleported) { 
            // Get the direction opposite to the current up vector (downward)
            Vector3 teleportDirection = -transform.up;

            // Teleport the object downward
            transform.position += teleportDirection * teleportDistance;
        }
    }

    void OnDisable()
    {
        primaryButton.action.Disable();
        secondaryButton.action.Disable();
        trigger.action.Disable();
    }
}
