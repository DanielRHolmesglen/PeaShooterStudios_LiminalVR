using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    //reference static reference to playerhealth 
    PlayerHealth playerHealth = EnemyMovement.playerHealth;

    private void Start()
    {
    
        if (playerHealth != null)
        {
            // Subscribe to the health change event
            playerHealth.OnHealthChanged += HandleHealthChanged;
        }
    }

    private void OnDestroy()
    {
   
        // Unsubscribe from the health change event when the GameObject is destroyed
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= HandleHealthChanged;
        }
    }

    private void HandleHealthChanged(float currentHealth, float maxHealth)
    {
        if (currentHealth <= 0f)
        {
            // Player health is zero or negative, quit the game
            Application.Quit();
        }
    }
}
