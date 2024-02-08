using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCameraController : MonoBehaviour
{
    public Transform origin; // Parent object (e.g., player or origin point)
    public Transform target; // Target object to rotate towards

    void Update()
    {
        if (origin != null && target != null)
        {
            transform.LookAt(target, Vector3.up);
        }
        else
        {
            Debug.LogWarning("Make sure to assign both origin and target objects in the inspector!");
        }
    }
}