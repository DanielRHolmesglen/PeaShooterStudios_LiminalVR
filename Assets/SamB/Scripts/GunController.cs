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
    public float minChargeTime = 0.5f;
    public float maxChargeTime = 1.5f;
    public float minDamage = 5f;
    public float maxDamage = 25f;

    public float chargeTimer;
    public bool isCharging;

    private LineRenderer laserLine; //reference to line renderer for laser
    public ParticleSystem chargingParticles; // Reference to charging particle effect


    private void Start()
    {
        laserLine = GetComponent<LineRenderer>();         // Find  Line Renderer on the gun
        chargingParticles = GetComponentInChildren<ParticleSystem>();
        laserLine.enabled = false; // Disable the Line Renderer initially
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

        //when mouse button released, fire laser
        if (primaryInput.GetButtonUp(VRButton.One) || Input.GetMouseButtonUp(0))
        {
            if (chargeTimer >= minChargeTime)
            {
                isCharging = false;
                FireLaser(chargeTimer);
                chargingParticles.Stop();

            }

            else
            {
                isCharging = false;
                chargeTimer = 0f;
                chargingParticles.Stop();

            }
        }

        if (isCharging)
        {
            // charging sound
            chargingParticles.Play();
            chargeTimer += Time.deltaTime;
            chargingParticles.Play();

        }
    }


    private void FireLaser(float chargeTime)
    {
        //reset charge timer
        chargeTimer = 0f;

        //if the raycast is hitting an enemy 
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
        {
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();

            // Enable the laser (Line Renderer) and set its positions
            laserLine.enabled = true;
            laserLine.SetPosition(0, transform.position);
            laserLine.SetPosition(1, hit.point);
            Invoke("TurnOffLaser", 0.3f);

            if (enemyHealth != null)
            {
                float damage = Mathf.Lerp(minDamage, maxDamage, (chargeTime - minChargeTime) / (maxChargeTime - minChargeTime));


                enemyHealth.Damage(damage, DamageType.Gun);
            }
        }
    }

    private void TurnOffLaser()
    {
        laserLine.enabled = false;
    }


}
