﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Responsible for the Exploder bugs attack function (which is called by EnemyMovement when in range)
/// </summary>
public class EnemyExploder : MonoBehaviour
{
    public float explosionDelay = 3f;
    public float explosionRadius = 5f;
    public int explosionDamage = 50;

    // Get reference to playerhealth
    //PlayerHealth playerHealth = ShipManager.playerHealth;

    public AudioClip soldierAttack;
    private AudioSource audioSource;


    private Health health;


    public ParticleSystem suicideExplosionParticle;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soldierAttack;
        health = GetComponent<Health>();
    }

    //trigger explosion (AKA its attack)
    public void Attack()
    {
        Invoke("Sacrifice", explosionDelay);
        
      
    }
    
    private void Sacrifice()
    {
        WaveManager.currentEnemies.Remove(gameObject);

        // Find all colliders within the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (var collider in colliders)
        {
            // Check if one of those colliders is the ships health
            PlayerHealth health = collider.GetComponent<PlayerHealth>();

            if (health != null)
            {
                // Apply damage to objects within the explosion radius
                health.Damage(explosionDamage, DamageType.Enemy);
            }
        }

        //Instantiate explosion VFX or play sound effects
        audioSource.Play();

        ParticleSystem explosionParticle = Instantiate(suicideExplosionParticle, transform.position, Quaternion.identity);
        Destroy(explosionParticle.gameObject, explosionParticle.main.duration);

        // Destroy the exploding enemy

        health.Destroy();
    }

}
