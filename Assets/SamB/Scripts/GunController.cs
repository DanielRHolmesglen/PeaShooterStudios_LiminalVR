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
public class GunController : MonoBehaviour 
{
    public AudioClip laserFireSound;
    private AudioSource audioSource;

    public float minChargeTime = 0.5f;
    public float maxChargeTime = 1.5f;
    public float minDamage = 5f;
    public float maxDamage = 25f;
    public float maxRange = 3f;

    public float chargeTimer;
    public bool isCharging;

    private LineRenderer laserLine; //reference to line renderer for laser
    public ParticleSystem chargingParticles; // Reference to charging particle effect
    public Transform gunBarrelEnd;



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

        //start charging gun when you press down
        if (primaryInput.GetButton(VRButton.One) || Input.GetMouseButton(0))
        {
            //turn the bool on 
            isCharging = true;
            chargingParticles.Play();


        }

        if (primaryInput.GetButtonUp(VRButton.One) || Input.GetMouseButtonUp(0)) //when mouse button released, fire laser

        {
            if (chargeTimer >= minChargeTime) //fire laser if the minimum charge time was reached
            {
                isCharging = false;
                FireLaser(chargeTimer);
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
    }


    private void FireLaser(float chargeTime)
    {
        //reset charge timer
        chargeTimer = 0f;

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
    }

    private void TurnOffLaser()
    {
        laserLine.enabled = false;
    }


}
