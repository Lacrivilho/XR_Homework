using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Hands;

public class CustomGrab : MonoBehaviour
{
    // This script should be attached to both controller objects in the scene
    // Make sure to define the input in the editor (LeftHand/Grip and RightHand/Grip recommended respectively)
    CustomGrab otherHand = null;
    public List<Transform> nearObjects = new List<Transform>();
    public Transform grabbedObject = null;
    public InputActionReference action;
    public InputActionReference doubleRotate;
    bool grabbing = false;

    private Vector3 previousPosition;
    private Quaternion previousRotation;

    private void Start()
    {
        action.action.Enable();
        doubleRotate.action.Enable();  

        // Find the other hand
        foreach (CustomGrab c in transform.parent.GetComponentsInChildren<CustomGrab>())
        {
            if (c != this)
                otherHand = c;
        }

        // init previous transform
        previousPosition = transform.position;
        previousRotation = transform.rotation;
    }

    void Update()
    {
        grabbing = action.action.IsPressed();
        if (grabbing)
        {
            // Grab nearby object or the object in the other hand
            if (!grabbedObject)
                if(nearObjects.Count > 0)
                {
                    grabbedObject = nearObjects[0];
                }

            if (grabbedObject && otherHand.grabbedObject != grabbedObject)
            {
                // Calculate delta transform and apply to current transform
                grabbedObject.position += transform.position-previousPosition;

                Quaternion deltaRotation = transform.rotation * Quaternion.Inverse(previousRotation);

                // Double the rotation
                if (doubleRotate.action.IsPressed())
                {
                    deltaRotation = deltaRotation * deltaRotation;
                }

                grabbedObject.position = deltaRotation * (grabbedObject.position - transform.position) + transform.position;
                grabbedObject.rotation = deltaRotation * grabbedObject.rotation;
            }
            else if(grabbedObject && otherHand.grabbedObject == grabbedObject)
            {
                // Calculate half delta transform and apply to current transform
                grabbedObject.position += (transform.position - previousPosition) / 2;

                Quaternion deltaRotation = transform.rotation * Quaternion.Inverse(previousRotation);
                deltaRotation = Quaternion.Slerp(deltaRotation, Quaternion.identity, 0.5f);

                //Double the rotation
                if (doubleRotate.action.IsPressed())
                {
                    deltaRotation = deltaRotation * deltaRotation;
                }

                grabbedObject.position = deltaRotation * (grabbedObject.position - transform.position) + transform.position;
                grabbedObject.rotation = deltaRotation * grabbedObject.rotation;
            }
        }
        // If let go of button, release object
        else if (grabbedObject)
            grabbedObject = null;

        previousPosition = transform.position;
        previousRotation = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Make sure to tag grabbable objects with the "grabbable" tag
        // You also need to make sure to have colliders for the grabbable objects and the controllers
        // Make sure to set the controller colliders as triggers or they will get misplaced
        // You also need to add Rigidbody to the controllers for these functions to be triggered
        // Make sure gravity is disabled though, or your controllers will (virtually) fall to the ground

        Transform t = other.transform;
        if (t && t.tag.ToLower() == "grabbable")
            nearObjects.Add(t);
    }

    private void OnTriggerExit(Collider other)
    {
        Transform t = other.transform;
        if (t && t.tag.ToLower() == "grabbable")
            nearObjects.Remove(t);
    }
}