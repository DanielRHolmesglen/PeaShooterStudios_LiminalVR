using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;
using System;
/// <summary>
/// Responsible for AoE gun. has gun atsts, is in charge of it's firing rate, drawing the laser, making the particle effect and sounds. 
/// </summary>
public class BoomBoomGunController : MonoBehaviour 
{
    
    public float maxRange = 20f; //how far line renderer is drawn if nothing is hit 

    public float explosionRadius = 5f; // The radius of the explosion
    public float explosionMinDamage = 5f; // outer edge dmg 
    public float explosionMaxDamage = 25f; // center of explosion dmg

    public bool isCoolingDown;
    public float cooldown = 2f;
    public float fireDelay = 0.25f;

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
        laserLine = GetComponent<LineRenderer>();  
        laserLine.enabled = false; // Disable the Line Renderer initially

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = laserFireSound;
    }

    private void Update()
    {
        var input = VRDevice.Device.SecondaryInputDevice; //setting vr device. IS SECONDARY/LEFT HAND.

        if (input.GetButtonDown(VRButton.One) && (!isCoolingDown) || Input.GetMouseButtonDown(0) && (!isCoolingDown))    //only checking the frame it's clicked as it shouldnt be "full auto"
        {
            chargingParticles.Play();

            Invoke("FireGun", fireDelay);

            Invoke("TurnOffParticles", fireDelay);

            StartCoroutine(GunCooldown());
            

        }
        
    }


    //firing raycast, creating explosion, damage and making effects/sounds
    private void FireGun()
    {
        if (Physics.Raycast(gunBarrelEnd.position, gunBarrelEnd.forward, out RaycastHit hit))
        {
            // Draw the laser line to hit the target point 
            laserLine.enabled = true;
            laserLine.SetPosition(0, gunBarrelEnd.position);
            laserLine.SetPosition(1, hit.point);

            //Create explosion
            ParticleSystem impactParticle = Instantiate(laserImpactParticle, hit.point, Quaternion.identity);
            Destroy(impactParticle.gameObject, impactParticle.main.duration);

            //get the directly hit enemy to do additional damage 
            EnemyHealth directEnemyHealth = hit.collider.GetComponentInParent<EnemyHealth>();

            if (directEnemyHealth != null)
            {
                directEnemyHealth.Damage(directDamage, DamageType.Gun);
            }

            Collider[] enemyColliders = Physics.OverlapSphere(hit.point, explosionRadius);

            // Apply explosion damage to each enemy in the new hit enemies array
            foreach (Collider hitCollider in enemyColliders)
            {
                float distance = Vector3.Distance(hit.point, hitCollider.transform.position);
                float aoeDamage = Mathf.Lerp(explosionMinDamage, explosionMaxDamage, distance / explosionRadius);

                // Apply damage to the enemy
                EnemyHealth enemyHealth = hitCollider.GetComponentInParent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.Damage(aoeDamage, DamageType.Gun);
                }

            }

            //Turn everything off
            Invoke("TurnOffLaser", 0.3f);
        }

        else //if the gun does not hit anything, still draw line just make end point its 'max range'
        {
            Vector3 targetPosition = gunBarrelEnd.position + gunBarrelEnd.forward * maxRange;

            laserLine.enabled = true;
            laserLine.SetPosition(0, gunBarrelEnd.position);
            laserLine.SetPosition(1, targetPosition);

            Invoke("TurnOffLaser", 0.3f);
        }
        
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
