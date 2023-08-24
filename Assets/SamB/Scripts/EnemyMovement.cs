using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public float attackRange = 2.0f;
    public float attackCooldown = 2.0f;

    private Transform player;
    private Animator animator;
    private float attackTimer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player == null)
            return; // No player found

        MoveTowardsPlayer();

        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            if (attackTimer <= 0)
            {
                Attack();
                attackTimer = attackCooldown;
            }
            else
            {
                attackTimer -= Time.deltaTime;
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        direction.Normalize();
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

        if (animator != null)
            animator.SetFloat("Speed", direction.magnitude);
    }

    private void Attack()
    {
        // Implement enemy attack logic here
    }
}
