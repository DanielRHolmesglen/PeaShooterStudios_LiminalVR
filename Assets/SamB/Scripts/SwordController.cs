﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Respnsible for tracking sword collisions and dealing damage with it. f
/// </summary>
public class SwordController : MonoBehaviour
{
    public float swordDamage = 20f;

    private void OnCollisionEnter(Collision collision)
    {
        // Get the EnemyHealth component from the hit enemies. This is also used in a check to make sure the thing hit was an enemy. 
        EnemyHealth enemyHealth = collision.collider.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            // Do damage, and pass in the sword enum so EnemyHealth knows what type of damage it is
            enemyHealth.Damage(swordDamage, DamageType.Sword);
        }
    }

}
