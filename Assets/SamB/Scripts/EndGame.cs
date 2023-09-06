using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    private PlayerHealth playerHealthEnd; // reference to player's health for all scripts to easily use 

    private void Start()
    {
        // Find the object with the PlayerHealth script
        //cannot use static reference as this is executed before the first enemy is spawned
        if (playerHealthEnd == null)
        {
            playerHealthEnd = FindObjectOfType<PlayerHealth>();
            if (playerHealthEnd != null)
            {
                // Get the PlayerHealth component of the object with PlayerHealth script
                playerHealthEnd = playerHealthEnd.GetComponent<PlayerHealth>();
            }
        }


        if (playerHealthEnd != null)
        {
            // Subscribe to the health change event
            playerHealthEnd.OnHealthChanged += HandleHealthChanged;
        }
    }

    
    private void HandleHealthChanged(float currentHealth, float maxHealth)
    {
        if (currentHealth <= 0f)
        {
            // Player health is zero or negative, reload game
            Debug.Log("The ship was destroyed! You lose :(");

            Invoke("ReloadScene", 2);
        }
    }

    private void OnDestroy()
    {

        // Unsubscribe from the health change event when the Ship is destroyed
        if (playerHealthEnd != null)
        {
            playerHealthEnd.OnHealthChanged -= HandleHealthChanged;
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("SamB");

    }
}
