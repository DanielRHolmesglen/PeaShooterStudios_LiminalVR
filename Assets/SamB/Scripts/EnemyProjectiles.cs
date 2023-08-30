using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectiles : MonoBehaviour
{
    public float rangedDamage = 5f;
    public float maxDistance = 10f; 

    private Vector3 initialPosition;
    private Transform player = EnemyMovement.player;
    private Vector3 playerPosition = EnemyMovement.playerPosition;

    public void Damage()
    {
        //Reference to the playehealth script. Referencing the enemymovement static one. 
        PlayerHealth playerHealth = EnemyMovement.playerHealth;

        if (playerHealth != null)
        {
            //do damage to health script "call script + function(amount, damage type)"
            playerHealth.Damage(rangedDamage, DamageType.Enemy);
        }
    }

    void Update()
    {
        // Check if the projectile has traveled the maximum distance
        if (Vector3.Distance(initialPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject); // Destroy the projectile
        }

        // Check for collision with the player
        CheckCollisionWithPlayer();
    }



    private void CheckCollisionWithPlayer()
    {

        // Calculate the distance between the projectile and the player
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);

        // Define a collision threshold based on the size of the player
        float collisionThreshold = 1.0f;

        // If the distance is within the collision threshold, consider it a hit
        if (distanceToPlayer <= collisionThreshold)
        {
            PlayerHealth playerHealth = EnemyMovement.playerHealth;

            if (playerHealth != null)
            {
                playerHealth.Damage(rangedDamage, DamageType.Enemy);
            }

            Destroy(gameObject); // Destroy the projectile on collision
        }
    }
}
