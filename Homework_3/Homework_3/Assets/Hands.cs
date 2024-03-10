using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class Hands : MonoBehaviour
{
    Animator animator;
    private float gripTarget_r;
    private float triggerTarget_r;
    private float gripCurrent_r;
    private float triggerCurrent_r;
    private float gripTarget_l;
    private float triggerTarget_l;
    private float gripCurrent_l;
    private float triggerCurrent_l;

    private string animatorGrip_r = "Grip_r";
    private string animatorTrigger_r = "Trigger_r";
    private string animatorActivate_r = "Activated_r";
    private string animatorShot_r = "Shot_r";
    private string animatorGrip_l = "Grip_l";
    private string animatorTrigger_l = "Trigger_l";
    private string animatorActivate_l = "Activated_l";
    private string animatorShot_l = "Shot_l";

    public float speed;
    private bool isActivated_r = false;
    private bool isActivated_l = false;
    private bool isShooting_l = false;
    private bool activationTriggered = false;

    private float lastShootTime_r = -10f;
    public float fireDelay_r;
    public float shotDelay_r = 0.12f;
    public ParticleSystem shootingSystem_r;
    public Transform bulletSpawnPoint_r;

    public float impactForce = 30f;

    private float lastShootTime_l = -10f;
    public float shotDelay_l = 0.01f;
    public float activationDelay_l = 1f;
    public ParticleSystem shootingSystem_l;
    public Transform bulletSpawnPoint_l;

    public LayerMask mask;
    public ParticleSystem impactParticleSystem;
    public TrailRenderer bulletTrail;

    public AudioSource bulletAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isActivated_r && (gripTarget_r < 0.5 || triggerTarget_r > 0.01))
        {
            isActivated_r = false;
            animator.SetBool(animatorActivate_r, false);
        }

        if (isActivated_l && (gripTarget_l < 0.5 || triggerTarget_l < 0.5))
        {
            isActivated_l = false;
            animator.SetBool(animatorActivate_l, false);
        }

        else if (!isActivated_l && !activationTriggered && (gripTarget_l > 0.6 && triggerTarget_l > 0.6))
        {
            animator.SetBool(animatorActivate_l, true);
            StartCoroutine(ActivateDelay(activationDelay_l));
        }

        AnimateHands();
    }

    private IEnumerator ActivateDelay(float delay)
    {
        yield return Wait(delay);
        isActivated_l = true;
        activationTriggered = false;
    }

    //Set grip
    public void SetGrip_r(float value)
    {
        gripTarget_r = value;
    }
    public void SetGrip_l(float value)
    {
        gripTarget_l = value;
    }

    //Set Trigger
    public void SetTrigger_r(float value)
    {
        triggerTarget_r = value; 
    }
    public void SetTrigger_l(float value)
    {
        triggerTarget_l = value;
    }

    void AnimateHands()
    {
        if(gripCurrent_r != gripTarget_r)
        {
            gripCurrent_r = Mathf.MoveTowards(gripCurrent_r, gripTarget_r, Time.deltaTime * speed);
            animator.SetFloat(animatorGrip_r, gripCurrent_r);
        }
        if(triggerCurrent_r != triggerTarget_r)
        {
            triggerCurrent_r = Mathf.MoveTowards(triggerCurrent_r, triggerTarget_r, Time.deltaTime * speed);
            animator.SetFloat(animatorTrigger_r, triggerCurrent_r);
        }

        if (gripCurrent_l != gripTarget_l)
        {
            gripCurrent_l = Mathf.MoveTowards(gripCurrent_l, gripTarget_l, Time.deltaTime * speed);
            animator.SetFloat(animatorGrip_l, gripCurrent_l);
        }
        if (triggerCurrent_l != triggerTarget_l)
        {
            triggerCurrent_l = Mathf.MoveTowards(triggerCurrent_l, triggerTarget_l, Time.deltaTime * speed);
            animator.SetFloat(animatorTrigger_l, triggerCurrent_l);
        }
    }

    public void toggleActivation_r()
    {
        if (isActivated_r)
        {
            //Shoot
            if (lastShootTime_r + shotDelay_r < Time.time)
            {
                animator.SetTrigger(animatorShot_r);
                StartCoroutine(Shoot_r());
            }
        }
        else if(gripTarget_r > 0.5 && triggerTarget_r < 0.01)
        {
            isActivated_r = true;
            animator.SetBool(animatorActivate_r, true);
        }
    }

    public void SetShooting_l(bool active)
    {
        isShooting_l = active;
        
        if(active && isActivated_l && lastShootTime_l + shotDelay_l < Time.time)
        {
            //shoot
            animator.SetTrigger(animatorShot_l);
            Shoot_l();
        }
    }

    private IEnumerator Shoot_r()
    {
        yield return Wait(fireDelay_r);

        Vector3 direction = getDirection_r();
            
        Instantiate(shootingSystem_r, bulletSpawnPoint_r.position, Quaternion.LookRotation(direction));

        if (Physics.Raycast(bulletSpawnPoint_r.position, direction, out RaycastHit hit, float.MaxValue, mask))
        {
            TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint_r.position, Quaternion.identity);

            yield return StartCoroutine(SpawnTrail(trail, hit));

            lastShootTime_r = Time.time;
        }
        
    }

    private void Shoot_l()
    {
        Vector3 direction = getDirection_l();

        Instantiate(shootingSystem_l, bulletSpawnPoint_l.position, Quaternion.LookRotation(direction));

        if (Physics.Raycast(bulletSpawnPoint_l.position, direction, out RaycastHit hit, float.MaxValue, mask))
        {
            TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint_l.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(trail, hit));
        }
        lastShootTime_l = Time.time;
    }

    private Vector3 getDirection_r ()
    {
        return bulletSpawnPoint_r.up;
    }

    private Vector3 getDirection_l()
    {
        return -bulletSpawnPoint_l.up;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        AudioSource shotSound = Instantiate(bulletAudioSource, startPosition, Quaternion.identity);
        Destroy(shotSound, 3);

        while(time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }
        trail.transform.position = hit.point;
        Instantiate(impactParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));

        Drone hitDrone = hit.transform.GetComponent<Drone>();
        if(hitDrone != null)
        {
            hitDrone.Damage();
        }

        if(hit.rigidbody != null)
        {
            hit.rigidbody.AddForceAtPosition(-hit.normal * impactForce, hit.point);
        }

        Destroy(trail.gameObject, trail.time);
    }

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
