using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTransform : MonoBehaviour
{
    public Transform target;
    public Transform origin;

    public Vector3 offsetT;
    public Quaternion offsetR;

    private Vector3 previousPosition;
    private Vector3 originPreviousPosition;
    private Quaternion previousRotation;

    private void Start()
    {
        // init previous transform
        previousPosition = target.position + offsetT;
        previousRotation = target.rotation * offsetR;

        originPreviousPosition = new Vector3(0, 0, 0);
        
        transform.position = target.position + (origin.position - originPreviousPosition);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 originOffset = origin.position - originPreviousPosition;

        // Calculate delta transform and apply to current transform
        transform.position += target.position - previousPosition - originOffset;

        Quaternion deltaRotation = target.rotation * Quaternion.Inverse(previousRotation);

        transform.position = deltaRotation * (transform.position - target.position) + target.position;
        transform.rotation = deltaRotation * transform.rotation;

        previousPosition = target.position;
        previousRotation = target.rotation;
        originPreviousPosition = origin.position;
    }
}
