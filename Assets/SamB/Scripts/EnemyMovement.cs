using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Responsible for moving enemy bugs towards the Ship. Also responsible for calling attack functions and make the bug stop moving when the bug is close enough to the ship, to save update checks on multiple other scripts.
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
    private float attackTimer = 0f; //clock that tracks attack cooldowns

    float distanceToPlayer;
    private Collider closestPlayerCollider;

    //bools to determine what enemy type, for attack calls
    public bool isSoldier;
    public bool isArtillery;
    public bool isDrifter;
    public bool isExploder;



    //private Transform player, as the closest one is calculated for each enemy
    private Collider[] playerColliders = ShipManager.PlayerColliders;

    private NavMeshAgent navMeshAgent;

    private Animator anim;
    private ScorpionFiniteStateMachine scorpianFSM;
    //private DrifterFiniteStateMachine drifterFSM;

    private void Start()
    {
        //get enemies navmesh refrence and players reference for position
        navMeshAgent = GetComponent<NavMeshAgent>();
        scorpianFSM = GetComponent<ScorpionFiniteStateMachine>();
        //player = ShipManager.player;
        anim = GetComponentInChildren<Animator>();

        // Set the initial NavMeshAgent speed/attack range
        navMeshAgent.speed = moveSpeed;
        navMeshAgent.stoppingDistance = attackRange;

        InvokeRepeating("ChangeSpeed", 0f, speedChangeInterval);
        
        StartCoroutine(CalculateDistanceToPlayer());
      
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

        if(!isInAttackRange)
        {
            if (isArtillery) anim.Play("ArtilleryMOVE");
            else if (isSoldier) anim.Play("ScorpionMOVE");
            else if (isExploder) anim.Play("ScorpionMOVE");
            else if (isDrifter) anim.Play("DrifterMOVE");
        }


    }
    
    private IEnumerator CalculateDistanceToPlayer()
    {
        WaitForSeconds waitTime = new WaitForSeconds(1.0f); // Update every 1 second

        while (true)
        {
                // Finding the closest point on the ship's collider(s) to the enemy's position.
                //WAS USED WHEN WE HAD TO DEFEND MULTIPLE 'GENERATORS'. Now redundant. 
                float closestDistance = float.MaxValue;

            if (playerColliders != null)
            {
                foreach (CapsuleCollider targetCollider in playerColliders)
                {
                    float distance = Vector3.Distance(transform.position, targetCollider.ClosestPoint(transform.position));

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPlayerCollider = targetCollider;
                    }
                }
            }
            else
            {
                yield break;
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


    private void HandleAttack()
    {

        //Get references to appropriate attacks
        EnemySoldier soldier = GetComponent<EnemySoldier>();
        EnemyArtillery artillery = GetComponent<EnemyArtillery>();
        EnemyDrifter drifter = GetComponent<EnemyDrifter>();
        EnemyExploder exploder = GetComponent<EnemyExploder>();


        //if timer is at respect attack cooldown time, then execute attack and reset timer
        if (attackTimer >= attackCooldown)
        {
            // Reset the timer
            attackTimer = 0f;

            // Call the appropriate attack based on enemy type
            if (isArtillery)
            {
                artillery.Attack();
                Debug.Log("Artillery attack called");

                anim.Play("ArtilleryATTACK");
            }
            else if (isSoldier)
            {
                soldier.Attack();
                Debug.Log("Soldier attack called");

                anim.Play("ScorpionATTACK");
            }
            else if (isExploder)
            {
                exploder.Attack();
                Debug.Log("Exploder attack called");

                anim.Play("ScorpionATTACK");
                /*if (scorpianFSM.currentState != ScorpionFiniteStateMachine.States.ATTACKING)
                {
                    scorpianFSM.currentState = ScorpionFiniteStateMachine.States.ATTACKING;
                }*/
            }
            else if (isDrifter)
            {
                drifter.Attack();
                Debug.Log("Drifter attack called");

                anim.Play("DrifterATTACK");
                //anim.Play("Attack");

            }

            else
            {
                Debug.Log("No  enemy type found");
                return;

            }

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







}


