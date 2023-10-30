using System.Collections;
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
    private bool isInAttackRange = false;
    private float timeOffset; // Gives each enemy a slightly different movement pattern
    public float moveSpeed; // stores move speed
    public float attackRange = 10.0f;
    public float attackCooldown = 2.0f;

    float distanceToPlayer;
    private Collider closestPlayerCollider;


    //bools to determine what enemy type, for attack calls
    public bool IsSoldier;
    public bool IsArtillery;
    public bool IsDrifter;

    //private Transform player, as the closest one is calculated for each enemy
    private Collider[] playerColliders = EndGame.PlayerColliders;

    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private ScorpionFiniteStateMachine scorpianFSM;

    private float attackTimer = 0f; //clock that tracks attack cooldowns

    private void Start()
    {
        //get enemies navmesh refrence and players reference for position
        navMeshAgent = GetComponent<NavMeshAgent>();
        scorpianFSM = GetComponent<ScorpionFiniteStateMachine>();
        //player = EndGame.player;
        //animator = GetComponent<Animator>();

        // Set the initial NavMeshAgent speed/attack range
        navMeshAgent.speed = moveSpeed;
        navMeshAgent.stoppingDistance = attackRange;

        InvokeRepeating("ChangeSpeed", 0f, speedChangeInterval);

        /*
        // Initially calculate the closest point on the NavMesh for each collider and choose the closest one
        Vector3 closestPointOnNavMesh = FindClosestNavMeshPoint(playerCollider[2].transform.position);
        float closestDistance = Vector3.Distance(transform.position, closestPointOnNavMesh);

        for (int i = 1; i < playerCollider.Length; i++)
        {
            Vector3 pointOnNavMesh = FindClosestNavMeshPoint(playerCollider[i].transform.position);
            float distancetoPlayer = Vector3.Distance(transform.position, pointOnNavMesh);

            if (distancetoPlayer < closestDistance)
            {
                closestPointOnNavMesh = pointOnNavMesh;
                closestDistance = distancetoPlayer;
            }
        }
        

        // Set the NavMeshAgent's destination to the closest point on the NavMesh
        navMeshAgent.SetDestination(closestPointOnNavMesh);
        */
        StartCoroutine(CalculateDistanceToPlayer());
        

        /*
        // Initially, find the closest point on the NavMesh to the ship's collider, then we use that to find the best place on the navmesh to move the enemy towards
        Vector3 closestPointOnShipCollider = playerCollider.ClosestPoint(transform.position);
        Vector3 closestPointOnNavMesh = FindClosestNavMeshPoint(closestPointOnShipCollider);

        // Set the NavMeshAgent's destination to the closest point on the NavMesh
        navMeshAgent.SetDestination(closestPointOnNavMesh);
        */


    }

    //changing the speed every 2 seconds to make movement feel not so linear
    private void ChangeSpeed()
    {
        navMeshAgent.speed = Random.Range(minSpeed, maxSpeed);
    }

    private void Update()
    {
        // Update attackTimer
        attackTimer += Time.deltaTime;

        /*
        // Calculate the distance between the enemy's position and the player's closest point on the collider
        float distanceToPlayer = Vector3.Distance(transform.position, playerCollider.ClosestPoint(transform.position));
        //Debug.Log("Distance to player: " + distanceToPlayer); 


        // Check if the target is in attack range
        bool isInAttackRange = distanceToPlayer <= attackRange;

        // If the enemy is in attack range, stop moving and handle the attack
        if (isInAttackRange)
        {
            navMeshAgent.isStopped = true;
            HandleAttack();
            //Debug.Log("Player is in attack range!"); 
        }
        else
        {
            // If not in attack range, continue moving towards the player
            navMeshAgent.isStopped = false;
            FindClosestNavMeshPoint(player.position);
            //Debug.Log("Enemy is not in attack range.");
        }
        */

    }
    
    private void HandleAttack()
    {

        //Get references to appropriate attacks
        EnemySoldier soldier = GetComponent<EnemySoldier>();
        EnemyArtillery artillery = GetComponent<EnemyArtillery>();
        EnemyDrifter drifter = GetComponent<EnemyDrifter>();

        //if timer is at respect attack cooldown time, then execute attack and reset timer
        if (attackTimer >= attackCooldown)
        {
            // Reset the timer
            attackTimer = 0f;

            // Call the appropriate attack based on enemy type
            if (IsArtillery)
            {
                artillery.Attack();
                Debug.Log("Artillery attack called");
            }
            else if (IsSoldier)
            {
                soldier.Attack();
                Debug.Log("Soldier attack called");

                if (scorpianFSM.currentState != ScorpionFiniteStateMachine.States.ATTACKING)
                {
                    scorpianFSM.currentState = ScorpionFiniteStateMachine.States.ATTACKING;
                }
            }
            else if (IsDrifter)
            {
                drifter.Attack();
                Debug.Log("Drifter attack called");
            }
            else
            {
                Debug.Log("No  enemy type found");
                return;
            }
           
            // Add more conditions for other enemy types
        }

    }

    // Helper function to set the NavMeshAgent's destination while making sure its actually hitting a point on a navmesh
    private Vector3 FindClosestNavMeshPoint(Vector3 position)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, Mathf.Infinity, NavMesh.AllAreas))
        {
            return hit.position;
        }

        // If no valid NavMesh point is found, return the original position
        Debug.Log("No valid navmesh point found");
        return position;
    }

    private IEnumerator CalculateDistanceToPlayer()
    {
        WaitForSeconds waitTime = new WaitForSeconds(1.0f); // Update every 1 second

        while (true)
        {
            
                // Find the closest point on the ship's collider(s) to the enemy's position
                float closestDistance = float.MaxValue;

                foreach (Collider targetCollider in playerColliders)
                {
                    float distance = Vector3.Distance(transform.position, targetCollider.ClosestPoint(transform.position));

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPlayerCollider = targetCollider;


                    }
                }

            float distanceToPlayer = closestDistance;


                bool isInAttackRange = distanceToPlayer <= attackRange;

                if (isInAttackRange)
                {
                    navMeshAgent.isStopped = true;
                    HandleAttack();
                }
                else
                {
                    navMeshAgent.isStopped = false;
                    navMeshAgent.SetDestination(closestPlayerCollider.transform.position);
                }


            yield return waitTime;
        }

    }








}


