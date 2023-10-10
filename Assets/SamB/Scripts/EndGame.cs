using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    //static references for other scripts to use
    public static Transform player; //Where the player is.
    public static Vector3 playerPosition; // player's Vector3 position
    public static PlayerHealth playerHealth; // reference to player's health for all scripts to easily use 
    public static Collider playerCollider; //players collider, mainly for enemyMovement and projectiles
    public static GameObject playerObject; //used for enemymovement

    public StartingSequence startingSequence;

    public Text hologramText;



    private void Awake()
    {
        player = GetComponent<Transform>();
        playerCollider = GetComponentInChildren<BoxCollider>(); //getting the box collider thats a child of the ship, with this script on it
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
        if (currentHealth <= 0f)   // Player health is zero or negative, reload game
        {
            hologramText.gameObject.SetActive(true);
            hologramText.text = "The ship has lost too much health. We need to repair and try again.";
            Debug.Log("Ship has been destroyed");

            startingSequence.StartCoroutine(startingSequence.FadeOutBlack());

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
        hologramText.gameObject.SetActive(false);
        SceneManager.LoadScene(1);

    }
}
