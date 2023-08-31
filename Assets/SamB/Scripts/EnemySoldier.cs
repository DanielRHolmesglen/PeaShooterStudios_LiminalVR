using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Responsible for the Soldier bugs attack function (which is called by EnemeyMovement)
/// </summary>
public class EnemySoldier : MonoBehaviour
{
    public float meleeDamage = 10f;

    //this is called by enemy movement when its close enough to attack
    public void Attack()
    {
        // Get the player's health script
        PlayerHealth playerHealth = EnemyMovement.playerHealth;

        if (playerHealth != null)
        {
            //do damage to health script. "call script + function(amount, damage type)"
            playerHealth.Damage(meleeDamage, DamageType.Enemy);
        }

        // Play attack animation
    }

}
