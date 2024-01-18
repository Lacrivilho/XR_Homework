using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Teleporter : MonoBehaviour
{
    public InputActionReference teleportAction;
    public XRController rightController;
    public Transform XROrigin;
    public Vector3 roomPosition;
    public Vector3 balconyPosition;

    private bool inRoom = true; //Track if the player is in the room

    // Start is called before the first frame update
    private void Start()
    {
        teleportAction.action.Enable();
        teleportAction.action.performed += TeleportActionHandler;
    }

    private void TeleportActionHandler(InputAction.CallbackContext context)
    {
        if (inRoom)
        {
            //teleport to balcony
            XROrigin.position = balconyPosition;
        }
        else
        {
            //teleport back to room
            XROrigin.position = roomPosition;
        }

        inRoom = !inRoom;
    }

    private void OnDisable()
    {
        teleportAction.action.Disable();
    }
}
