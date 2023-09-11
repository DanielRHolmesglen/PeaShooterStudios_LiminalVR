using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    //static references for other scripts to use
    public static Transform player; //Where the player is.
    public static Vector3 playerPosition; // player's Vector3 position
    public static PlayerHealth playerHealth; // reference to player's health for all scripts to easily use 
    public static Collider playerCollider; //players collider, mainly for monvment and projectiles
    public static GameObject playerObject; //used for enemymovement



    private void Awake()
    {
        player = GetComponent<Transform>();
        playerCollider = GetComponent<BoxCollider>();
        playerObject = gameObject;
        playerHealth = GetComponent<PlayerHealth>();
        playerPosition = transform.position;

        if (playerHealth != null)
        {
            // Subscribe to the health change event
            playerHealth.OnHealthChanged += HandleHealthChanged;
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
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= HandleHealthChanged;
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("SamB");

    }
}
