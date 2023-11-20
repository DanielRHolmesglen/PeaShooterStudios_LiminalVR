using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Is in charge of managing the ship the player is defending, and 'resetting' or 'ending the game' with defeat if its destroyed.
/// </summary>

public class ShipManager : MonoBehaviour
{
    //static references for other scripts to use
    public static Transform player; //Where the SHIP is.
    public static Vector3 playerPosition; // ships's Vector3 position+
    public static PlayerHealth playerHealth; // reference to ships's health for all scripts to easily use 

    public static Collider[] PlayerColliders; //static reference for ship generator colliders, mainly for enemyMovement and projectiles
    public Collider[] setPlayerColliders; //assign them in inspector

    public static GameObject playerObject; //used for enemymovement

    public StartingSequence startingSequence; 
    public Text hologramText;


    public float resetDelay = 3f; // Delay before resetting the game
    public Image fadeImage;
    public Text lossText;
    public AudioSource lossSound;

    public WaveManager waveManager;



    private void Awake()
    {
        player = GetComponent<Transform>();
        //playerCollider = GetComponentInChildren<ShipCollidersTag>(); //getting the script tag of "ShipTargetCollider", with this component on it. It's children's colliders should also count.
        PlayerColliders = setPlayerColliders;
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

            startingSequence.StartCoroutine(startingSequence.FadeToBlack());

            Invoke("ReloadScene", 5);

            // Play loss sound
            lossSound.Play();

            // Reset the game
            ResetGame();

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
        SceneManager.LoadScene(0);

    }
    
    private void ResetGame()
    {
        playerHealth.currentHealth = 200;

        //waveManager.currentWave =  
        // Reset defendedObject's health
        // ...

        //foreach gameobject in WaemManager.enemies
        // Remove active enemies
        // ...

        // Reset any other game state
        // ...

        // Restart the game or transition to a menu, etc.
        // ...
    }



}
