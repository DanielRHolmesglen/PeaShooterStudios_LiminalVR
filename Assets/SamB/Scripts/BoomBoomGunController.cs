using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;
using System;
/// <summary>
/// Responsible for charging the gun when mouse is held down, firing gun, dealing damage, drawing the laser and turning on/off the particle effect. 
/// </summary>
public class BoomBoomGunController : MonoBehaviour 
{

    //public float minChargeTime = 0.5f;
    //public float maxChargeTime = 1.5f;
    //public float DirectMinDamage = 5f;
    //public float DirectMaxDamage = 25f;
    public float maxRange = 20f; //how far line renderer is drawn if nothing is hit 

    public float explosionRadius = 5f; // The radius of the explosion
    public float explosionMinDamage = 5f; // Minimum damage to apply within the explosion radius
    public float explosionMaxDamage = 25f; // Maximum damage to apply at the center of the explosion

    public bool isCoolingDown;
    public float cooldown = 2f;
    public float fireDelay = 0.25f;
    
    //public float chargeTimer;
    //public bool isCharging;

    public AudioClip laserFireSound;
    private AudioSource audioSource;

    private LineRenderer laserLine; //reference to line renderer for laser
    public ParticleSystem chargingParticles; // Reference to charging particle effect
    //public ParticleSystem fireParticles;
    public Transform gunBarrelEnd;
    public ParticleSystem laserImpactParticle;


    public float directDamage = 25f;




    private void Start()
    {
        laserLine = GetComponent<LineRenderer>();  // Find  Line Renderer on the gun
        //chargingParticles = GetComponentInChildren<ParticleSystem>();
        laserLine.enabled = false; // Disable the Line Renderer initially

        //get sound stuff for firing the laser
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = laserFireSound;
    }

    private void Update()
    {
        var primaryInput = VRDevice.Device.PrimaryInputDevice;

        if (primaryInput.GetButtonDown(VRButton.One) || Input.GetMouseButtonDown(0) && (!isCoolingDown))    //only checking the frame it's clicked as it shouldnt be "full auto"
        {
            chargingParticles.Play();

            Invoke("FireGun", fireDelay);

            Invoke("TurnOffParticles", fireDelay);

            StartCoroutine(GunCooldown());

            //turn the bool on 
            //isCharging = true;

        }

        /* OLD CHARGING STUFF
        if (primaryInput.GetButtonUp(VRButton.One) || Input.GetMouseButtonUp(0)) //when mouse button released, fire laser
        {
            if (chargeTimer >= minChargeTime) //fire laser if the minimum charge time was reached
            {
                isCharging = false;
                BoomBoom(chargeTimer);
                chargingParticles.Stop();
            }

            else //do not fire laser if minimum charge time wasnt reached
            {
                isCharging = false;
                chargeTimer = 0f;
                chargingParticles.Stop();
            }
        }

        if (isCharging)
        {
            //playing charge effect
            chargingParticles.Play();

            //increase charge timer
            chargeTimer += Time.deltaTime;
        }
        */
    }



    private void FireGun()
    {
        //reset charge timer
        //chargeTimer = 0f;

        //checking what it hits/if it hits something 
        if (Physics.Raycast(gunBarrelEnd.position, gunBarrelEnd.forward, out RaycastHit hit))
        {
            // Draw the laser line in the direction the gun was pointing.
            laserLine.enabled = true;
            laserLine.SetPosition(0, gunBarrelEnd.position);
            laserLine.SetPosition(1, hit.point);

            //Create explosion (AKA particle system)
            ParticleSystem impactParticle = Instantiate(laserImpactParticle, hit.point, Quaternion.identity);
            Destroy(impactParticle.gameObject, impactParticle.main.duration);

            //get the directly hit enemy
            EnemyHealth directEnemyHealth = hit.collider.GetComponentInParent<EnemyHealth>();

            //Do additional damage to the the directly hit enemy (if there is one)
            if (directEnemyHealth != null)
            {
                //float directDamage = Mathf.Lerp(DirectMinDamage, DirectMaxDamage, (chargeTime - minChargeTime) / (maxChargeTime - minChargeTime));
                directEnemyHealth.Damage(directDamage, DamageType.Gun);
            }

            // Create an array of hit enemies, and make that array be all of the hit enemies
            Collider[] enemyColliders = Physics.OverlapSphere(hit.point, explosionRadius);

            // Apply explosion damage to each enemy in the new hit enemies array
            foreach (Collider hitCollider in enemyColliders)
            {
                // Calculate the damage based on the distance from the center of the explosion. Closer to center is bigger damage.
                float distance = Vector3.Distance(hit.point, hitCollider.transform.position);
                float aoeDamage = Mathf.Lerp(explosionMinDamage, explosionMaxDamage, distance / explosionRadius);

                // Apply damage to the enemy
                EnemyHealth enemyHealth = hitCollider.GetComponentInParent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.Damage(aoeDamage, DamageType.Gun);
                }

            }

            // Draw the laser line in the direction the gun was pointing.
            laserLine.enabled = true;
            laserLine.SetPosition(0, gunBarrelEnd.position);
            laserLine.SetPosition(1, gunBarrelEnd.position + gunBarrelEnd.forward * maxRange);

            //Turn everything off
            Invoke("TurnOffLaser", 0.3f);
        }

        else //if the gun does not hit anything, still draw line just make end point its 'max range'
        {
            Vector3 targetPosition = gunBarrelEnd.position + gunBarrelEnd.forward * maxRange;

            // Enable the laser (Line Renderer) and set its positions
            laserLine.enabled = true;
            laserLine.SetPosition(0, gunBarrelEnd.position);
            laserLine.SetPosition(1, targetPosition);
            Invoke("TurnOffLaser", 0.3f);
        }

        /*
        //if the raycast is hitting something 
        if (Physics.Raycast(gunBarrelEnd.position, gunBarrelEnd.forward, out RaycastHit hit))
        {
            //play laser fire sound
            audioSource.Play();

            // Draw the laser (enable Line Renderer and set positions)
            laserLine.enabled = true;
            laserLine.SetPosition(0, gunBarrelEnd.position);
            laserLine.SetPosition(1, hit.point);
            Invoke("TurnOffLaser", 0.3f);

            //Get reference to enemy health of the hit thing
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();

            if (enemyHealth != null)  //If we got a reference to enemy health

            {
                float damage = Mathf.Lerp(minDamage, maxDamage, (chargeTime - minChargeTime) / (maxChargeTime - minChargeTime));


                enemyHealth.Damage(damage, DamageType.Gun);
            }
        }

        else //if the gun does not hit anything
        {
            Vector3 targetPosition = gunBarrelEnd.position + gunBarrelEnd.forward * maxRange;

            // Enable the laser (Line Renderer) and set its positions
            laserLine.enabled = true;
            laserLine.SetPosition(0, gunBarrelEnd.position);
            laserLine.SetPosition(1, targetPosition);
            Invoke("TurnOffLaser", 0.3f);
        }
        */
    }
    

    private void TurnOffLaser()
    {
        laserLine.enabled = false;
    }

    private void TurnOffParticles()
    {
        chargingParticles.Stop();

    }

    private IEnumerator GunCooldown()
    {
        isCoolingDown = true;
        yield return new WaitForSeconds(cooldown);
        isCoolingDown = false;
    }

}
