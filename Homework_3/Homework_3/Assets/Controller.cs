using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Controller : MonoBehaviour
{
    public InputActionReference grip_r;
    public InputActionReference trigger_r;
    public InputActionReference activate_r;

    public InputActionReference grip_l;
    public InputActionReference trigger_l;
    public InputActionReference shoot_l;
    public InputActionReference secondary_l;

    public GameObject[] droneSpawns;
    public GameObject StartSign;

    public Hands hands;

    // Start is called before the first frame update
    void Start()
    {
        grip_r.action.Enable();
        trigger_r.action.Enable();
        activate_r.action.Enable();
        
        grip_l.action.Enable();
        trigger_l.action.Enable();
        shoot_l.action.Enable();
        secondary_l.action.Enable();


        activate_r.action.performed += (ctx) =>
        {
            hands.toggleActivation_r();
        };

        secondary_l.action.performed += (ctx) =>
        {
            foreach (GameObject droneSpawn in droneSpawns)
            {
                droneSpawn.GetComponent<DroneSpawner>().activate();
            }
            Destroy(StartSign);
        };
    }

    // Update is called once per frame
    void Update()
    {
        hands.SetGrip_r(grip_r.action.ReadValue<float>());
        hands.SetTrigger_r(trigger_r.action.ReadValue<float>());

        hands.SetGrip_l(grip_l.action.ReadValue<float>());
        hands.SetTrigger_l(trigger_l.action.ReadValue<float>());
        hands.SetShooting_l(shoot_l.action.IsPressed());
    }
}
