using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Responsible for moving enemy bugs towards the player. Moves bugs towards players in a "zigzag" or a "curvy line", but haven't been able to figure that out 
/// yet. Also responsible for calling attack functions when the enemy is cloe enough to the player, to save update checks on multiple other scripts.
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    public float minSpeed = 1.0f; // Minimum movement speed
    public float maxSpeed = 5.0f; // Maximum movement speed
    public float speedChangeInterval = 2.0f; // Time interval for changing speed
    public float moveSpeed; // stores move speed
    public float attackRange = 10.0f;
    public float attackCooldown = 2.0f;

    private float initialSpeed; // The initial movement speed
    private float speedChangeTimer = 0f; // Timer for speed changes
    private bool isSpeedIncreasing = true; // Flag to track speed direction

    //bools to determine what enemy type, for attack calls
    public bool IsSoldier;
    public bool IsArtillery;
    public bool IsDrifter; 

    //static references for other scripts to use
    public static Transform player; //static so other scripts know which instance to refrence, also just easier to do this way for other scripts
    public static Vector3 playerPosition; // player's position
    public static PlayerHealth playerHealth; // reference to player's health for all scripts to easily use 

    private Animator animator;
    private Vector3 initialPosition;
    private float attackTimer = 0f; //the clock that tracks attackcooldown time
    private bool isAttacking = false;

    private float timeOffset; // Gives each enemy a slightly different movement pattern




    private void Awake()
    {
        player = FindObjectOfType<PlayerHealth>().transform; // Get the Transform component of the player
        animator = GetComponent<Animator>();
        float timeOffset = Random.Range(0f, 2f * Mathf.PI);
        initialPosition = transform.position - new Vector3(timeOffset, 0f, 0f); // Adjust initial position based on offset

        // Check if playerPosition is set yet
        if (playerPosition == Vector3.zero)
        {
            // Find the player object by its name
            GameObject playerObject = GameObject.Find("Ship");
            if (playerObject != null)
            {
                // Get the player's position
                playerPosition = playerObject.transform.position;
            }
        }

        // Find the object with the PlayerHealth script
        if (playerHealth == null)
        {
            playerHealth = FindObjectOfType<PlayerHealth>();
            if (playerHealth != null)
            {
                // Get the PlayerHealth component of the object with PlayerHealth script
                playerHealth = playerHealth.GetComponent<PlayerHealth>();
            }
        }

        initialSpeed = moveSpeed; // Store the initial movement speed

        // Calculate the direction from the enemy to the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Calculate the rotation to look at the player
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);

        // Apply the rotation to the enemy's transform
        transform.rotation = lookRotation;

    }



    private void Update()
    {
        if (player == null)
            return; // No player found, dont do code or else you get errors

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            isAttacking = true;
            // Stop moving towards player when attacking
            HandleAttack();
        }
        else
        {
            // Update the speed periodically
            speedChangeTimer += Time.deltaTime;
            if (speedChangeTimer >= speedChangeInterval)
            {
                // Toggle speed direction
                isSpeedIncreasing = !isSpeedIncreasing;
                speedChangeTimer = 0f;
            }
            
            // Adjust the movement speed based on the speed direction
            moveSpeed = isSpeedIncreasing ? Mathf.Lerp(minSpeed, maxSpeed, speedChangeTimer / speedChangeInterval) : Mathf.Lerp(maxSpeed, minSpeed, speedChangeTimer / speedChangeInterval);

            // Move towards the player
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            isAttacking = false;
        }

    }
    
    private void HandleAttack()
    {
        // Update attackTimer
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {

            //reset timer
            attackTimer = 0f;

            //call the appropriate attack
            EnemySoldier soldier = GetComponent<EnemySoldier>();
            EnemyArtillery artillery = GetComponent<EnemyArtillery>();
            EnemyDrifter drifter = GetComponent<EnemyDrifter>();

            if (IsArtillery)
            {
                artillery.Attack();
            }
            else if (IsSoldier)
            {
                soldier.Attack();
            }
            else if (IsDrifter)
            {
                drifter.Attack();
            }
            else
            {
                return;
            }
            // Add more conditions for other enemy types if needed
        }


    }

}


 