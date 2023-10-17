using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Responsible for the Artilery bugs attack function (which is called by EnemeyMovement). Spawns a projectile and yeets it towards player.
/// </summary>
public class EnemyArtillery : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab of the projectile
    public Transform projectileSpawnPoint; // Where the projectile spawns
    public float projectileSpeed = 10f;
    private Transform player = EndGame.player;
    private Vector3 playerPosition = EndGame.playerPosition;

    //this is called by enemy movement when the enemy is close enough to attack. A bit different as this one needs to make a projectile.
    public void Attack()
    {
        // Create a new projectile
        GameObject newProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        // Get the projectile's rigidbody
        Rigidbody projectileRigidbody = newProjectile.GetComponent<Rigidbody>();

        // Shoot the projectile towards the player
        if (projectileRigidbody != null)
        {
            Debug.Log("Projectile instantiated successfully.");
            Vector3 directionToPlayer = (playerPosition - projectileSpawnPoint.position).normalized;
            projectileRigidbody.velocity = directionToPlayer * projectileSpeed;
        }
        //attack animations
    }

}
