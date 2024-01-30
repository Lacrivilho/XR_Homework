using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotator : MonoBehaviour
{
    // Public variable to control rotation speed
    public float rotationSpeed = 5.0f;

    // Update is called once per frame
    void Update()
    {
        // Rotate the object around the y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
