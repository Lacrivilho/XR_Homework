using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCameraController : MonoBehaviour
{
    public Transform origin; // Parent object (e.g., player or origin point)
    public Transform target; // Target object to rotate towards
    public Transform rotationTarget;

    void Update()
    {
        if (origin != null && target != null)
        {
            transform.LookAt(target, rotationTarget.forward);
        }
        else
        {
            Debug.LogWarning("Make sure to assign both origin and target objects in the inspector!");
        }
    }
}