using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public Transform playerTransform;
    public float forceMultiplier = 10f;
    public float rotationSpeed = 5f;
    public float inTargetRange = 5f;
    public float shootDelay = 1f;
    public int health = 3;

    public Transform bulletSpawnPoint;
    public ParticleSystem shotParticleSystem;
    public LayerMask mask;
    public ParticleSystem impactParticleSystem;
    public TrailRenderer bulletTrail;
    public AudioSource bulletAudioSource;

    private float lastShootTime = -10f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned. Please assign a player transform in the Unity Editor.");
        }
        
    }

    void Update()
    {
        if (health > 0)
        {
            RotateTowardsPlayer();

            if (Vector3.Distance(transform.position, transform.parent.position) >= inTargetRange)
            {
                FlyToTargetPosition();
            }
            // Check if the object is within shooting range
            else if (lastShootTime + shootDelay < Time.time)
            {
                Shoot();
            }
        }
        else
        {
            rb.useGravity = true;

            if(transform.position.y < -20)
            {
                Destroy(gameObject);
            }
        }
    }

    void FlyToTargetPosition()
    {
        // Calculate the direction to the target
            Vector3 direction = transform.parent.position - transform.position;

            // Normalize the direction vector to get a unit vector
            direction.Normalize();

            // Apply force to the rigidbody
            rb.AddForce(direction * forceMultiplier);
        
    }

    void RotateTowardsPlayer()
    {
        if (playerTransform != null)
        {
            // Calculate the direction to the player
            Vector3 directionToPlayer = playerTransform.position - transform.position;

            // Rotate towards the player using Slerp for smooth rotation
            Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPlayer), rotationSpeed * Time.deltaTime);

            // Apply the rotation to the rigidbody
            rb.MoveRotation(rotation);
        }
    }

    void Shoot()
    {
        Vector3 direction = -bulletSpawnPoint.up;

        Instantiate(shotParticleSystem, bulletSpawnPoint.position, Quaternion.LookRotation(direction));

        if (Physics.Raycast(bulletSpawnPoint.position, direction, out RaycastHit hit, float.MaxValue, mask))
        {
            TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(trail, hit));
        }

        lastShootTime = Time.time;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        AudioSource shotSound = Instantiate(bulletAudioSource, startPosition, Quaternion.identity);
        Destroy(shotSound, 3);

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }
        trail.transform.position = hit.point;
        Instantiate(impactParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(trail.gameObject, trail.time);
    }

    public void Damage()
    {
        // Reduce health by 1
        health--;
    }
}
