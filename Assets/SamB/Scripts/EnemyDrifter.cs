using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Responsible for the Difter bugs attack function (which is called by EnemeyMovement)
/// </summary>
public class EnemyDrifter : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab of the projectile
    public Transform projectileSpawnPoint; // Where the projectile spawns
    public float projectileSpeed = 10f;
    private Transform player = ShipManager.player;
    private Vector3 playerPosition = ShipManager.playerPosition;

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



    /*

    public float flyingDamage = 7;

    public AudioClip drifterAttack;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = drifterAttack;
    }

    //this is called by enemy movement when its close enough to attack
    public void Attack()
    {
        // Get the player's health script
        PlayerHealth playerHealth = ShipManager.playerHealth;

        if (playerHealth != null)
        {
            //do damage to health script "call script + function(amount, damage type)"
            playerHealth.Damage(flyingDamage, DamageType.Enemy);
        }
        audioSource.Play();
        // Play attack animation
    }

    */
    
}
