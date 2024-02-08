using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LensController : MonoBehaviour
{
    public Transform referenceObject;

    void Update()
    {
        if (referenceObject != null)
        {
            // Calculate rotation
            var lookPos = new Vector3(0, 20, 0) - referenceObject.transform.position - transform.position;
            var referenceLocal = referenceObject.InverseTransformPoint(lookPos);
            var localProjection = new Vector3(referenceLocal.x, 0, referenceLocal.z);
            var worldProjection = referenceObject.TransformPoint(localProjection);

            var rotation = Quaternion.LookRotation(worldProjection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);
        }
        else
        {
            Debug.LogWarning("Reference object is not assigned. Please assign a reference object in the inspector.");
        }
    }
} 
