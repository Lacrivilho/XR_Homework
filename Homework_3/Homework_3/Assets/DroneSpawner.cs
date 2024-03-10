using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSpawner : MonoBehaviour
{
    public bool activated = false;

    public GameObject drone;
    public Vector3 offset;
    public float spawnFrequency = 2f;

    private int state = 1;
    private float lastDeath = 0f;
    private float spawnDelay = 0f;

    public void activate()
    {
        activated = !activated;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            if (state == 1 && transform.childCount < 1)
            {
                state = 0;
                lastDeath = Time.time;
                spawnDelay = Random.value * spawnFrequency;
            }

            if (state == 0 && lastDeath + spawnDelay < Time.time)
            {
                state = 1;
                GameObject newDrone = Instantiate(drone, transform.position + offset, Quaternion.identity);
                newDrone.transform.SetParent(transform);
            }
        }
    }
}
