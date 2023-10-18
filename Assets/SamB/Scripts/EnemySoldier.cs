using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Responsible for the Soldier bugs attack function (which is called by EnemeyMovement)
/// </summary>
public class EnemySoldier : MonoBehaviour
{
    public float meleeDamage = 10f;

    // Get reference to playerhealth
    PlayerHealth playerHealth = EndGame.playerHealth;

    public AudioClip soldierAttack;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soldierAttack;
    }

    //this is called by enemy movement when its close enough to attack
    public void Attack()
    {

        if (playerHealth != null)
        {
            //do damage to health script. "call script + function(amount, damage type)"
            playerHealth.Damage(meleeDamage, DamageType.Enemy);
        }

        audioSource.Play();
        // Play attack animation
    }

}
