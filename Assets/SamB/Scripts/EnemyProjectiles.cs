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
    //private Vector3 playerPosition = EndGame.playerPosition;
    private PlayerHealth playerHealth = EndGame.playerHealth;


    private void Start()
    {
         initialPosition = transform.position;
    }

    void Update()
    {
        // Check if the projectile has traveled the maximum distance
        if (Vector3.Distance(initialPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collision is with a player collider
        if (other == player.GetComponent<Collider>())
        {
            // Damage the player and destroy the projectile
            Damage();
            Destroy(gameObject);
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

    /*
    private void CheckCollisionWithPlayer()
    {

        // Calculate the distance between the projectile and the player
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);

        // Define a collision threshold
        float collisionThreshold = 3.5f;

        // If the distance is within the collision threshold, consider it a hit
        if (distanceToPlayer <= collisionThreshold)
        {
            PlayerHealth playerHealth = EndGame.playerHealth;

            if (playerHealth != null)
            {
                playerHealth.Damage(rangedDamage, DamageType.Enemy);
            }

            Destroy(gameObject); // Destroy the projectile on collision
        }
    }
    */


}
