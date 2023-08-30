using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public float attackRange = 10.0f;
    public float attackCooldown = 2.0f;
    public float sideToSideMovementRange = 1f;
    public float sideToSideMovementSpeed = 1f;

    public bool IsSoldier;
    public bool IsArtillery;
    public bool IsDrifter;

    //has to be static so other scripts know which instance of EnemyMovemt player variable to refrence, also makes game faster
    public static Transform player;
    public static Vector3 playerPosition; // Static Vector3 to store player's position
    public static PlayerHealth playerHealth; // Static reference to player's health for all scripts

    private Animator animator;
    private Vector3 initialPosition;
    private float attackTimer = 0f;
    private bool isAttacking = false;
    private bool isMovingTowardsPlayer = true; // Flag for movement direction
    private float sideToSideMovement = 3f;



    private void Start()
    {
        // Get the Transform component of the player
        player = FindObjectOfType<PlayerHealth>().transform;
        animator = GetComponent<Animator>();
        initialPosition = transform.position;

        // Check if playerPosition is not set yet
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
    }

    

    private void Update()
    {
        if (player == null)
            return; // No player found, dont do code or else you get errors

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            isAttacking = true;
            isMovingTowardsPlayer = false; // Stop moving towards player
        }
        else
        {
            // Calculate zigzag 
            sideToSideMovement = Mathf.PingPong(Time.time * sideToSideMovementSpeed, sideToSideMovementRange * 2) - sideToSideMovementRange;

            // Determine target position (AKA where palyer is)
            Vector3 targetPosition = isMovingTowardsPlayer ? player.position : initialPosition + new Vector3(sideToSideMovement, 0, 0);

            // Move towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            
            // Check if the enemy should stop moving
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMovingTowardsPlayer = !isMovingTowardsPlayer;
            }
            

            isAttacking = false;
        }



        // Perform attack logic
        if (isAttacking)
        {
            HandleAttack();
        }

        // Update playerPosition continuously
        if (playerPosition != Vector3.zero && playerPosition != player.position)
        {
            playerPosition = player.position;
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


 