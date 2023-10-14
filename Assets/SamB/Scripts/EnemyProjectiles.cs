using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Controls projectiles collisions and damage that have been fired by enimies  
/// </summary>
public class EnemyProjectiles : MonoBehaviour
{
    public float rangedDamage = 5f;
    public float maxDistance = 10f;

    private Vector3 initialPosition; //used to check when to destroy the projectile. 
    private Transform player = EndGame.player;
    private Collider[] playerCollider = EndGame.PlayerColliders;
    private PlayerHealth playerHealth = EndGame.playerHealth;

    public AudioClip projectileHit;
    private AudioSource audioSource;


    private void Start()
    {
        initialPosition = transform.position;

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = projectileHit;
    }

    void Update()
    {
        // Check if the projectile has traveled the maximum distance
        if (Vector3.Distance(initialPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision involes a player collider
        foreach (Collider playerCollider in playerCollider)
        {
            if (collision.collider == playerCollider)
            {
                // Damage the player and destroy the projectile
                Damage();
                Invoke("DestroyProjectile", 0.3f);

                audioSource.Play();

            }
        }
            
    }

    public void Damage()
    {
        if (playerHealth != null)
        {
            //do damage to health script "call script + function(amount, damage type)"
            playerHealth.Damage(rangedDamage, DamageType.Enemy);
        }
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);

    }

}
