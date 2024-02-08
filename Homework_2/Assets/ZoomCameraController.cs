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
            // Get the direction vector from origin to target
            //Vector3 directionToTarget = target.position - origin.position;

            // Calculate the rotation to face the target
            //Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
            transform.LookAt(target, origin.transform.localRotation.eulerAngles);

            // Apply the rotation with adjustment around the vector
            //origin.rotation = Quaternion.Euler(targetRotation.eulerAngles);
        }
        else
        {
            Debug.LogWarning("Make sure to assign both origin and target objects in the inspector!");
        }
    }
}