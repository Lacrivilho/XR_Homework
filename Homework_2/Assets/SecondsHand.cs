using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondsHand : MonoBehaviour
{
    private float passedTime = 0;

    // Update is called once per frame
    void Update()
    {
        passedTime += Time.deltaTime;

        if(passedTime >= 1)
        {
            transform.Rotate(0,6,0);
            passedTime = 0;
        }
    }
}
