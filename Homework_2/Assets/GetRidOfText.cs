using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GetRidOfText : MonoBehaviour
{
    public InputActionReference action;

    // Start is called before the first frame update
    void Start()
    {
        action.action.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (action.action.IsPressed())
        {
            Destroy(gameObject);
        }
    }
}
