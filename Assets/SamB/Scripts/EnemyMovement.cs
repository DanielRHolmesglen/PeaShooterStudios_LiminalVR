﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Responsible for moving enemy bugs towards the player. Moves bugs towards players in a "zigzag" or a "curvy line", but haven't been able to figure that out 
/// yet. Also responsible for calling attack functions when the enemy is cloe enough to the player, to save update checks on multiple other scripts.
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    public float minSpeed = 1.0f; // Minimum movement speed
    public float maxSpeed = 5.0f; // Maximum movement speed
    public float speedChangeInterval = 2.0f; // Time interval for changing speed
    private float timeOffset; // Gives each enemy a slightly different movement pattern
    public float moveSpeed; // stores move speed
    public float attackRange = 10.0f;
    public float attackCooldown = 2.0f;

    //bools to determine what enemy type, for attack calls
    public bool IsSoldier;
    public bool IsArtillery;
    public bool IsDrifter;

    private Transform player = EndGame.player;
    private GameObject playerObject = EndGame.playerObject;
    private PlayerHealth playerHealth = EndGame.playerHealth;
    private Collider playerCollider = EndGame.playerCollider;

    private Animator animator;
    private NavMeshAgent navMeshAgent;

    private float attackTimer = 0f; //the clock that tracks attackcooldown time

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        //animator = GetComponent<Animator>();

        // Set the initial NavMeshAgent speed/attack range
        navMeshAgent.speed = moveSpeed;
        navMeshAgent.stoppingDistance = attackRange;

        InvokeRepeating("ChangeSpeed", 0f, speedChangeInterval);

        navMeshAgent.SetDestination(player.position);

    }

    //changing the speed every 2 seconds to make movement feel not so telegraphed
    private void ChangeSpeed()
    {
        navMeshAgent.speed = Random.Range(minSpeed, maxSpeed);
    }

    private void Update()
    {
        //making sure there is an enemy to move towards
        if (playerObject == null)
            return;

        // Calculate the distance between the enemy's position and the player's closest point on the collider
        float distanceToPlayer = Vector3.Distance(transform.position, playerCollider.ClosestPoint(transform.position));


        // Find the nearest NavMesh point to the player's position
        Vector3 nearestNavMeshPoint = FindNearestNavMeshPoint(player.position);

        // Set the destination to the nearest NavMesh point
        navMeshAgent.SetDestination(nearestNavMeshPoint);


        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            // In range, so attack
            HandleAttack();
        }
        else
        {
            // Set the destination to the player's position
            navMeshAgent.SetDestination(player.position);
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

    private Vector3 FindNearestNavMeshPoint(Vector3 position)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, Mathf.Infinity, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return position; // Return the original position if no valid NavMesh point is found
    }


}


 